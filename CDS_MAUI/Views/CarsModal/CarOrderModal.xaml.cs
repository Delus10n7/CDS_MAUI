using CDS_MAUI.ViewModels.CarsVM;
using CDS_MAUI.Models;

namespace CDS_MAUI.Views.CarsModal;

[QueryProperty(nameof(Car), "Car")]
public partial class CarOrderModal : ContentPage
{
	private CarModel _car;

	public CarModel Car
	{
		get => _car;
		set 
		{
            _car = value;
            OnPropertyChanged();

            if (BindingContext is CarOrderViewModel viewModel)
            {
                viewModel.Car = value;
            }
        }
	}

	public CarOrderModal(CarOrderViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}