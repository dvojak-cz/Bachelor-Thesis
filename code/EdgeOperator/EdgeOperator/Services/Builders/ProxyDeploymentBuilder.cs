using cz.dvojak.k8s.EdgeOperator.Configuration.Options;
using k8s.Models;
using Microsoft.Extensions.Options;

namespace cz.dvojak.k8s.EdgeOperator.Services.Builders;

public class ProxyDeploymentBuilder : IProxyDeploymentBuilder
{
    readonly private DeploymentTemplateOption _deploymentTemplateOption;
    private V1Deployment _deployment;

    public ProxyDeploymentBuilder(IOptions<DeploymentTemplateOption> deploymentTemplateOption)
    {
        _deploymentTemplateOption = deploymentTemplateOption.Value;
        _deployment = new V1Deployment();
    }

    public V1Deployment Build() => _deployment;

    public IProxyDeploymentBuilder Reset()
    {
        _deployment = new V1Deployment();
        return this;
    }


    public IProxyDeploymentBuilder SetName(string name)
    {
        _deployment.Metadata.Name = name;
        return this;
    }

    public IProxyDeploymentBuilder SetDeploymentLabels()
    {
        _deployment.Metadata.Labels = _deployment.Metadata.Labels.Concat(_deploymentTemplateOption.Labels)
            .ToDictionary(x => x.Key,x => x.Value);
        return this;
    }

    public IProxyDeploymentBuilder SetPodLabels()
    {
        _deployment.Spec.Template.Metadata.Labels = _deployment.Spec.Template.Metadata.Labels
            .Concat(_deploymentTemplateOption.Labels).ToDictionary(x => x.Key,x => x.Value);
        return this;
    }

    public IProxyDeploymentBuilder SetPodNetwork(string networkName)
    {
        _deployment.Spec.Template.Metadata.Annotations.Add("k8s.v1.cni.cncf.io/networks",networkName);
        return this;
    }

    public IProxyDeploymentBuilder SetPodNodeSelector()
    {
        _deployment.Spec.Template.Spec.NodeSelector = _deploymentTemplateOption.NodeSelectorLabels;
        return this;
    }

    public IProxyDeploymentBuilder SetPodNode(V1Node node)
    {
        _deployment.Spec.Template.Spec.NodeName = node.Metadata.Name;
        return this;
    }

    public IProxyDeploymentBuilder SetPodTolerations()
    {
        _deployment.Spec.Template.Spec.Tolerations = _deploymentTemplateOption.Tolerations;
        return this;
    }

    public IProxyDeploymentBuilder SetContainers(IList<V1Container> containers)
    {
        _deployment.Spec.Template.Spec.Containers = containers;
        return this;
    }
}