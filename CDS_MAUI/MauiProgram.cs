using CDS_BLL;
using CDS_BLL.Service;
using CDS_Interfaces.Service;
using CDS_MAUI.ViewModels;
using CDS_MAUI.ViewModels.CarsVM;
using CDS_MAUI.ViewModels.OrdersVM;
using CDS_MAUI.ViewModels.ReportsVM;
using CDS_MAUI.ViewModels.ServiceContractsVM;
using CDS_MAUI.Views;
using CDS_MAUI.Views.CarsModal;
using CDS_MAUI.Views.OrdersModal;
using CDS_MAUI.Views.ServiceContractsModal;
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

            // Репозиторий БД
            builder.Services.AddDBFromBLL(connectionString);

            // Сервисы
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICarConfigurationService, CarConfigurationService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();
            builder.Services.AddScoped<IModelService, ModelService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IServiceContractsService, ServiceContractsService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDatabaseService, DatabaseService>();

            // Регистрация ViewModels
            builder.Services.AddTransient<CarsViewModel>();
            builder.Services.AddTransient<OrdersViewModel>();
            builder.Services.AddTransient<ReportsViewModel>();
            builder.Services.AddTransient<CarDetailsViewModel>();
            builder.Services.AddTransient<OrderDetailsViewModel>();
            builder.Services.AddTransient<CarOrderViewModel>();
            builder.Services.AddTransient<ServiceContractsViewModel>();
            builder.Services.AddTransient<ServiceContractsOrderViewModel>();

            // Регистрация Views
            builder.Services.AddTransient<CarsPage>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<ReportsPage>();
            builder.Services.AddTransient<CarDetailsModal>();
            builder.Services.AddTransient<OrderDetailsModal>();
            builder.Services.AddTransient<CarOrderModal>();
            builder.Services.AddTransient<ServiceContractsPage>();
            builder.Services.AddTransient<ServiceContractsOrderModal>();

            var app = builder.Build();

            InitializeDatabase(app);

            return app;
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

        private static void InitializeDatabase(MauiApp app)
        {
            // Используем Task.Run, чтобы не блокировать UI поток
            Task.Run(async () =>
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                    await databaseService.InitializeDatabaseAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка инициализации БД: {ex.Message}");
                }
            }).Wait();
        }
    }
}
