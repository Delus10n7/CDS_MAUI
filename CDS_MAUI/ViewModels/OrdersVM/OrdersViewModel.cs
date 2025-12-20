using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CDS_MAUI.Models;
using CDS_MAUI.Views.OrdersModal;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CDS_MAUI.Views.OrdersPage;

namespace CDS_MAUI.ViewModels.OrdersVM
{
    public partial class OrdersViewModel : BaseViewModel
    {
        // === КОЛЛЕКЦИИ ДАННЫХ ===
        [ObservableProperty]
        private ObservableCollection<OrderModel> _orders = new();

        [ObservableProperty]
        private ObservableCollection<OrderModel> _filteredOrders = new();

        [ObservableProperty]
        private ObservableCollection<string> _brands = new();

        [ObservableProperty]
        private ObservableCollection<string> _models = new();

        [ObservableProperty]
        private ObservableCollection<string> _managers = new();

        // === ФИЛЬТРЫ ===
        [ObservableProperty]
        private string _selectedBrand = "Любой";

        [ObservableProperty]
        private string _selectedModel = "Любая";

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
        private OrderModel _selectedOrder;

        // === СТРАНИЦЫ ===
        private List<OrderModel> _allOrders = new();
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
        private IOrderService _orderService;
        private ICarService _carService;
        private ICarConfigurationService _carConfigService;
        private IUserService _userService;

        public OrdersViewModel(IOrderService orderService, ICarService carService, ICarConfigurationService carConfigurationService, IUserService userService)
        {
            _orderService = orderService;
            _carService = carService;
            _carConfigService = carConfigurationService;
            _userService = userService;

            Title = "Заказы";
            Initialize();
        }

        private void Initialize()
        {
            // Инициализация списков
            InitializeBrands();
            InitializeManagers();
            InitializeFilterOptions();

            // Загрузка заказов
            LoadAllOrders();
            LoadCurrentPageOrders();
        }

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Любой");

            List<BrandDTO> brandDTOs = _carConfigService.GetAllBrands();

            foreach (var b in brandDTOs)
            {
                Brands.Add(b.BrandName);
            }
        }

        private void InitializeManagers()
        {
            Managers.Clear();
            Managers.Add("Любой");

            List<ManagerDTO> managerDTOs = _userService.GetAllManagers();

            foreach (var m in managerDTOs)
            {
                Managers.Add(m.FullName);
            }
        }

        private void InitializeFilterOptions()
        {
            Models.Clear();
            Models.Add("Любая");
        }

        // === КОМАНДЫ ===

        [RelayCommand]
        private void Refresh()
        {
            LoadAllOrders();
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
            SelectedBrand = "Любой";
            SelectedModel = "Любая";
            SelectedManager = "Любой";
            ClientName = "";
            PriceFrom = "";
            PriceTo = "";
            SearchText = "";

            UpdateModelsForBrand("Любой");
            FilterOrders();
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            FilterOrders();
            LoadCurrentPageOrders();
            IsFilterPanelVisible = false;
        }

        [RelayCommand]
        private async Task ShowOrderDetails(OrderModel order)
        {
            if (order == null) return;

            SelectedOrder = order;

            var parameters = new Dictionary<string, object>
        {
            { "Order", order }
        };

            await Shell.Current.GoToAsync(nameof(OrderDetailsModal), true, parameters);
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

            LoadCurrentPageOrders();
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

            LoadCurrentPageOrders();
        }

        [RelayCommand]
        private void FirstPage()
        {
            _currentPage = 1;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;

            LoadCurrentPageOrders();
        }

        [RelayCommand]
        private void LastPage()
        {
            _currentPage = _pageCount;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = _currentPage > 1 ? true : false;
            CanGoNextPage = false;

            LoadCurrentPageOrders();
        }

        // === ОБРАБОТЧИКИ ИЗМЕНЕНИЙ ===

        partial void OnSelectedBrandChanged(string value)
        {
            UpdateModelsForBrand(value);
            SelectedModel = Models[0];
        }

        private void UpdateModelsForBrand(string brand)
        {
            Models.Clear();
            Models.Add("Любая");

            if (brand == "Любой" || string.IsNullOrEmpty(brand))
                return;

            var brandModels = _allOrders
                .Where(o => o.Brand == brand)
                .Select(o => o.Model)
                .Distinct()
                .OrderBy(m => m);

            foreach (var model in brandModels)
            {
                Models.Add(model);
            }
        }

        // === ЗАГРУЗКА ДАННЫХ ===

        private void LoadAllOrders()
        {
            _allOrders.Clear();

            List<OrderDTO> orderDTOs = _orderService.GetAllOrders().Where(i => i.StatusId != 4).OrderByDescending(i => i.OrderDate).OrderByDescending(i => i.Id).ToList();

            foreach (var order in orderDTOs)
            {
                _allOrders.Add(new OrderModel(order));
            }

            FilterOrders();
        }

        private void FilterOrders()
        {
            // Фильтрация заказов

            var filtered = _allOrders.ToList();

            if (SelectedBrand != "Любой" && !string.IsNullOrEmpty(SelectedBrand))
                filtered = filtered.Where(o => o.Brand == SelectedBrand).ToList();

            if (SelectedModel != "Любая" && !string.IsNullOrEmpty(SelectedModel))
                filtered = filtered.Where(o => o.Model == SelectedModel).ToList();

            if (SelectedManager != "Любой" && !string.IsNullOrEmpty(SelectedManager))
                filtered = filtered.Where(o => o.ManagerName == SelectedManager).ToList();

            if (!string.IsNullOrWhiteSpace(ClientName))
                filtered = filtered.Where(o =>
                    o.CustomerName.Contains(ClientName, StringComparison.OrdinalIgnoreCase)).ToList();

            // Фильтрация по сумме заказа
            if (!string.IsNullOrEmpty(PriceFrom))
                filtered = filtered.Where(o => o.Price >= Decimal.Parse(PriceFrom)).ToList();

            if (!string.IsNullOrEmpty(PriceTo))
                filtered = filtered.Where(o => o.Price <= Decimal.Parse(PriceTo)).ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(o =>
                    o.Brand.ToLower().Contains(searchLower) ||
                    o.Model.ToLower().Contains(searchLower) ||
                    o.CustomerName.ToLower().Contains(searchLower) ||
                    o.VIN.ToLower().Contains(searchLower)).ToList();
            }

            // Обновляем отфильтрованную коллекцию
            FilteredOrders.Clear();
            foreach (var order in filtered)
            {
                FilteredOrders.Add(order);
            }

            _pageCount = (int)Math.Ceiling((decimal)FilteredOrders.Count / _pageSize);
            _currentPage = 1;
            CurPage = "1";
            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;
        }

        private void LoadCurrentPageOrders()
        {
            var currIndex = _currentPage - 1;

            var startIndex = currIndex * 20;
            var endIndex = (currIndex * 20 + 19) >= FilteredOrders.Count() ? FilteredOrders.Count() - 1 : (currIndex * 20 + 19);

            Orders.Clear();
            for (int i = startIndex; i <= endIndex; i++)
            {
                Orders.Add(FilteredOrders[i]);
            }

            if (Orders.Count > 5) HasFooterPageButtons = true;
            else HasFooterPageButtons = false;
        }
    }
}
