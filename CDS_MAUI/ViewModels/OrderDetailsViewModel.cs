using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CDS_MAUI.Models;

namespace CDS_MAUI.ViewModels;

public partial class OrderDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private OrderModel _order;

    public OrderDetailsViewModel()
    {
        Title = "Детали заказа";
    }

    [RelayCommand]
    private async Task EditOrder()
    {
        if (Order == null) return;

        IsBusy = true;

        try
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Изменение заказа",
                $"Вы хотите изменить заказ на {Order.Brand} {Order.Model}?",
                "Да",
                "Отмена"
            );

            if (confirm)
            {
                // Логика редактирования заказа
                await Task.Delay(500);
                await Shell.Current.DisplayAlert("Информация", "Редактирование заказа...", "OK");
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