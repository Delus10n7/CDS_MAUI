using CDS_DomainModel.Entities;
using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CDS_MAUI.Models;
using CDS_MAUI.Views.ServiceContractsModal;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;

namespace CDS_MAUI.ViewModels.ServiceContractsVM
{
    public partial class ServiceContractsViewModel : BaseViewModel
    {
        // === КОЛЛЕКЦИИ ДАННЫХ ===
        [ObservableProperty]
        private ObservableCollection<ServiceContractModel> _serviceContracts = new();

        [ObservableProperty]
        private ObservableCollection<ServiceContractModel> _filteredServiceContracts = new();

        [ObservableProperty]
        private ObservableCollection<string> _additionalServices = new();

        [ObservableProperty]
        private ObservableCollection<string> _managers = new();

        // === ФИЛЬТРЫ ===
        [ObservableProperty]
        private string _selectedAdditionalService = "Любая";

        [ObservableProperty]
        private string _selectedManager = "Любой";

        [ObservableProperty]
        private string _clientName = "";

        [ObservableProperty]
        private string _priceFrom = "";

        [ObservableProperty]
        private string _priceTo = "";

        // === ПОИСК И СОСТОЯНИЕ ===
        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private bool _isFilterPanelVisible = false;

        [ObservableProperty]
        private ServiceContractModel _selectedServiceContract;

        // === СТРАНИЦЫ ===
        private List<ServiceContractDTO> _allServiceContracts = new();
        private const int _pageSize = 20;
        private int _pageCount = 1;
        private int _currentPage = 1;

        [ObservableProperty]
        private string _curPage = "1";

        [ObservableProperty]
        private bool _canGoNextPage = true;

        [ObservableProperty]
        private bool _canGoPrevPage = false;

        [ObservableProperty]
        private bool _hasFooterPageButtons = false;

        // === СЕРВИСЫ ===
        IServiceContractsService _serviceContractsService;
        IUserService _userService;

        public ServiceContractsViewModel(IServiceContractsService serviceContractsService, IUserService userService)
        {
            _serviceContractsService = serviceContractsService;
            _userService = userService;

            Title = "Дополнительные услуги";
            Initialize();
        }

        private void Initialize()
        {
            InitializeAdditionalServices();
            InitializeManagers();

            LoadAllServiceContracts();
            LoadCurrentPageServiceContracts();
        }

        private void InitializeAdditionalServices()
        {
            AdditionalServices.Clear();
            AdditionalServices.Add("Любая");

            List<AdditionalServiceDTO> additionalServiceDTOs = _serviceContractsService.GetAllAdditionalServices();

            foreach(var a in additionalServiceDTOs)
            {
                AdditionalServices.Add(a.ServiceName);
            }
        }

        private void InitializeManagers()
        {
            Managers.Clear();
            Managers.Add("Любой");

            List<ManagerDTO> managerDTOs = _userService.GetAllManagers();

            foreach(var m in managerDTOs)
            {
                Managers.Add(m.FullName);
            }
        }

        // === КОМАНДЫ ===

        [RelayCommand]
        private void Refresh()
        {
            LoadAllServiceContracts();
            ApplyFilters();
        }

        [RelayCommand]
        private void ToggleFilters()
        {
            IsFilterPanelVisible = !IsFilterPanelVisible;
        }

        [RelayCommand]
        private void ResetFilters()
        {
            SelectedAdditionalService = "Любая";
            SelectedManager = "Любой";
            ClientName = "";
            PriceFrom = "";
            PriceTo = "";
            SearchText = "";

            FilterServiceContracts();
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            FilterServiceContracts();
            LoadCurrentPageServiceContracts();
            IsFilterPanelVisible = false;
        }

        [RelayCommand]
        private void Search()
        {
            ApplyFilters();
        }

        [RelayCommand]
        private void NextPage()
        {
            if (_currentPage < _pageCount) _currentPage++;

            CurPage = _currentPage.ToString();

            if (_currentPage > 1) CanGoPrevPage = true;
            else CanGoPrevPage = false;
            if (_currentPage < _pageCount) CanGoNextPage = true;
            else CanGoNextPage = false;

            LoadCurrentPageServiceContracts();
        }

        [RelayCommand]
        private void PrevPage()
        {
            if (_currentPage > 1) _currentPage--;

            CurPage = _currentPage.ToString();

            if (_currentPage > 1) CanGoPrevPage = true;
            else CanGoPrevPage = false;
            if (_currentPage < _pageCount) CanGoNextPage = true;
            else CanGoNextPage = false;

            LoadCurrentPageServiceContracts();
        }

