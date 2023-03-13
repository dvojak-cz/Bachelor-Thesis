using cz.dvojak.k8s.EdgeOperator.Models.Entities;
using cz.dvojak.k8s.EdgeOperator.Services.Builders;
using k8s.Models;
using KubeOps.KubernetesClient;

namespace cz.dvojak.k8s.EdgeOperator.Services;

public class ProxyCreator : IProxyCreator
{
    readonly private IKubernetesClient _client;
    readonly private IProxyDeploymentBuilder _proxyDeploymentBuilder;
    readonly private IProxyContainerBuilder _proxyContainerBuilder;
    public ProxyCreator(
        IKubernetesClient client,
        IProxyDeploymentBuilder proxyDeploymentBuilder,
        IProxyContainerBuilder proxyContainerBuilder)
    {
        _client = client;
        _proxyDeploymentBuilder = proxyDeploymentBuilder;
        _proxyContainerBuilder = proxyContainerBuilder;
        
    }

    public string GetProxyName(ConnectionEntity entity) => $"proxy-{entity.Metadata.Name}";
    
    public async Task<V1Deployment> CreateProxyDeployment(ConnectionEntity entity,string networkName)
    {
        var device = await _client.Get<DeviceEntity>(entity.Spec.DeviceName);
        if (device is null)
            throw new NullReferenceException($"Device {entity.Spec.DeviceName} could not been found");
        var node = await _client.Get<V1Node>(device.Spec.NodeName);
        if (node is null)
            throw new NullReferenceException($"Node {device.Spec.NodeName} could not been found");
        var allApplications = await _client.List<ApplicationEntity>();
        var application = allApplications.Where(a => entity.Spec.ApplicationNames.Contains(a.Metadata.Name)).ToList();
        if (application.Count != entity.Spec.ApplicationNames.Count)
            throw new Exception("Not all applications could be found");


        
        foreach (var a in application.Select(applicationEntity => applicationEntity.Spec.Services))
            _proxyContainerBuilder.Add(device.Spec.IpAddress,a.ToArray());
        var containers = _proxyContainerBuilder.Build();

        var deployment = _proxyDeploymentBuilder
            .SetName(GetProxyName(entity))
            .SetDeploymentLabels()
            .SetPodLabels()
            .SetPodNetwork("")
            .SetPodNodeSelector()
            .SetPodNode(node)
            .SetPodTolerations()
            .SetContainers(containers)
            .Build();

        return deployment;
    }

}