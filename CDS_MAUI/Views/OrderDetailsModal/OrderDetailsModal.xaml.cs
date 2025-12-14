using CDS_MAUI.ViewModels.OrdersVM;
using CDS_MAUI.Models;

namespace CDS_MAUI.Views.OrderDetailsModal;

[QueryProperty(nameof(Order), "Order")]
public partial class OrderDetailsModal : ContentPage
{
    private OrderModel _order;

    public OrderModel Order
    {
        get => _order;
        set
        {
            _order = value;
            OnPropertyChanged();

            if (BindingContext is OrderDetailsViewModel viewModel)
            {
                viewModel.Order = value;
            }
        }
    }

    public OrderDetailsModal(OrderDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}