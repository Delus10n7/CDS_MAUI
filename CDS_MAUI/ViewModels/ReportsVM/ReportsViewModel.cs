using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.ViewModels.ReportsVM
{
    public partial class ReportsViewModel : BaseViewModel
    {
        // === ОТЧЕТ ЗА ПЕРИОД ===
        [ObservableProperty]
        private DateTime _periodFromDate;

        [ObservableProperty]
        private DateTime _periodToDate;

        [ObservableProperty]
        private bool _isPeriodReportVisible = false;

        [ObservableProperty]
        private string _periodReportButtonText = "";

        [ObservableProperty]
        private string _periodOrdersCount = "";

        [ObservableProperty]
        private string _periodOrdersSum = "";

        // === СЕРВИСЫ ===
        IOrderService _orderService;

        public ReportsViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            Title = "Отчеты";

            IsPeriodReportVisible = false;
            PeriodReportButtonText = "Показать отчет";
            PeriodFromDate = DateTime.Now;
            PeriodToDate = DateTime.Now;
        }

        [RelayCommand]
        private void ExecutePeriodReport()
        {
            IsPeriodReportVisible = !IsPeriodReportVisible;
            if (PeriodReportButtonText == "Показать отчет") PeriodReportButtonText = "Скрыть отчет";
            else PeriodReportButtonText = "Показать отчет";

            if (string.IsNullOrEmpty(PeriodOrdersCount) && string.IsNullOrEmpty(PeriodOrdersSum))
            {
                List<OrderDTO> orderDTOs = _orderService.GetAllOrders()
                .Where(o => o.OrderDate >= DateOnly.FromDateTime(PeriodFromDate) &&
                            o.OrderDate <= DateOnly.FromDateTime(PeriodToDate))
                .ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in orderDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.SalePrice;
                }

                PeriodOrdersCount = string.Empty;
                PeriodOrdersSum = string.Empty;

                PeriodOrdersCount += "Количество заказов за выбранный период: ";
                PeriodOrdersSum += "Выручка за выбранный период: ";

                PeriodOrdersCount += ordersCount.ToString();
                PeriodOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                PeriodOrdersCount = string.Empty;
                PeriodOrdersSum = string.Empty;
            }
        }
    }
}
