using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CDS_MAUI.Models;
using CDS_Interfaces.Service;
using CDS_MAUI.Views.CarDetailsModal;

namespace CDS_MAUI.ViewModels.CarsVM;

public partial class CarDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private CarModel _car;

    // === СЕРВИСЫ ===
    ICarService _carService;
    ICarConfigurationService _carConfigService;
    IOrderService _orderService;
    IUserService _userService;

    public CarDetailsViewModel(ICarService carService, ICarConfigurationService carConfigService, IOrderService orderService, IUserService userService)
    {
        _carService = carService;
        _carConfigService = carConfigService;
        _orderService = orderService;
        _userService = userService;

        Title = "Детали автомобиля";
    }

    [RelayCommand]
    private async Task ProcessOrder()
    {
        if (Car == null) return;

        IsBusy = true;

        try
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Оформление заказа",
                $"Вы уверены, что хотите оформить заказ на\n\n{Car.Brand} {Car.Model}\nVIN: {Car.VIN}\n\nза {Car.Price:N0} руб.?",
                "Да, оформить",
                "Отмена"
            );

            if (confirm)
            {
                // Логика оформления заказа
                var parameters = new Dictionary<string, object>()
                {
                    {"Car", Car}
                };

                //await Shell.Current.GoToAsync(nameof(CarOrderModal), true, parameters);

                await Shell.Current.DisplayAlert("Успех", "Заказ успешно оформлен!", "OK");

                // Закрываем модальное окно
                await Shell.Current.Navigation.PopModalAsync();
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CloseModal()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}