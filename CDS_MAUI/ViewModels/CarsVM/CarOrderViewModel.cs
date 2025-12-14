using CDS_Interfaces.Service;
using CDS_Interfaces.DTO;
using CDS_MAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.ViewModels.CarsVM
{
    public partial class CarOrderViewModel : BaseViewModel
    {
        [ObservableProperty]
        private CarModel _car;

        [ObservableProperty]
        private List<string> _managers = new();

        [ObservableProperty]
        private string _selectedManager;

        [ObservableProperty]
        private string _customerName;

        [ObservableProperty]
        private string _customerPhone;

        [ObservableProperty]
        private string _customerEmail;

        // === СЕРВИСЫ ===
        ICarService _carService;
        ICarConfigurationService _carConfigService;
        IOrderService _orderService;
        IUserService _userService;

        public CarOrderViewModel(ICarService carService, ICarConfigurationService carConfigService, IOrderService orderService, IUserService userService)
        {
            _carService = carService;
            _carConfigService = carConfigService;
            _orderService = orderService;
            _userService = userService;

            Title = "Оформление заказа";
            InitializeManagers();
        }

        public void InitializeManagers()
        {
            Managers.Clear();
            Managers.Add("Не выбран");

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
    }
}