        [RelayCommand]
        private void FirstPage()
        {
            _currentPage = 1;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;

            LoadCurrentPageServiceContracts();
        }

        [RelayCommand]
        private void LastPage()
        {
            _currentPage = _pageCount;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = _currentPage > 1 ? true : false;
            CanGoNextPage = false;

            LoadCurrentPageServiceContracts();
        }

        [RelayCommand]
        private async Task ShowServiceContractsOrderModal()
        {
            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync(nameof(ServiceContractsOrderModal), true);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ChangeContractStatus(ServiceContractModel contract)
        {
            if (contract == null) return;

            string action = await Shell.Current.DisplayActionSheet(
                "Изменить статус заказа",
                "Закрыть",
                null,
                "Выполнен",
                "В ожидании",
                "Отменен");

            if (string.IsNullOrEmpty(action) || action == "Закрыть")
            {
                return;
            }

            if (action == "Отменен")
            {
                bool confirm = await Shell.Current.DisplayAlert(
                "Подтверждение отмены",
                "Вы уверены, что хотите отменить заказ?",
                "Да, отменить",
                "Нет");

                if (!confirm)
                    return;
            }

            contract.Status = action;

            ServiceContractDTO contractDTO = _serviceContractsService.GetServiceContract(contract.Id);
            contractDTO.ContractStatus = contract.Status;
            _serviceContractsService.UpdateServiceContract(contractDTO);

            Refresh();
        }

        // === ЗАГРУЗКА ДАННЫХ ===

        private void LoadAllServiceContracts()
        {
            _allServiceContracts.Clear();

            List<ServiceContractDTO> serviceContractDTOs = _serviceContractsService.GetAllServiceContracts().Where(i => i.ContractStatus != "Отменен")
                .OrderByDescending(i => i.SaleDate).OrderByDescending(i => i.Id).ToList();

            foreach (var contract in serviceContractDTOs)
            {
                _allServiceContracts.Add(contract);
            }

            FilterServiceContracts();
        }

        private void FilterServiceContracts()
        {
            var filtered = _allServiceContracts.ToList();

            if (SelectedAdditionalService != "Любая" && !string.IsNullOrEmpty(SelectedAdditionalService))
            {
                var ids = new List<int>();

                foreach (var sc in filtered)
                {
                    foreach(var ss in sc.SelectedServices)
                    {
                        if (ss.AdditionalServiceName == SelectedAdditionalService)
                        {
                            ids.Add(sc.Id);
                        }
                    }
                }

                filtered = filtered.Where(c => ids.Contains(c.Id)).ToList();
            }

            if (SelectedManager != "Любой" && !string.IsNullOrEmpty(SelectedManager))
                filtered = filtered.Where(o => o.ManagerName == SelectedManager).ToList();

            if (!string.IsNullOrWhiteSpace(ClientName))
                filtered = filtered.Where(o =>
                    o.CustomerName.Contains(ClientName, StringComparison.OrdinalIgnoreCase)).ToList();

            // Фильтрация по сумме заказа
            if (!string.IsNullOrEmpty(PriceFrom))
                filtered = filtered.Where(o => o.TotalPrice >= Decimal.Parse(PriceFrom)).ToList();

            if (!string.IsNullOrEmpty(PriceTo))
                filtered = filtered.Where(o => o.TotalPrice <= Decimal.Parse(PriceTo)).ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(o =>
                    o.CustomerName.ToLower().Contains(searchLower) ||
                    o.ManagerName.ToLower().Contains(searchLower)).ToList();
            }

            FilteredServiceContracts.Clear();
            foreach (var contract in filtered)
            {
                FilteredServiceContracts.Add(new ServiceContractModel(contract));
            }

            _pageCount = (int)Math.Ceiling((decimal)FilteredServiceContracts.Count / _pageSize);
            _currentPage = 1;
            CurPage = "1";
            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;
        }

        private void LoadCurrentPageServiceContracts()
        {
            var currIndex = _currentPage - 1;

            var startIndex = currIndex * 20;
            var endIndex = (currIndex * 20 + 19) >= FilteredServiceContracts.Count() ? FilteredServiceContracts.Count() - 1 : (currIndex * 20 + 19);

            ServiceContracts.Clear();
            for (int i = startIndex; i <= endIndex; i++)
            {
                ServiceContracts.Add(FilteredServiceContracts[i]);
            }

            if (ServiceContracts.Count > 5) HasFooterPageButtons = true;
            else HasFooterPageButtons = false;
        }
    }
}
