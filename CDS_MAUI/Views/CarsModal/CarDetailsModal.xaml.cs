using CDS_MAUI.ViewModels.CarsVM;
using CDS_MAUI.Models;

namespace CDS_MAUI.Views.CarsModal;

public partial class CarDetailsModal : ContentPage, IQueryAttributable
{
    public CarDetailsViewModel ViewModel { get; }

    public CarDetailsModal(CarDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Car", out var car) && car is CarModel carModel)
        {
            ViewModel.Car = carModel;
        }
    }
}