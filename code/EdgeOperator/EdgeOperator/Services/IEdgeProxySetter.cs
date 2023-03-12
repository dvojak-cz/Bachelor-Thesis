using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Services;

public interface IEdgeProxySetter
{
    void SetAdditionalNetwork(V1Deployment deployment,string networkName);
    void SetOperatorLabels(V1Deployment deployment);
    void SetTolerations(V1Deployment deployment);
    void SetNodeSelector(V1Deployment deployment);
    void SetNode(V1Deployment deployment, V1Node node);
}