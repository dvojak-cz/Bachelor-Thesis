using KubeOps.Operator;

namespace cz.dvojak.k8s.EdgeOperator.Configuration;

public static class OperatorAppConfiguration
{
    public static void ConfigureEdgeOperatorApp(this WebApplication app)
    {
        app.UseKubernetesOperator();
    }
}