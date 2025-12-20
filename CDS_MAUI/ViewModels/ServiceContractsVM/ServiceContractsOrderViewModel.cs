using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
using CDS_MAUI.Models;
using CDS_MAUI.PdfGenerator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.ViewModels.ServiceContractsVM
{
    public partial class ServiceContractsOrderViewModel : BaseViewModel
    {
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

        [ObservableProperty]
        private ObservableCollection<AdditionalServiceItemModel> _additionalServiceItems = new();

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

        // === ДОБАВЛЕНИЕ ДОП УСЛУГИ ===

        [ObservableProperty]
        private bool _isAddAdditionalServicePanelVisible = false;

        private List<AdditionalServiceDTO> _additionalServiceDTOs = new();

        [ObservableProperty]
        private ObservableCollection<string> _additionalServices = new();

        [ObservableProperty]
        private string _selectedAdditionalService = "";

        [ObservableProperty]
        private int _selectedQuantity = 1;

        [ObservableProperty]
        private string _selectedPrice = "";

        // === PDF Генератор ===
        PdfGenerator.PdfGenerator gen;
        ServiceContractDataModel data;

        [ObservableProperty]
        private string _selectedOutputFolderPath = "";

        [ObservableProperty]
        private string _outputFolderPathButtonText = "";

        [ObservableProperty]
        private string _generatedContractFilePath = "";

        // === СЕРВИСЫ ===
        IUserService _userService;
        IServiceContractsService _serviceContractsService;

        public ServiceContractsOrderViewModel(IUserService userService, IServiceContractsService serviceContractsService)
        {
            _userService = userService;
            _serviceContractsService = serviceContractsService;

            gen = new PdfGenerator.PdfGenerator();
            data = new ServiceContractDataModel();

            Title = "Оформление заказа на дополнительные услуги";
            InitializeManagersAndCustomers();
            InitializeAdditionalServices();

            OutputFolderPathButtonText = "Выбрать папку для сохранения договора на доп услуги";
            IsAddAdditionalServicePanelVisible = false;
            SelectedQuantity = 1;
            SalePriceFormatted = "0 руб.";
        }

        private void InitializeManagersAndCustomers()
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

        private void InitializeAdditionalServices()
        {
            AdditionalServices.Clear();
            AdditionalServices.Add("Не выбрана");
            SelectedAdditionalService = AdditionalServices[0];

            _additionalServiceDTOs = _serviceContractsService.GetAllAdditionalServices();

            foreach(var a in _additionalServiceDTOs)
            {
                AdditionalServices.Add(a.ServiceName);
            }
        }

        private void UpdatePriceForAdditionalService(string additionalServiceName)
        {
            if (!string.IsNullOrEmpty(additionalServiceName) && additionalServiceName != "Не выбрана")
            {
                var selected = _additionalServiceDTOs.FirstOrDefault(i => i.ServiceName == additionalServiceName);

                if (selected != null)
                {
                    SelectedPrice = selected.Price?.ToString("N0") ?? string.Empty;
                }
            }
        }

        private void UpdatePriceForAdditionalService(int additionalServiceQuantity)
        {
            if (!string.IsNullOrEmpty(SelectedAdditionalService) && SelectedAdditionalService != "Не выбрана")
            {
                var selected = _additionalServiceDTOs.FirstOrDefault(i => i.ServiceName == SelectedAdditionalService);

                if (selected != null && !string.IsNullOrEmpty(SelectedPrice))
                {
                    var price = selected.Price;

                    SelectedPrice = (price * additionalServiceQuantity)?.ToString("N0") ?? string.Empty;
                }
            }
        }

        partial void OnSelectedAdditionalServiceChanged(string value)
        {
            UpdatePriceForAdditionalService(value);
        }

        partial void OnSelectedQuantityChanged(int value)
        {
            UpdatePriceForAdditionalService(value);
        }

        [RelayCommand]
        private async Task CloseModal()
        {
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

                Shell.Current.DisplayAlert(
                            "Клиент успешно создан!",
                            $"Клиент {CustomerName} успешно создан",
                            "ОК");
            }
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
        private void ShowAddAdditionalServicePanel()
        {
            IsAddAdditionalServicePanelVisible = !IsAddAdditionalServicePanelVisible;
        }

        [RelayCommand]
        private void AddNewAdditionalServiceItem()
        {
            if (!string.IsNullOrEmpty(SelectedAdditionalService) && SelectedAdditionalService != "Не выбрана" && !string.IsNullOrEmpty(SelectedPrice))
            {
                AdditionalServiceItems.Insert(0, new AdditionalServiceItemModel(SelectedAdditionalService, SelectedQuantity, Convert.ToDecimal(SelectedPrice)));
            }
            else
            {
                Shell.Current.DisplayAlert("Ошибка", "Не выбрана дополнительная услуга", "ОК");
                return;
            }

            ResetAddAdditionalServicePanel();
            CalculateSalePrice();
        }

        [RelayCommand]
        private void DeleteAdditionalServiceItem(AdditionalServiceItemModel item)
        {
            AdditionalServiceItems.Remove(item);

            CalculateSalePrice();
        }

        [RelayCommand]
        private async Task MakeServiceContractOrder()
        {
            await CreateNewServiceContractOrder();
        }

        private async Task CreateNewServiceContractOrder()
        {
            IsBusy = true;

            try
            {
                var flag = await Submit();
                if (!flag)
                {
                    IsBusy = false;
                    return;
                }

                bool confirm = await Shell.Current.DisplayAlert(
                    "Оформление заказа",
                    $"Вы хотите оформить заказ на доп услуги на сумму {SalePriceFormatted}?",
                    "Да",
                    "Отмена"
                );

                if (confirm)
                {
                    ServiceContractDTO serviceContractDTO = new ServiceContractDTO();
                    CustomerDTO customerDTO = _userService.GetCustomer(_selectedCustomerModel.Id);
                    ManagerDTO managerDTO = _userService.GetAllManagers().FirstOrDefault(m => m.FullName == SelectedManager);

                    serviceContractDTO.ClientId = customerDTO.Id;
                    serviceContractDTO.ManagerId = managerDTO.Id;
                    serviceContractDTO.TotalPrice = _salePrice;
                    var date = DateOnly.FromDateTime(DateTime.Now);
                    serviceContractDTO.SaleDate = date;

                    _serviceContractsService.CreateServiceContract(serviceContractDTO);

                    serviceContractDTO = _serviceContractsService.GetAllServiceContracts().FirstOrDefault(i =>
                                            i.ClientId == serviceContractDTO.ClientId &&
                                            i.ManagerId == serviceContractDTO.ManagerId &&
                                            i.TotalPrice == serviceContractDTO.TotalPrice &&
                                            i.SaleDate == date);

                    SelectedServiceDTO selectedServiceDTO = new SelectedServiceDTO();
                    foreach(var a in AdditionalServiceItems)
                    {
                        int serviceContractId = serviceContractDTO.Id;
                        int additionalServiceId = _additionalServiceDTOs.FirstOrDefault(i => i.ServiceName == a.Name).Id;

                        selectedServiceDTO.ServiceContractId = serviceContractId;
                        selectedServiceDTO.AdditionalServiceId = additionalServiceId;
                        selectedServiceDTO.Quantity = a.Quantity;
                        selectedServiceDTO.TotalPrice = a.Price;
                        _serviceContractsService.CreateSelectedService(selectedServiceDTO);
                    }

                    // Генерация PDF
                    data = new ServiceContractDataModel(serviceContractDTO);
                    data.AdditionalServiceItems = AdditionalServiceItems;
                    GeneratedContractFilePath = gen.GenerateServiceContractPdf(data, SelectedOutputFolderPath);

                    await Shell.Current.DisplayAlert("Успех!", $"Заказ на сумму {SalePriceFormatted} успешно создан\n" +
                        $"PDF договор сохранен в папку {SelectedOutputFolderPath}", "OK");

                    await CloseModal();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculateSalePrice()
        {
            _salePrice = 0;
            SalePriceFormatted = "0 руб.";

            if (AdditionalServiceItems.Count > 0)
            {
                decimal? price = (decimal)0.0;
                foreach (var a in AdditionalServiceItems)
                {
                    price += a.Price;
                }
                _salePrice = price;
                SalePriceFormatted = _salePrice?.ToString("N0") + " руб." ?? "0 руб.";
            }
        }

        private void ResetAddAdditionalServicePanel()
        {
            InitializeAdditionalServices();
            SelectedQuantity = 1;
            SelectedPrice = "";
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

            if (AdditionalServiceItems.Count == 0)
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не добавлены дополнительные услуги", "ОК");
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
