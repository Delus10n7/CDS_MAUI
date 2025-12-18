using CDS_MAUI.ViewModels.ReportsVM;

namespace CDS_MAUI.Views;

public partial class ReportsPage : ContentPage
{
	public ReportsPage(ReportsViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
	}
}