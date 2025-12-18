using CDS_MAUI.Models;
using CDS_MAUI.ViewModels.OrdersVM;
using Microsoft.Maui.Controls;

namespace CDS_MAUI.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage(OrdersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is OrdersViewModel viewModel)
        {
            
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderModel order)
        {
            if (BindingContext is OrdersViewModel viewModel)
            {
                await viewModel.ShowOrderDetailsCommand.ExecuteAsync(order);
            }
        }
    }
}