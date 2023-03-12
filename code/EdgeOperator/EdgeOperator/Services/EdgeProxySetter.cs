using cz.dvojak.k8s.EdgeOperator.Repository;
using k8s;
using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Services;

public class EdgeProxySetter : IEdgeProxySetter
{
    readonly private DeploymentRepositoryTemplate _deploymentRepositoryTemplate;

    public EdgeProxySetter(DeploymentRepositoryTemplate deploymentRepositoryTemplate)
    {
        _deploymentRepositoryTemplate = deploymentRepositoryTemplate;
    }
    
    public void SetAdditionalNetwork(V1Deployment deployment,string networkName)
    {
        deployment.Spec.Template.Metadata.Annotations.Add("k8s.v1.cni.cncf.io/networks",networkName);
    }

    public void SetOperatorLabels(V1Deployment deployment)
    {
        foreach (var e in _deploymentRepositoryTemplate.Labels)
        {
            deployment.Metadata.Labels.Add(e.Key,e.Value);
            deployment.Spec.Template.Metadata.Labels.Add(e.Key,e.Value);
        }
    }

    public void SetTolerations(V1Deployment deployment)
    {
        deployment.Spec.Template.Spec.Tolerations = _deploymentRepositoryTemplate.Tolerations;
    }
    
    public void SetNodeSelector(V1Deployment deployment)
    {
        deployment.Spec.Template.Spec.NodeSelector = _deploymentRepositoryTemplate.NodeSelectorLabels;
    }
    public void SetNode(V1Deployment deployment, V1Node node)
    {
        deployment.Spec.Template.Spec.NodeName = node.Metadata.Name;
    }

}