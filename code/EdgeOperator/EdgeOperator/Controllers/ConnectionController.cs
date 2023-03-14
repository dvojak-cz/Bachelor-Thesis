using cz.dvojak.k8s.EdgeOperator.Models.Entities;
using cz.dvojak.k8s.EdgeOperator.Services;
using k8s.Models;
using KubeOps.KubernetesClient;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Controller.Results;
using KubeOps.Operator.Rbac;

namespace cz.dvojak.k8s.EdgeOperator.Controllers;

[EntityRbac(typeof(ConnectionEntity),Verbs = RbacVerb.All)]
[EntityRbac(typeof(DeviceEntity),Verbs = RbacVerb.All)]
[EntityRbac(typeof(ApplicationEntity),Verbs = RbacVerb.All)]
[EntityRbac(typeof(V1Deployment),Verbs = RbacVerb.All)]
public class ConnectionController : IResourceController<ConnectionEntity>
{
    readonly private ILogger<ConnectionController> _logger;
    readonly private IKubernetesClient _client;
    private readonly IProxyCreator _proxyCreator;

    public ConnectionController(
        ILogger<ConnectionController> logger,
        IKubernetesClient client,
        IProxyCreator proxyCreator)
    {
        _logger = logger;
        _client = client;
        _proxyCreator = proxyCreator;
    }

    public async Task<ResourceControllerResult?> ReconcileAsync(ConnectionEntity entity)
    {
        var deplTest = new V1Deployment()
        {
            Metadata = new V1ObjectMeta()
            {
                Name = "Foo",
            },
            Spec = new V1DeploymentSpec()
            {
                Selector = new V1LabelSelector()
                {
                    MatchLabels = new Dictionary<string,string>()
                    {
                        { "app","Foo" },
                    }
                },
                Template = new V1PodTemplateSpec()
                {
                    Metadata = new V1ObjectMeta()
                    {
                        Labels = new Dictionary<string,string>()
                        {
                            { "app","Foo" },
                        }
                    },
                    Spec = new V1PodSpec()
                    {
                        Containers = new List<V1Container>()
                        {
                            new()
                            {
                                Name = "Foo",
                                Image = "nginx",
                                Ports = new List<V1ContainerPort>()
                                {
                                    new()
                                    {
                                        ContainerPort = 80,
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
        try
        {
            await _client.Create(deplTest);
            _logger.LogInformation("deplymentNaplanovan");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message,e);
            _logger.LogError(e.Data.ToString(),e);
        }

        return null;
        ///////////////////////////


        if (!await ValidateConnectionEntity(entity))
        {
            _logger.LogWarning("Connection {connectionName} is not valid",entity.Metadata.Name);
            return null;
        }

        var depl = await _proxyCreator.CreateProxyDeployment(entity,"foo");
        _logger.LogInformation("deplymentVytvoren");
        try
        {
            await _client.Create(depl);
            _logger.LogInformation("deplymentNaplanovan");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message,e);
            _logger.LogError(e.Data.ToString(),e);
        }


        return null;
    }

    public Task StatusModifiedAsync(ConnectionEntity entity) => Task.CompletedTask;

    public Task DeletedAsync(ConnectionEntity entity)
    {
        var deploymentName = _proxyCreator.GetProxyName(entity);

        return _client.Delete<V1Deployment>(deploymentName);
    }


    private async Task<bool> ValidateConnectionEntity(ConnectionEntity entity)
    {
        var device = await _client.Get<DeviceEntity>(entity.Spec.DeviceName);
        if (device is null)
        {
            _logger.LogWarning("Device {deviceName} not found",entity.Spec.DeviceName);
            return false;
        }

        var applications = await _client.List<ApplicationEntity>();
        var requiredAppliacations =
            applications.Where(a => entity.Spec.ApplicationNames.Contains(a.Metadata.Name)).ToList();
        if (requiredAppliacations.Count() != entity.Spec.ApplicationNames.Count)
        {
            _logger.LogWarning("Application {applicationName} not found",entity.Spec.ApplicationNames);
            return false;
        }

        var valid = requiredAppliacations.All(a => a.Spec.Device == entity.Spec.DeviceName);
        if (!valid)
        {
            _logger.LogWarning("Application {applicationName} is not valid",entity.Spec.ApplicationNames);
            return false;
        }

        _logger.LogInformation("Connection {connectionName} is valid",entity.Metadata.Name);
        return true;
    }

}