using CDS_DomainModel.Entities;
using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CDS_MAUI.Models;
using CDS_MAUI.PdfGenerator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.BouncyCastle.Crypto;
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
        // === ОСНОВНОЕ ===

        [ObservableProperty]
        private CarModel _car;

        [ObservableProperty]
        private ObservableCollection<string> _managers = new();

        [ObservableProperty]
        private string _selectedManager;

        [ObservableProperty]
        private ObservableCollection<string> _customers = new();

        [ObservableProperty]
        private string _selectedCustomer;

        [ObservableProperty]
        private ObservableCollection<CustomerModel> _filteredCustomers = new();

        private CustomerModel _selectedCustomerModel;

        // === СКИДКА ===

        [ObservableProperty]
        private string _discountPercent = "";

        [ObservableProperty]
        private string _salePriceFormatted = "";

        private decimal? _salePrice;

        // === КЛИЕНТ ===

        [ObservableProperty]
        private string _searchText = "";

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

        // === ТРЕЙД-ИН ===

        [ObservableProperty]
        private ObservableCollection<string> _tradeInCars = new();

        [ObservableProperty]
        private string _selectedTradeInCar;

        [ObservableProperty]
        private string _tradeInCarBrand = "";

        [ObservableProperty]
        private string _tradeInCarModel = "";

        [ObservableProperty]
        private string _tradeInCarVIN = "";

        [ObservableProperty]
        private string _tradeInCarYear = "";

        [ObservableProperty]
        private string _tradeInCarMileage = "";

        [ObservableProperty]
        private string _tradeInCarEngineVolume = "";

        [ObservableProperty]
        private string _tradeInCarEnginePower = "";

        [ObservableProperty]
        private ObservableCollection<string> _engineTypes = new();

        [ObservableProperty]
        private string _selectedEngineType = "";

        [ObservableProperty]
        private ObservableCollection<string> _transmissionTypes = new();

        [ObservableProperty]
        private string _selectedTransmission = "";

        [ObservableProperty]
        private ObservableCollection<string> _driveTypes = new();

        [ObservableProperty]
        private string _selectedDriveType = "";

        [ObservableProperty]
        private ObservableCollection<string> _bodyTypes = new();

        [ObservableProperty]
        private string _selectedBodyType = "";

        [ObservableProperty]
        private string _tradeInCarColor = "";

        [ObservableProperty]
        private string _tradeInCarPrice = "";

        [ObservableProperty]
        private string _tradeInCarPriceFormatted = "";

        [ObservableProperty]
        private bool _isTradeIn = false;

        [ObservableProperty]
        private bool _tradeInMenuVisible = false;

        // === СЕРВИСЫ ===
        ICarService _carService;
        ICarConfigurationService _carConfigService;
        IOrderService _orderService;
        IUserService _userService;
        IDiscountService _discountService;

        // === PDF Генератор ===
        CarContractPdfGenerator gen;
        CarContractDataModel data;
        CarContractDataModel tradeInData = new CarContractDataModel();

        [ObservableProperty]
        private string _selectedOutputFolderPath = "";

        [ObservableProperty]
        private string _outputFolderPathButtonText = "";

        int skip = 0;

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

            gen = new CarContractPdfGenerator();
            data = new CarContractDataModel();

            Title = "Оформление заказа";
            InitializeManagersAndCustomers();
            InitializeCarConfigurations();

            OutputFolderPathButtonText = "Выбрать папку для сохранения ДКП";
        }

        partial void OnCarChanged(CarModel value)
        {
            if (value != null)
            {
                GetDiscountedPrice();
            }
        }

        partial void OnSelectedTradeInCarChanged(string value)
        {
            if (value != "Нет" && value != string.Empty)
            {
                IsTradeIn = true;
            }
            else IsTradeIn = false;

            if (skip > 0) GetDiscountedPrice();
            else skip++;
        }

        partial void OnSelectedCustomerChanged(string value)
        {
            if (value != "Не выбран" && !string.IsNullOrEmpty(value))
            {
                if (IsTradeIn) tradeInData.CustomerFullName = value;
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

            TradeInCars.Clear();
            TradeInCars.Add("Нет");
            SelectedTradeInCar = TradeInCars[0];

            List<ManagerDTO> managerDTOs = _userService.GetAllManagers();

            foreach (var m in managerDTOs)
            {
                Managers.Add(m.FullName);
            }
        }

        private void InitializeCarConfigurations()
        {
            BodyTypes.Clear();
            EngineTypes.Clear();
            TransmissionTypes.Clear();
            DriveTypes.Clear();

            List<BodyTypeDTO> bodyTypeDTOs = _carConfigService.GetAllBodyTypes();
            List<EngineTypeDTO> engineTypeDTOs = _carConfigService.GetAllEngineTypes();
            List<TransmissionTypeDTO> transmissionTypeDTOs = _carConfigService.GetAllTransmissionTypes();
            List<DriveTypeDTO> driveTypeDTOs = _carConfigService.GetAllDriveTypes();

            foreach (var b in bodyTypeDTOs)
            {
                BodyTypes.Add(b.BodyName);
            }
            foreach (var e in engineTypeDTOs)
            {
                EngineTypes.Add(e.EngineName);
            }
            foreach (var t in transmissionTypeDTOs)
            {
                TransmissionTypes.Add(t.TransmissionName);
            }
            foreach (var d in driveTypeDTOs)
            {
                DriveTypes.Add(d.DriveName);
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
        private void ShowTradeInMenu()
        {
            TradeInMenuVisible = !TradeInMenuVisible;
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
        private async Task AddTradeInCar()
        {
            var flag = await TradeInSubmit();
            if (!flag) return;

            TradeInCars.Clear();
            TradeInCars.Add("Нет");
            TradeInCars.Add(TradeInCarBrand + " " + TradeInCarModel);
            SelectedTradeInCar = TradeInCars[1];

            TradeInMenuVisible = false;

            TradeInCarPriceFormatted = Convert.ToDecimal(TradeInCarPrice).ToString("N0") + " руб.";

            GetDiscountedPrice();

            if (_selectedCustomerModel != null) tradeInData.CustomerFullName = _selectedCustomerModel.FullName;

            tradeInData.CarBrand = TradeInCarBrand;
            tradeInData.CarModel = TradeInCarModel;
            tradeInData.CarVIN = TradeInCarVIN;
            tradeInData.CarReleaseYear = TradeInCarYear;
            tradeInData.CarMileage = TradeInCarMileage;
            tradeInData.CarEngineVolume = TradeInCarEngineVolume;
            tradeInData.CarEnginePower = TradeInCarEnginePower;
            tradeInData.CarEngineType = SelectedEngineType;
            tradeInData.CarTransmissionType = SelectedTransmission;
            tradeInData.CarDriveType = SelectedDriveType;
            tradeInData.CarBodyType = SelectedBodyType;
            tradeInData.CarColor = TradeInCarColor;

            tradeInData.OrderSalePrice = Convert.ToDecimal(TradeInCarPrice);
        }

        [RelayCommand]
        private async Task SelectOutputFileFolder()
        {
            try
            {
                // Для Windows используем нативный диалог
                #if WINDOWS
                SelectedOutputFolderPath = await PickFolderWindows();

                #else
                var result = await FolderPicker.Default.PickAsync();
                if (result != null)
                {
                    _selectedFolderPath = result.FullPath;
                }
                #endif

                if (!string.IsNullOrEmpty(SelectedOutputFolderPath))
                {
                    OutputFolderPathButtonText = SelectedOutputFolderPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось выбрать папку: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task MakeOrder()
        {
            await CreateNewOrder();
        }

        private async Task CreateNewOrder()
        {
            IsBusy = true;

            try
            {
                var flag = await Submit();
                if (!flag) return;

                bool confirm = await Shell.Current.DisplayAlert(
                    "Изменение заказа",
                    $"Вы хотите оформить {Car.Brand} {Car.Model} на сумму {SalePriceFormatted}?",
                    "Да",
                    "Отмена"
                );

                if (confirm)
                {
                    // Логика оформления заказа
                    
                    CarDTO carDTO = _carService.GetCar(Car.Id);
                    CustomerDTO customerDTO = _userService.GetCustomer(_selectedCustomerModel.Id);
                    ManagerDTO managerDTO = _userService.GetAllManagers().FirstOrDefault(m => m.FullName == SelectedManager);

                    // Генерация PDF
                    try
                    {
                        data = new CarContractDataModel(Car, _salePrice, _selectedCustomerModel.FullName);
                        //gen.FillPdfTemplate(data, SelectedTemplateFile.FullPath, SelectedOutputFolderPath);
                        //if (IsTradeIn) gen.FillPdfTemplate(tradeInData, SelectedTradeInTemplateFile.FullPath, SelectedOutputFolderPath);

                        gen.GenerateContractPdf(data, SelectedOutputFolderPath);
                        if (IsTradeIn) gen.GenerateTradeInContractPdf(tradeInData, SelectedOutputFolderPath);
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ошибка!", $"Ошибка генерации договора: {ex.Message}", "ОК");
                        return;
                    }

                    OrderDTO newOrder = new OrderDTO();
                    newOrder.IsTradeIn = IsTradeIn;
                    if (IsTradeIn) newOrder.TradeInValue = Convert.ToDecimal(TradeInCarPrice);
                    else newOrder.TradeInValue = (decimal)0.0;
                    newOrder.ClientId = customerDTO.Id;
                    newOrder.ManagerId = managerDTO.Id;
                    newOrder.CarId = carDTO.Id;
                    newOrder.OrderDate = DateOnly.FromDateTime(DateTime.Now);
                    newOrder.StatusId = 3; // В обработке
                    newOrder.SalePrice = _salePrice;

                    _orderService.CreateOrder(newOrder);

                    carDTO.AvailabilityId = 3; // Продана
                    _carService.UpdateCar(carDTO);

                    await Shell.Current.DisplayAlert("Успех!", $"Заказ {Car.Brand} {Car.Model} на сумму {SalePriceFormatted} успешно создан\n" +
                        $"PDF договор сохранен в папку {SelectedOutputFolderPath}", "OK");

                    await CloseAllModal();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void GetDiscountedPrice()
        {
            DiscountPercent = "0.00 %";
            SalePriceFormatted = Car.FormattedPrice;
            _salePrice = Car.Price;
            decimal? discountPercent = (decimal)0.0;
            decimal? tradeInCarValue = (decimal)0.0;

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
                else if (SelectedTradeInCar != "Нет" && !string.IsNullOrEmpty(SelectedTradeInCar)
                    && discount.DiscountTypeId == 5 && discount.DiscountPercent > discountPercent)
                {
                    discountPercent = discount.DiscountPercent;
                    loyaltyDiscounts.Add(discount);
                    tradeInCarValue = Convert.ToDecimal(TradeInCarPrice);
                }
                if (discount.DiscountTypeId == 4)
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
                    if (discount.OrdersNeeded == null) loyaltyDiscountPercent += discount.DiscountPercent;
                    else if (discount.OrdersNeeded <= orderDTOs.Count)
                    {
                        loyaltyDiscountPercent += discount.DiscountPercent;
                    }
                }

                if (loyaltyDiscountPercent > discountPercent) discountPercent = loyaltyDiscountPercent;
            }

            DiscountPercent = discountPercent?.ToString() + " %";
            
            if (discountPercent > 0)
            {
                if (tradeInCarValue != (decimal)0.0)
                {
                    var price = Car.Price;
                    price -= tradeInCarValue;

                    var priceWithTradeIn = (price - (price * (discountPercent / 100)));
                    SalePriceFormatted = priceWithTradeIn?.ToString("N0") + " руб.";

                    _salePrice = (Car.Price - (Car.Price * (discountPercent / 100)));
                }
                else
                {
                    _salePrice = (Car.Price - (Car.Price * (discountPercent / 100)));
                    SalePriceFormatted = _salePrice?.ToString("N0") + " руб.";
                }
            }
        }

        private async Task<bool> Submit()
        {
            if (_selectedCustomerModel == null)
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран килент", "ОК");
                return false;
            }
            if (SelectedManager == "Не выбран" || string.IsNullOrEmpty(SelectedManager))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран менеджер", "ОК");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedOutputFolderPath))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбрана папка для сохранения договора", "ОК");
                return false;
            }
            return true;
        }

        private async Task<bool> TradeInSubmit()
        {
            if (string.IsNullOrEmpty(TradeInCarBrand))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введена марка авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarModel))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введена модель авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarVIN))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введен VIN номер авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarYear))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введен год выпуска авто в трейд-ин", "ОК");
                return false; 
            }
            if (string.IsNullOrEmpty(TradeInCarMileage))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введен пробег авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarEngineVolume))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введен объем двигателя авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarEnginePower))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введена мощность двигателя авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedEngineType))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран тип двигателя авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedTransmission))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран тип трансмиссии авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedDriveType))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран тип привода авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedBodyType))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не выбран тип кузова авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarColor))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введен цвет авто в трейд-ин", "ОК");
                return false;
            }
            if (string.IsNullOrEmpty(TradeInCarPrice))
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не введена цена авто в трейд-ин", "ОК");
                return false;
            }
            return true;
        }

        // Метод для Windows-специфичного выбора папки
        #if WINDOWS
        private async Task<string> PickFolderWindows()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();

            // Получаем handle окна MAUI
            var window = MauiWinUIApplication.Current.Application.Windows[0].Handler.PlatformView;
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            // Настройки диалога
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");

            var folder = await folderPicker.PickSingleFolderAsync();
            return folder?.Path;
        }
        #endif
    }
}
