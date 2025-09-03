using Microsoft.Extensions.Configuration;
using System.IO;

namespace RealEstate.Tests.TestUtils
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot LoadApiConfiguration()
        {
            // Ajusta la ruta si tu estructura es distinta:
            // asumiendo: /RealEstateSolution/RealEstate.Api/appsettings.json
            var apiProjectDir = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), 
                "../../../../RealEstate.Api"
            ));

            var config = new ConfigurationBuilder()
                .SetBasePath(apiProjectDir)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            return config;
        }

        public static string GetConnectionString()
        {
            var cfg = LoadApiConfiguration();
            return cfg.GetConnectionString("DefaultConnection")!;
        }

        public static IConfigurationRoot ApiConfig => LoadApiConfiguration();
    }
}
