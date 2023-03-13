using cz.dvojak.k8s.EdgeOperator.Configuration.Definitions;
using cz.dvojak.k8s.EdgeOperator.Extends.KubeOps;
using k8s.Models;
using KubeOps.KubernetesClient.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace cz.dvojak.k8s.EdgeOperator.Models.Entities;

[KubernetesEntity(Kind = "Application",Group = Api.Group,ApiVersion = Api.ApiVersion)]
[EntityScope(EntityScope.Cluster)]
public class ApplicationEntity : CustomKubernetesEntityRequiredSpec<ApplicationEntity.ApplicationSpec, ApplicationEntity.ApplicationeEntityStatus>
{
    public class ApplicationSpec
    {
        [Required] public string Device { get; set; } = null!;
        [Required] public IList<Service> Services { get; set; } = null!;
        public class Service
        {
            public string Name { get; set; } = null!;

            [Required] public Protocol Protocol { get; init; }

            [Required]
            [RangeMinimum(Minimum = 0)]
            [RangeMaximum(Maximum = 65535)]
            public int Port { get; init; }

            [Required] public bool IsUp { get; set; } = false;
        }
    }
    public class ApplicationeEntityStatus
    {
        public bool Used = false;
    }
}