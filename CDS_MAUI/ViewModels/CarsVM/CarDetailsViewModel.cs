using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CDS_MAUI.Models;
using CDS_Interfaces.Service;
using CDS_MAUI.Views.CarsModal;

namespace CDS_MAUI.ViewModels.CarsVM;

public partial class CarDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private CarModel _car;

    public CarDetailsViewModel()
    {
        Title = "Детали автомобиля";
    }

    [RelayCommand]
    private async Task ProcessOrder()
    {
        if (Car == null) return;

        IsBusy = true;

        try
        {
            // Логика оформления заказа
            var parameters = new Dictionary<string, object>()
                {
                    {"Car", Car}
                };

            await Shell.Current.GoToAsync(nameof(CarOrderModal), true, parameters);
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