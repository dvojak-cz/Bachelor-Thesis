using System.Collections.ObjectModel;
using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Repository;

public class DeploymentRepositoryTemplate
{
    public readonly IDictionary<string,string> Labels = new Dictionary<string,string>()
    {
        { "operator","edge-operator" }, { "operatorType","edge-proxy" },
    };

    public readonly IList<V1Toleration> Tolerations = new Collection<V1Toleration>()
    {
        new() { OperatorProperty = "Exists",Effect = "NoSchedule" },
        new() { OperatorProperty = "Exists",Effect = "NoExecute" }
    };

    public readonly IDictionary<string,string> NodeSelectorLabels = new Dictionary<string,string>()
    {
        { "edge","true" },
    };
}