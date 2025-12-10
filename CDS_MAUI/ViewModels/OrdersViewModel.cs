using CDS_MAUI.Models;
using CDS_MAUI.Views.OrderDetailsModal;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CDS_MAUI.Views.OrdersPage;

namespace CDS_MAUI.ViewModels
{
    public partial class OrdersViewModel : BaseViewModel
    {
        // === КОЛЛЕКЦИИ ДАННЫХ ===
        [ObservableProperty]
        private ObservableCollection<OrderModel> _orders = new();

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

        public OrdersViewModel()
        {
            Title = "Заказы";
            Initialize();
        }

        private void Initialize()
        {
            InitializeBrands();
            InitializeManagers();
            InitializeFilterOptions();
            LoadTestOrders();
        }

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Любой");
            Brands.Add("Audi");
            Brands.Add("BMW");
            Brands.Add("Mercedes");
            Brands.Add("Toyota");
            Brands.Add("Honda");
        }

        private void InitializeManagers()
        {
            Managers.Clear();
            Managers.Add("Любой");
            Managers.Add("Анисимов И.Н.");
            Managers.Add("Ефимов С.А.");
            Managers.Add("Зверев А.Г.");
            Managers.Add("Крылов М.Ю.");
        }

        private void InitializeFilterOptions()
        {
            Models.Clear();
            Models.Add("Любая");
        }

        // === КОМАНДЫ ===

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
        private async Task EditOrder(OrderModel order)
        {
            if (order == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Редактирование заказа",
                $"Редактировать заказ {order.Brand} {order.Model}?",
                "Да",
                "Отмена");

            if (confirm)
            {
                IsBusy = true;
                try
                {
                    // Логика редактирования заказа
                    await Task.Delay(500);
                    await Shell.Current.DisplayAlert("Информация", "Редактирование заказа...", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        [RelayCommand]
        private async Task DeleteOrder(OrderModel order)
        {
            if (order == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Удаление заказа",
                $"Удалить заказ {order.Brand} {order.Model} клиента {order.CustomerName}?",
                "Удалить",
                "Отмена");

            if (confirm)
            {
                Orders.Remove(order);
                await Shell.Current.DisplayAlert("Успех", "Заказ удален", "OK");
            }
        }

        [RelayCommand]
        private void Search()
        {
            FilterOrders();
        }

        // === ОБРАБОТЧИКИ ИЗМЕНЕНИЙ ===

        partial void OnSelectedBrandChanged(string value)
        {
            UpdateModelsForBrand(value);
        }

        private void UpdateModelsForBrand(string brand)
        {
            Models.Clear();
            Models.Add("Любая");

            if (brand == "Любой" || string.IsNullOrEmpty(brand))
                return;

            var brandModels = Orders
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

        private void LoadTestOrders()
        {
            Orders.Clear();

            var testOrders = new List<OrderModel>
        {
            new OrderModel
            {
                Brand = "Honda",
                Model = "Accord",
                VIN = "JHMCM56557C404453",
                CustomerName = "Лебедев С.В.",
                ManagerName = "Анисимов И.Н.",
                Price = 800000.00m,
                Date = DateTime.Now.AddDays(-5)
            },
            new OrderModel
            {
                Brand = "BMW",
                Model = "7 Series",
                VIN = "WBA7E0C59GGM56193",
                CustomerName = "Ивин Г.А.",
                ManagerName = "Крылов М.Ю.",
                Price = 2150000.00m,
                Date = DateTime.Now.AddDays(-3)
            },
            new OrderModel
            {
                Brand = "Audi",
                Model = "A4",
                VIN = "WAUZZZ8KXBA123456",
                CustomerName = "Петров А.И.",
                ManagerName = "Зверев А.Г.",
                Price = 3200000.00m,
                Date = DateTime.Now.AddDays(-1)
            }
        };

            foreach (var order in testOrders)
            {
                Orders.Add(order);
            }

            UpdateModelsForBrand("Любой");
        }

        private void FilterOrders()
        {
            // Фильтрация заказов

            var filtered = Orders.AsEnumerable();

            if (SelectedBrand != "Любой")
                filtered = filtered.Where(o => o.Brand == SelectedBrand);

            if (SelectedModel != "Любая")
                filtered = filtered.Where(o => o.Model == SelectedModel);

            if (SelectedManager != "Любой")
                filtered = filtered.Where(o => o.ManagerName == SelectedManager);

            if (!string.IsNullOrWhiteSpace(ClientName))
                filtered = filtered.Where(o =>
                    o.CustomerName.Contains(ClientName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(o =>
                    o.Brand.ToLower().Contains(searchLower) ||
                    o.Model.ToLower().Contains(searchLower) ||
                    o.CustomerName.ToLower().Contains(searchLower) ||
                    o.VIN.ToLower().Contains(searchLower));
            }
        }
    }
}
