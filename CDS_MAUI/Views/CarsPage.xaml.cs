using CDS_MAUI.ViewModels.CarsVM;
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