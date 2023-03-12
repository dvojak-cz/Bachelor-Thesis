using cz.dvojak.k8s.EdgeOperator.Definitions;
using cz.dvojak.k8s.EdgeOperator.Extends;
using k8s.Models;
using KubeOps.KubernetesClient.Entities;
using KubeOps.Operator.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace cz.dvojak.k8s.EdgeOperator.Entities;

[KubernetesEntity(Kind = "Connection",Group = Api.Group,ApiVersion = Api.ApiVersion)]
[EntityScope(EntityScope.Namespaced)]
public class ConnectionEntity : CustomKubernetesEntityRequiredSpec<ConnectionEntity.ConnectionSpec,
    ConnectionEntity.ConnectionStatus>
{
    public class ConnectionSpec
    {
        [Required] public string ServiceName { get; set; }
        [Required] public string DeviceName { get; set; }
        [Required] public IList<string> ApplicationNames { get; set; } = null!;

    }
    public class ConnectionStatus
    {
        public IList<string> Pods { get; set; } = null!;
        public bool Active = false;
        public bool Running = false;
    }
}