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

        // === ОТЧЕТ ПРОДАЖ МАРКИ ===

        [ObservableProperty]
        private ObservableCollection<string> _brands = new ObservableCollection<string>();

        [ObservableProperty]
        private string _selectedBrand = "";

        [ObservableProperty]
        private bool _isBrandReportVisible = false;

        [ObservableProperty]
        private string _brandReportButtonText = "";

        [ObservableProperty]
        private string _brandOrdersCount = "";

        [ObservableProperty]
        private string _brandOrdersSum = "";



        // === ОТЧЕТ НА ДОП УСЛУГИ ЗА ПЕРИОД ===

        [ObservableProperty]
        private DateTime _additionalServicePeriodFromDate;

        [ObservableProperty]
        private DateTime _additionalServicePeriodToDate;

        [ObservableProperty]
        private bool _isAdditionalServicePeriodReportVisible = false;

        [ObservableProperty]
        private string _additionalServicePeriodReportButtonText = "";

        [ObservableProperty]
        private string _additionalServicePeriodOrdersCount = "";

        [ObservableProperty]
        private string _additionalServicePeriodOrdersSum = "";

        // === ОТЧЕТ ПРОДАЖ ДОП УСЛУГ МЕНЕДЖЕРА ===

        [ObservableProperty]
        private string _selectedAdditionalServiceManager = "";

        [ObservableProperty]
        private bool _isAdditionalServiceManagerReportVisible = false;

        [ObservableProperty]
        private string _additionalServiceManagerReportButtonText = "";

        [ObservableProperty]
        private string _additionalServiceManagerOrdersCount = "";

        [ObservableProperty]
        private string _additionalServiceManagerOrdersSum = "";

        // === ОТЧЕТ ПРОДАЖ ДОП УСЛУГИ ===

        [ObservableProperty]
        private ObservableCollection<string> _additionalServices = new ObservableCollection<string>();

        [ObservableProperty]
        private string _selectedAdditionalService = "";

        [ObservableProperty]
        private bool _isAdditionalServiceReportVisible = false;

        [ObservableProperty]
        private string _additionalServiceReportButtonText = "";

        [ObservableProperty]
        private string _additionalServiceOrdersCount = "";

        [ObservableProperty]
        private string _additionalServiceOrdersSum = "";



        // === СЕРВИСЫ ===

        IOrderService _orderService;
        IUserService _userService;
        ICarService _carService;
        ICarConfigurationService _carConfigurationService;
        IServiceContractsService _serviceContractsService;

        public ReportsViewModel(IOrderService orderService, IUserService userService, ICarService carService, ICarConfigurationService carConfigurationService, IServiceContractsService serviceContractsService)
        {
            _orderService = orderService;
            _userService = userService;
            _carService = carService;
            _carConfigurationService = carConfigurationService;
            _serviceContractsService = serviceContractsService;

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

            // Отчет продаж марки
            IsBrandReportVisible = false;
            BrandReportButtonText = "Показать отчет";
            InitializeBrands();

            // Отчет доп услуг за период
            IsAdditionalServicePeriodReportVisible = false;
            AdditionalServicePeriodReportButtonText = "Показать отчет";
            AdditionalServicePeriodFromDate = DateTime.Now;
            AdditionalServicePeriodToDate = DateTime.Now;

            // Отчет продаж доп услуг менеджера
            IsAdditionalServiceManagerReportVisible = false;
            AdditionalServiceManagerReportButtonText = "Показать отчет";

            // Отчет продаж доп услуги
            IsAdditionalServiceReportVisible = false;
            AdditionalServiceReportButtonText = "Показать отчет";
            InitializeAdditionalServices();
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

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Не выбрана");
            SelectedBrand = Brands[0];

            List<BrandDTO> brandDTOs = _carConfigurationService.GetAllBrands();

            foreach(var b in brandDTOs)
            {
                Brands.Add(b.BrandName);
            }
        }

        private void InitializeAdditionalServices()
        {
            AdditionalServices.Clear();
            AdditionalServices.Add("Не выбрана");
            SelectedAdditionalService = AdditionalServices[0];

            List<AdditionalServiceDTO> additionalServiceDTOs = _serviceContractsService.GetAllAdditionalServices();

            foreach(var a in additionalServiceDTOs)
            {
                AdditionalServices.Add(a.ServiceName);
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
                    .Where(o => o.StatusId != 4 &&
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

            if (SelectedManager != "Не выбран" && !string.IsNullOrEmpty(SelectedManager))
            {
                ManagerDTO m = _userService.GetAllManagers().FirstOrDefault(m => m.FullName == SelectedManager);

                List<OrderDTO> orderDTOs = _orderService.GetAllOrders()
                    .Where(o => o.StatusId != 4 &&
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

                ManagerOrdersCount += "Количество заказов выбранного менеджера: ";
                ManagerOrdersSum += "Выручка выбранного менеджера: ";

                ManagerOrdersCount += ordersCount.ToString();
                ManagerOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                ManagerOrdersCount = string.Empty;
                ManagerOrdersSum = string.Empty;

                ManagerOrdersCount += "Количество заказов выбранного менеджера: 0";
                ManagerOrdersSum += "Выручка выбранного менеджера: 0";
            }
        }

        [RelayCommand]
        private void ExecuteBrandReport()
        {
            IsBrandReportVisible = !IsBrandReportVisible;
            if (BrandReportButtonText == "Показать отчет") BrandReportButtonText = "Скрыть отчет";
            else BrandReportButtonText = "Показать отчет";

            if (SelectedBrand != "Не выбрана" && !string.IsNullOrEmpty(SelectedBrand))
            {
                BrandDTO b = _carConfigurationService.GetAllBrands().FirstOrDefault(b => b.BrandName == SelectedBrand);

                List<int> carIds = _carService.GetAllCars().Where(c => c.BrandName == b.BrandName).Select(c => c.Id).ToList();

                List<OrderDTO> orderDTOs = _orderService.GetAllOrders()
                    .Where(o => o.StatusId != 4 &&
                                carIds.Contains((int)o.CarId))
                    .ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in orderDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.SalePrice;
                }

                BrandOrdersCount = string.Empty;
                BrandOrdersSum = string.Empty;

                BrandOrdersCount += "Количество заказов на выбранную марку: ";
                BrandOrdersSum += "Выручка за выбранную марку: ";

                BrandOrdersCount += ordersCount.ToString();
                BrandOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                BrandOrdersCount = string.Empty;
                BrandOrdersSum = string.Empty;

                BrandOrdersCount += "Количество заказов на выбранную марку: 0";
                BrandOrdersSum += "Выручка за выбранную марку: 0";
            }
        }

        [RelayCommand]
        private void ExecuteAdditionalServicePeriodReport()
        {
            IsAdditionalServicePeriodReportVisible = !IsAdditionalServicePeriodReportVisible;
            if (AdditionalServicePeriodReportButtonText == "Показать отчет") AdditionalServicePeriodReportButtonText = "Скрыть отчет";
            else AdditionalServicePeriodReportButtonText = "Показать отчет";

            if (string.IsNullOrEmpty(AdditionalServicePeriodOrdersCount) && string.IsNullOrEmpty(AdditionalServicePeriodOrdersSum))
            {
                List<ServiceContractDTO> serviceContractDTOs = _serviceContractsService.GetAllServiceContracts()
                    .Where(s => s.SaleDate >= DateOnly.FromDateTime(AdditionalServicePeriodFromDate) &&
                                s.SaleDate <= DateOnly.FromDateTime(AdditionalServicePeriodToDate))
                    .ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in serviceContractDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.TotalPrice;
                }

                AdditionalServicePeriodOrdersCount = string.Empty;
                AdditionalServicePeriodOrdersSum = string.Empty;

                AdditionalServicePeriodOrdersCount += "Количество заказов за выбранный период: ";
                AdditionalServicePeriodOrdersSum += "Выручка за выбранный период: ";

                AdditionalServicePeriodOrdersCount += ordersCount.ToString();
                AdditionalServicePeriodOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                AdditionalServicePeriodOrdersCount = string.Empty;
                AdditionalServicePeriodOrdersSum = string.Empty;
            }
        }

        [RelayCommand]
        private void ExecuteAdditionalServiceManagerReport()
        {
            IsAdditionalServiceManagerReportVisible = !IsAdditionalServiceManagerReportVisible;
            if (AdditionalServiceManagerReportButtonText == "Показать отчет") AdditionalServiceManagerReportButtonText = "Скрыть отчет";
            else AdditionalServiceManagerReportButtonText = "Показать отчет";

            if (SelectedAdditionalServiceManager != "Не выбран" && !string.IsNullOrEmpty(SelectedAdditionalServiceManager))
            {
                ManagerDTO m = _userService.GetAllManagers().FirstOrDefault(m => m.FullName == SelectedAdditionalServiceManager);

                List<ServiceContractDTO> serviceContractDTOs = _serviceContractsService.GetAllServiceContracts()
                    .Where(s => s.ManagerId == m.Id)
                    .ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in serviceContractDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.TotalPrice;
                }

                AdditionalServiceManagerOrdersCount = string.Empty;
                AdditionalServiceManagerOrdersSum = string.Empty;

                AdditionalServiceManagerOrdersCount += "Количество заказов выбранного менеджера: ";
                AdditionalServiceManagerOrdersSum += "Выручка выбранного менеджера: ";

                AdditionalServiceManagerOrdersCount += ordersCount.ToString();
                AdditionalServiceManagerOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                AdditionalServiceManagerOrdersCount = string.Empty;
                AdditionalServiceManagerOrdersSum = string.Empty;

                AdditionalServiceManagerOrdersCount += "Количество заказов выбранного менеджера: 0";
                AdditionalServiceManagerOrdersSum += "Выручка выбранного менеджера: 0";
            }
        }

        [RelayCommand]
        private void ExecuteAdditionalServiceReport()
        {
            IsAdditionalServiceReportVisible = !IsAdditionalServiceReportVisible;
            if (AdditionalServiceReportButtonText == "Показать отчет") AdditionalServiceReportButtonText = "Скрыть отчет";
            else AdditionalServiceReportButtonText = "Показать отчет";

            if (SelectedAdditionalService != "Не выбрана" && !string.IsNullOrEmpty(SelectedAdditionalService))
            {
                AdditionalServiceDTO a = _serviceContractsService.GetAllAdditionalServices().FirstOrDefault(a => a.ServiceName == SelectedAdditionalService);

                List<ServiceContractDTO> serviceContractDTOs = _serviceContractsService.GetAllServiceContracts();
                
                List<int> contractIds = new List<int>();
                foreach(var contract in serviceContractDTOs)
                {
                    foreach(var service in contract.SelectedServices)
                    {
                        if (service.AdditionalServiceName == SelectedAdditionalService)
                        {
                            contractIds.Add(contract.Id);
                        }
                    }
                }

                serviceContractDTOs = serviceContractDTOs.Where(s => contractIds.Contains(s.Id)).ToList();

                int ordersCount = 0;
                decimal ordersSum = 0;

                foreach (var order in serviceContractDTOs)
                {
                    ordersCount++;
                    ordersSum += (decimal)order.TotalPrice;
                }

                AdditionalServiceOrdersCount = string.Empty;
                AdditionalServiceOrdersSum = string.Empty;

                AdditionalServiceOrdersCount += "Количество заказов на выбранную услугу: ";
                AdditionalServiceOrdersSum += "Выручка за выбранную услугу: ";

                AdditionalServiceOrdersCount += ordersCount.ToString();
                AdditionalServiceOrdersSum += ordersSum.ToString("N0");
            }
            else
            {
                AdditionalServiceOrdersCount = string.Empty;
                AdditionalServiceOrdersSum = string.Empty;

                AdditionalServiceOrdersCount += "Количество заказов на выбранную марку: 0";
                AdditionalServiceOrdersSum += "Выручка за выбранную марку: 0";
            }
        }
    }
}
