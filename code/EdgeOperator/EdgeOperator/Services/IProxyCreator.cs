using cz.dvojak.k8s.EdgeOperator.Models.Entities;
using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Services;

public interface IProxyCreator
{
    /// <summary>
    /// Returns the name of the proxy deployment
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    string GetProxyName(ConnectionEntity entity);
    /// <summary>
    /// Creates a proxy deployment for the given connection
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="networkName"></param>
    /// <returns></returns>
    Task<V1Deployment> CreateProxyDeployment(ConnectionEntity entity,string networkName);
}