using CDS_DomainModel.Entities;
using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CDS_MAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Audio;

namespace CDS_MAUI.ViewModels.CarsVM
{
    public partial class CarOrderViewModel : BaseViewModel
    {
        [ObservableProperty]
        private CarModel _car;

        [ObservableProperty]
        private ObservableCollection<string> _managers = new();

        [ObservableProperty]
        private ObservableCollection<string> _customers = new();

        [ObservableProperty]
        private ObservableCollection<CustomerModel> _filteredCustomers = new();

        [ObservableProperty]
        private string _discountPercent = "";

        [ObservableProperty]
        private string _salePrice = "";

        [ObservableProperty]
        private bool _isTradeIn = false;

        [ObservableProperty]
        private string _selectedManager;

        [ObservableProperty]
        private string _selectedCustomer;

        [ObservableProperty]
        private string _customerName = "";

        [ObservableProperty]
        private string _customerPhone = "";

        [ObservableProperty]
        private string _customerEmail = "";

        [ObservableProperty]
        private bool _newCustomer = false;

        [ObservableProperty]
        private bool _oldCustomer = false;

        [ObservableProperty]
        private string _searchText = "";

        private CustomerModel _selectedCustomerModel;

        // === СЕРВИСЫ ===
        ICarService _carService;
        ICarConfigurationService _carConfigService;
        IOrderService _orderService;
        IUserService _userService;
        IDiscountService _discountService;

        public CarOrderViewModel(ICarService carService, 
                                 ICarConfigurationService carConfigService, 
                                 IOrderService orderService, 
                                 IUserService userService, 
                                 IDiscountService discountService)
        {
            _carService = carService;
            _carConfigService = carConfigService;
            _orderService = orderService;
            _userService = userService;
            _discountService = discountService;

            Title = "Оформление заказа";
            InitializeManagersAndCustomers();
        }

        partial void OnCarChanged(CarModel value)
        {
            if (value != null)
            {
                GetDiscountedPrice();
            }
        }

        public void InitializeManagersAndCustomers()
        {
            Customers.Clear();
            Customers.Add("Не выбран");
            SelectedCustomer = Customers[0];

            Managers.Clear();
            Managers.Add("Не выбран");
            SelectedManager = Managers[0];

            List<ManagerDTO> managerDTOs = _userService.GetAllManagers();

            foreach (var m in managerDTOs)
            {
                Managers.Add(m.FullName);
            }
        }

