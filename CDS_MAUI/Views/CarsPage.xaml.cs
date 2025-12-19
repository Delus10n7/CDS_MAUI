using CDS_MAUI.Models;
using CDS_MAUI.ViewModels.CarsVM;
using CDS_MAUI.ViewModels.OrdersVM;
using Microsoft.Maui.Controls;

namespace CDS_MAUI.Views;

public partial class CarsPage : ContentPage
{
    public CarsPage(CarsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CarsViewModel viewModel)
        {
            
        }
    }
}