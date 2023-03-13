namespace cz.dvojak.k8s.EdgeOperator.Configuration.Options;

public class ProxyTemplateOption
{
    public const string ProxyTemplate = "ProxyTemplate";
    public string ImageName { get; set; } = "alpine/socat";
    public string[] Args { get; set; } = { "-dd","-lh" };
    public int BasePort { get; set; } = 8000;
}