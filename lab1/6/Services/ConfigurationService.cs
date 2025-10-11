using Microsoft.Extensions.Options;
using WebApplicationConfig.Configuration;

namespace WebApplicationConfig.Services;

public interface IConfigurationService
{
    string GetConfigurationInfo();
    ApplicationSettings GetApplicationSettings();
    FeatureFlags GetFeatureFlags();
}

public class ConfigurationService : IConfigurationService
{
    private readonly ApplicationSettings _appSettings;
    private readonly FeatureFlags _featureFlags;
    private readonly IConfiguration _configuration;

    public ConfigurationService(
        IOptions<ApplicationSettings> appSettings,
        IOptions<FeatureFlags> featureFlags,
        IConfiguration configuration)
    {
        _appSettings = appSettings.Value;
        _featureFlags = featureFlags.Value;
        _configuration = configuration;
    }

    public string GetConfigurationInfo()
    {
        return $"""
            Application: {_appSettings.AppName}
            Version: {_appSettings.Version}
            Environment: {_appSettings.Environment}
            API URL: {_appSettings.ApiUrl}
            Debug Mode: {_appSettings.DebugMode}
            Max Items Per Page: {_appSettings.MaxItemsPerPage}
            Features:
              - Swagger: {_featureFlags.EnableSwagger}
              - Caching: {_featureFlags.EnableCaching}
              - Detailed Errors: {_featureFlags.EnableDetailedErrors}
            Connection String: {_configuration.GetConnectionString("DefaultConnection")}
            """;
    }

    public ApplicationSettings GetApplicationSettings()
    {
        return _appSettings;
    }

    public FeatureFlags GetFeatureFlags()
    {
        return _featureFlags;
    }
}