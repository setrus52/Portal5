namespace IDM.Infrastructure.Options.EndpointOptions;

public class EndpointOptions
{
    public List<EndpointGroup> Groups { get; set; } = [];
}

public class EndpointGroup
{
    public string Code { get; set; } = string.Empty;
    public string Root { get; set; } = string.Empty;
    public List<Endpoint> Points { get; set; } = [];
}

public class Endpoint
{
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Point { get; set; } = string.Empty;
}