        [RelayCommand]
        private async Task CloseModal()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private async Task CloseAllModal()
        {
            await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private void ShowNewCustomerMenu()
        {
            NewCustomer = !NewCustomer;
            OldCustomer = false;
        }

        [RelayCommand]
        private void ShowOldCustomerMenu()
        {
            NewCustomer = false;
            OldCustomer = !OldCustomer;
        }

        [RelayCommand]
        private void Search()
        {
            List<CustomerDTO> customerDTOs = _userService.GetAllCustomers();

            List<CustomerDTO> filtered = new List<CustomerDTO>();

            if (!string.IsNullOrEmpty(SearchText))
            {
                var searchLower = SearchText.ToLower();
                foreach (var customer in customerDTOs)
                {
                    filtered = customerDTOs.Where(c => 
                        c.FullName.ToLower().Contains(searchLower) ||
                        c.PhoneNumber.ToLower().Contains(searchLower) ||
                        c.Email.ToLower().Contains(searchLower)
                    ).ToList();
                }

                FilteredCustomers.Clear();
                foreach (var customer in filtered)
                {
                    FilteredCustomers.Add(new CustomerModel(customer));
                }
            }
            else
            {
                FilteredCustomers.Clear();
            }
        }

        [RelayCommand]
        private void SelectCustomer(CustomerModel customer)
        {
            NewCustomer = false;
            OldCustomer = false;

            if (customer == null) return;

            Customers.Clear();
            Customers.Add("Не выбран");
            Customers.Add(customer.FullName);

            SelectedCustomer = customer.FullName;
            _selectedCustomerModel = customer;

            GetDiscountedPrice();
        }

        [RelayCommand]
        private void SelectNewCustomer()
        {
            NewCustomer = false;
            OldCustomer = false;

            List<CustomerDTO> customerDTOs = new List<CustomerDTO>();
            List<UserDTO> userDTOs = new List<UserDTO>();
            int newCustomerId = 0;
            CustomerModel newCustomer;
            CustomerDTO newCustomerDTO;

            if (!string.IsNullOrEmpty(CustomerName) && !string.IsNullOrEmpty(CustomerPhone) && !string.IsNullOrEmpty(CustomerEmail))
            {
                customerDTOs = _userService.GetAllCustomers();
                foreach (var c in customerDTOs)
                {
                    if (c.PhoneNumber == CustomerPhone)
                    {
                        Shell.Current.DisplayAlert(
                            "Клиент уже существует!",
                            $"Клиент с номером телефона {CustomerPhone} уже существует",
                            "ОК");
                        return;
                    }
                    if (c.Email == CustomerEmail)
                    {
                        Shell.Current.DisplayAlert(
                            "Клиент уже существует!",
                            $"Клиент с email {CustomerEmail} уже существует",
                            "ОК");
                        return;
                    }
                }

                Customers.Clear();
                Customers.Add("Не выбран");
                Customers.Add(CustomerName);

                SelectedCustomer = CustomerName;

                userDTOs = _userService.GetAllUsers();
                newCustomerId = userDTOs.Count + 1;

                newCustomerDTO = new CustomerDTO(CustomerName, CustomerPhone, CustomerEmail, newCustomerId);
                _userService.CreateUser(newCustomerDTO);

                newCustomerDTO = _userService.GetAllCustomers().FirstOrDefault(i => i.Email == CustomerEmail);
                newCustomer = new CustomerModel(newCustomerDTO);
                _selectedCustomerModel = newCustomer;

                GetDiscountedPrice();

                Shell.Current.DisplayAlert(
                            "Клиент успешно создан!",
                            $"Клиент {CustomerName} успешно создан",
                            "ОК");
            }
        }

        [RelayCommand]
        private async Task MakeOrder()
        {
            await CloseAllModal();
        }

        private void CreateNewOrder()
        {

        }

        private void GetDiscountedPrice()
        {
            DiscountPercent = "0.00 %";
            SalePrice = Car.FormattedPrice;
            decimal? discountPercent = (decimal)0.0;

            List<DiscountDTO> discountDTOs = _discountService.GetAllDicsounts().Where(i => i.IsActive == true && 
                i.EndDate.Value > DateOnly.FromDateTime(DateTime.Now)).ToList();

            List<DiscountDTO> loyaltyDiscounts = new List<DiscountDTO>();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            CustomerDTO customer = new CustomerDTO();

            foreach (var discount in discountDTOs)
            {
                if (discount.ModelId != null && discount.ModelId == Car.ModelId 
                    && discount.DiscountPercent > discountPercent)
                {
                    discountPercent = discount.DiscountPercent;
                }
                else if (discount.BrandId != null && discount.BrandId == Car.BrandId 
                    && discount.DiscountPercent > discountPercent)
                {
                    discountPercent = discount.DiscountPercent;
                }
                else if (IsTradeIn && discount.DiscountTypeId == 5 
                    && discount.DiscountPercent > discountPercent)
                {
                    discountPercent = discount.DiscountPercent;
                }
                else if (discount.DiscountTypeId == 4)
                {
                    loyaltyDiscounts.Add(discount);
                }
            }

            if (SelectedCustomer != "Не выбран")
            {
                customer = _userService.GetCustomer(_selectedCustomerModel.Id);
                orderDTOs = _orderService.GetAllOrders().Where(i => i.ClientId == customer.Id).ToList();
                decimal? loyaltyDiscountPercent = (decimal)0.0;

                foreach (var discount in loyaltyDiscounts)
                {
                    if (discount.DiscountName == "Скидка постоянного клиента" && orderDTOs.Count >= 2)
                    {
                        loyaltyDiscountPercent += discount.DiscountPercent;
                    }

                    if (discount.DiscountName == "Бонус за вторую покупку" && orderDTOs.Count >= 1)
                    {
                        loyaltyDiscountPercent += discount.DiscountPercent;
                    }
                }

                if (loyaltyDiscountPercent > discountPercent) discountPercent = loyaltyDiscountPercent;
            }

            DiscountPercent = discountPercent?.ToString() + " %";
            if (discountPercent > 0) SalePrice = (Car.Price - (Car.Price * (discountPercent / 100)))?.ToString("N0") + " руб.";
        }
    }
}
