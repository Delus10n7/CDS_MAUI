using CDS_MAUI.ViewModels.ServiceContractsVM;

namespace CDS_MAUI.Views.ServiceContractsModal;

public partial class ServiceContractsOrderModal : ContentPage
{
	public ServiceContractsOrderModal(ServiceContractsOrderViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}