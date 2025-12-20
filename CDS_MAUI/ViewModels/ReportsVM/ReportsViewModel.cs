using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // === ОТЧЕТ ПРОДАЖ МЕНЕДЖЕРА ===

        [ObservableProperty]
        private ObservableCollection<string> _managers = new ObservableCollection<string>();

        [ObservableProperty]
        private string _selectedManager = "";

        [ObservableProperty]
        private bool _isManagerReportVisible = false;

        [ObservableProperty]
        private string _managerReportButtonText = "";

        [ObservableProperty]
        private string _managerOrdersCount = "";

        [ObservableProperty]
        private string _managerOrdersSum = "";

        // === СЕРВИСЫ ===

        IOrderService _orderService;
        IUserService _userService;

        public ReportsViewModel(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;

            Title = "Отчеты";

            // Отчет за период
            IsPeriodReportVisible = false;
            PeriodReportButtonText = "Показать отчет";
            PeriodFromDate = DateTime.Now;
            PeriodToDate = DateTime.Now;

            // Отчет продаж менеджера
            IsManagerReportVisible = false;
            ManagerReportButtonText = "Показать отчет";
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            Managers.Clear();
            Managers.Add("Не выбран");
            SelectedManager = Managers[0];

            List<ManagerDTO> managerDTOs = _userService.GetAllManagers();

            foreach(var m in managerDTOs)
            {
                Managers.Add(m.FullName);
            }
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
                    .Where(o => o.StatusId != 3 &&
                                o.OrderDate >= DateOnly.FromDateTime(PeriodFromDate) &&
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

        [RelayCommand]
        private void ExecuteManagerReport()
        {
            IsManagerReportVisible = !IsManagerReportVisible;
            if (ManagerReportButtonText == "Показать отчет") ManagerReportButtonText = "Скрыть отчет";
            else ManagerReportButtonText = "Показать отчет";

            if (SelectedManager != "Не выбран" && !string.IsNullOrEmpty(ManagerReportButtonText))
            {
                ManagerDTO m = _userService.GetAllManagers().FirstOrDefault(m => m.FullName == SelectedManager);

                List<OrderDTO> orderDTOs = _orderService.GetAllOrders()
                    .Where(o => o.StatusId != 3 &&
                                o.ManagerId == m.Id)
                    .ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in orderDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.SalePrice;
                }

                ManagerOrdersCount = string.Empty;
                ManagerOrdersSum = string.Empty;

                ManagerOrdersCount += "Количество заказов за выбранный период: ";
                ManagerOrdersSum += "Выручка за выбранный период: ";

                ManagerOrdersCount += ordersCount.ToString();
                ManagerOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                ManagerOrdersCount = string.Empty;
                ManagerOrdersSum = string.Empty;

                ManagerOrdersCount += "Количество заказов за выбранный период: 0";
                ManagerOrdersSum += "Выручка за выбранный период: 0";
            }
        }
    }
}
