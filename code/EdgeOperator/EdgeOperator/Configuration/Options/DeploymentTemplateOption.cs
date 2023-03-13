using System.Collections.ObjectModel;
using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Configuration.Options;

public class DeploymentTemplateOption
{
    public const string DeploymentTemplate = "DeploymentTemplate";

    public IDictionary<string,string> Labels { get; set; } = new Dictionary<string,string>()
    {
        { "operator","edge-operator" }, { "operatorType","edge-proxy" },
    };

    public IList<V1Toleration> Tolerations { get; set; } = new Collection<V1Toleration>()
    {
        new() { OperatorProperty = "Exists",Effect = "NoSchedule" },
        new() { OperatorProperty = "Exists",Effect = "NoExecute" }
    };

    public IDictionary<string,string> NodeSelectorLabels { get; set; } = new Dictionary<string,string>()
    {
        { "edge","true" },
    };
}