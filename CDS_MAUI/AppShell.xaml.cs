using CDS_MAUI.Views.CarsModal;
using CDS_MAUI.Views.OrdersModal;

namespace CDS_MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Регистрация маршрутов для модальных окон
            Routing.RegisterRoute(nameof(CarDetailsModal), typeof(CarDetailsModal));
            Routing.RegisterRoute(nameof(OrderDetailsModal), typeof(OrderDetailsModal));
            Routing.RegisterRoute(nameof(CarOrderModal), typeof(CarOrderModal));
        }
    }
}
