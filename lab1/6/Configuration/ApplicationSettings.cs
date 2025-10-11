namespace WebApplicationConfig.Configuration;

public class ApplicationSettings
{
    public string AppName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public bool DebugMode { get; set; }
    public int MaxItemsPerPage { get; set; } = 10;
}

public class FeatureFlags
{
    public bool EnableSwagger { get; set; }
    public bool EnableCaching { get; set; }
    public bool EnableDetailedErrors { get; set; }
}