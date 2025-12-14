using CDS_MAUI.ViewModels.OrdersVM;
using CDS_MAUI.Models;

namespace CDS_MAUI.Views.OrdersModal;

public partial class OrderDetailsModal : ContentPage, IQueryAttributable
{
    public OrderDetailsViewModel ViewModel { get; }

    public OrderDetailsModal(OrderDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Order", out var order) && order is OrderModel orderModel)
        {
            ViewModel.Order = orderModel;
        }
    }
}