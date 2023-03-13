using cz.dvojak.k8s.EdgeOperator.Models.Entities;
using k8s.Models;

namespace cz.dvojak.k8s.EdgeOperator.Services.Builders;

/// <summary>
/// Builder for proxy containers
/// </summary>
public interface IProxyContainerBuilder
{
    /// <summary>
    /// Reset builder to default state
    /// </summary>
    /// <returns></returns>
    IProxyContainerBuilder Reset();
    /// <summary>
    /// Build containers and return them
    /// </summary>
    /// <returns>Created containers</returns>
    IList<V1Container> Build();
    /// <summary>
    /// Add container for proxy
    /// </summary>
    /// <param name="ip">IP of service</param>
    /// <param name="service">Service to proxy</param>
    /// <returns></returns>
    IProxyContainerBuilder Add(string ip,params ApplicationEntity.ApplicationSpec.Service[] service);
}