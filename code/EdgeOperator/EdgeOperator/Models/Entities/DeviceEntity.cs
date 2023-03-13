using cz.dvojak.k8s.EdgeOperator.Configuration.Definitions;
using cz.dvojak.k8s.EdgeOperator.Extends.KubeOps;
using k8s.Models;
using KubeOps.KubernetesClient.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace cz.dvojak.k8s.EdgeOperator.Models.Entities;

[KubernetesEntity(Kind = "Device",Group = Api.Group,ApiVersion = Api.ApiVersion)]
[EntityScope(EntityScope.Cluster)]
public class DeviceEntity : CustomKubernetesEntityRequiredSpec<DeviceEntity.DeviceEntitySpec>
{
    public class DeviceEntitySpec
    {
        [Description("Name of the node where the device is connected to")]
        [Required]
        public string NodeName { get; set; } = string.Empty; 

        [Required] public bool IsUp { get; set; } = false;
        [Required] public string IpAddress { get; set; } = null!;
        public bool Ready = true;
    }

}