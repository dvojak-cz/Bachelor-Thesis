using cz.dvojak.k8s.EdgeOperator.Entities;
using k8s.Models;
using KubeOps.KubernetesClient;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Controller.Results;
using KubeOps.Operator.Rbac;

namespace cz.dvojak.k8s.EdgeOperator.Controllers;

[EntityRbac(typeof(ConnectionEntity),Verbs = RbacVerb.All)]
[EntityRbac(typeof(DeviceEntity),Verbs = RbacVerb.Get)]
[EntityRbac(typeof(ApplicationEntity),Verbs = RbacVerb.Get)]
public class ConnectionController : IResourceController<ConnectionEntity>
{
    private readonly ILogger<ConnectionController> _logger;
    private readonly IKubernetesClient _client;

    public ConnectionController(ILogger<ConnectionController> logger,IKubernetesClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<ResourceControllerResult?> ReconcileAsync(ConnectionEntity entity)
    {
        if (!(await ValidateConnectionEntity(entity)))
        {
            _logger.LogWarning("Connection {connectionName} is not valid",entity.Metadata.Name);
            return null;
        }

        var depl = new V1Deployment();
        depl.Metadata.Name = $"proxy-{entity.Metadata.Name}";


        return null;
    }

    public Task StatusModifiedAsync(ConnectionEntity entity) => Task.CompletedTask;

    public Task DeletedAsync(ConnectionEntity entity) => Task.CompletedTask;


    private async Task<bool> ValidateConnectionEntity(ConnectionEntity entity)
    {
        var device = await _client.Get<DeviceEntity>(entity.Spec.DeviceName);
        if (device is null)
        {
            _logger.LogWarning("Device {deviceName} not found",entity.Spec.DeviceName);
            return false;
        }
        var applications = await _client.List<ApplicationEntity>();
        var requiredAppliacations = applications.Where(a => entity.Spec.ApplicationNames.Contains(a.Metadata.Name));
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