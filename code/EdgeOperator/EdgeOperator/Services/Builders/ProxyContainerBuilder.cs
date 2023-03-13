using System.Collections.ObjectModel;
using cz.dvojak.k8s.EdgeOperator.Configuration.Options;
using cz.dvojak.k8s.EdgeOperator.Models;
using cz.dvojak.k8s.EdgeOperator.Models.Entities;
using k8s.Models;
using Microsoft.Extensions.Options;
using static cz.dvojak.k8s.EdgeOperator.Models.Entities.ApplicationEntity.ApplicationSpec;

namespace cz.dvojak.k8s.EdgeOperator.Services.Builders;

public class ProxyContainerBuilder : IProxyContainerBuilder
{
    readonly private ProxyTemplateOption _proxyTemplateOption;
    private IList<V1Container> _containers;
    public IList<V1Container> Build() => _containers;
    private int _port;

    public ProxyContainerBuilder(IOptions<ProxyTemplateOption> proxyTemplateOption)
    {
        _proxyTemplateOption = proxyTemplateOption.Value;
        _port = _proxyTemplateOption.BasePort;
        _containers = new List<V1Container>();
    }

    private static string[] CreateConnectionArgs(string ip,(Service service,int port) service)
    {
        return service.service.Protocol == Protocol.Tcp || service.service.Protocol == Protocol.Http
            ? new[] { $"tcp4-listen:{service.port},fork",$"tcp4-connect:{ip}:{service.service.Port}" }
            : new[] { "-u",$"udp-listen:{service.port},fork",$"udp4-connect::{ip}:{service.service.Port}" };
    }

    private V1Container CreateProxyContainer(string ip,(Service service,int port) service)
    {
        return new V1Container()
        {
            Name = $"{service.service.Name}-{service.service.GetHashCode()}",
            Image = _proxyTemplateOption.ImageName,
            Args = _proxyTemplateOption.Args.Concat(new[] { "-lp",service.service.Name, })
                .Concat(CreateConnectionArgs(ip,service)).ToArray(), //todo
            Ports = new Collection<V1ContainerPort>()
            {
                new V1ContainerPort()
                {
                    Name = $"{service.service.Name}-{service.service.GetHashCode()}",
                    Protocol = service.service.Protocol.ToString(), //todo bude tady spravny srting?,
                    ContainerPort = service.port
                }
            }
        };
    }

    public IProxyContainerBuilder Add(string ip,params ApplicationEntity.ApplicationSpec.Service[] service)
    {
        foreach (var variable in service)
        {
            _containers.Add(CreateProxyContainer(ip,(variable,_port)));
            _port++;
        }

        return this;
    }

    public IProxyContainerBuilder Reset()
    {
        _containers = new List<V1Container>();
        return this;
    }

}