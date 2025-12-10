using CDS_BLL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CDS_MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var config = LoadConfiguration();
            builder.Configuration.AddConfiguration(config);

            var connectionString = builder.Configuration.GetConnectionString("CDS_SQL_DB")
                ?? throw new InvalidOperationException("Строка подключения не найдена!");
            builder.Services.AddDBFromBLL(connectionString);

            return builder.Build();
        }

        private static IConfiguration LoadConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceNames = assembly.GetManifestResourceNames();
            var resourceName = resourceNames.FirstOrDefault(r => r.Contains("appsettings.json"));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new FileNotFoundException("appsettings.json не найден в ресурсах");
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Не удалось открыть поток для {resourceName}");
            }

            return new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
        }
    }
}
