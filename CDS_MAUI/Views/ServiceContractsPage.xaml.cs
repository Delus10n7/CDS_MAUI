using CDS_MAUI.ViewModels.ServiceContractsVM;

namespace CDS_MAUI.Views;

public partial class ServiceContractsPage : ContentPage
{
	public ServiceContractsPage(ServiceContractsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}