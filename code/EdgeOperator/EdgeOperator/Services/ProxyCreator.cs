using cz.dvojak.k8s.EdgeOperator.Entities;
using k8s.Models;
using KubeOps.KubernetesClient;

namespace cz.dvojak.k8s.EdgeOperator.Services;

public class ProxyCreator
{
    readonly private IEdgeProxySetter _edgeProxySetter;
    readonly private IKubernetesClient _client;
    
    
    
    
    public ProxyCreator(IEdgeProxySetter edgeProxySetter,IKubernetesClient client)
    {
        _edgeProxySetter = edgeProxySetter;
        _client = client;
    }

    public string GetProxyName(ConnectionEntity entity) => $"proxy-{entity.Metadata.Name}";

    private void SetBasicMetadata(V1Deployment depl)
    {
        _edgeProxySetter.SetOperatorLabels(depl);
        _edgeProxySetter.SetTolerations(depl);
        _edgeProxySetter.SetNodeSelector(depl);
    }

    private void SetProxy()
    {
        
    }

    public async Task<V1Deployment> CreateProxyDeployment(ConnectionEntity entity, string network_name)
    {
        var depl = new V1Deployment();
        depl.Metadata.Name = GetProxyName(entity);
        SetBasicMetadata(depl);

        var device = await _client.Get<DeviceEntity>(entity.Spec.DeviceName); //todo null
        var node = await _client.Get<V1Node>(device.Spec.NodeName); //todo null
        _edgeProxySetter.SetNode(depl,node);
        _edgeProxySetter.SetAdditionalNetwork(depl,network_name);
        return depl;
    }

}