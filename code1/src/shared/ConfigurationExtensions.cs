// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetApiPathBase(this IConfiguration configuration)
        {
            return configuration["API_PATH_BASE"];
        }
    }
}