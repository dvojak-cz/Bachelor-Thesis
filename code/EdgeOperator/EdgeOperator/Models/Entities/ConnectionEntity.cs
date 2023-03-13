using cz.dvojak.k8s.EdgeOperator.Configuration.Definitions;
using cz.dvojak.k8s.EdgeOperator.Extends.KubeOps;
using k8s.Models;
using KubeOps.KubernetesClient.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace cz.dvojak.k8s.EdgeOperator.Models.Entities;

[KubernetesEntity(Kind = "Connection",Group = Api.Group,ApiVersion = Api.ApiVersion)]
[EntityScope]
public class ConnectionEntity : CustomKubernetesEntityRequiredSpec<ConnectionEntity.ConnectionSpec,
    ConnectionEntity.ConnectionStatus>
{
    public class ConnectionSpec
    {
        [Required] public string ServiceName { get; set; } = null!;
        [Required] public string DeviceName { get; set; } = null!;
        [Required] public IList<string> ApplicationNames { get; set; } = null!;

    }
    public class ConnectionStatus
    {
        public IList<string> Pods { get; set; } = null!;
        public bool Active = false;
        public bool Running = false;
    }
}