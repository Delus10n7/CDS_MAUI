using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CDS_MAUI.Models;

namespace CDS_MAUI.ViewModels;

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
            bool confirm = await Shell.Current.DisplayAlert(
                "Оформление заказа",
                $"Вы уверены, что хотите оформить заказ на {Car.Brand} {Car.Model} за {Car.Price:N0} руб.?",
                "Да, оформить",
                "Отмена"
            );

            if (confirm)
            {
                // Логика оформления заказа
                await Task.Delay(1000); // Имитация процесса
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