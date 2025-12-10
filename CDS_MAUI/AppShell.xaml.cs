using CDS_MAUI.Views.CarDetailsModal;
using CDS_MAUI.Views.OrderDetailsModal;

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
        }
    }
}
