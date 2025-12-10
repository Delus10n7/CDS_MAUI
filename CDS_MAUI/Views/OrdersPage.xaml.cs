using CDS_MAUI.ViewModels;
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
}