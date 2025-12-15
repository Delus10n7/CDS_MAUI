using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CDS_MAUI.Models;
using CDS_Interfaces.Service;
using System.Collections.ObjectModel;
using CDS_Interfaces.DTO;

namespace CDS_MAUI.ViewModels.OrdersVM;

public partial class OrderDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private OrderModel _order;

    [ObservableProperty]
    private ObservableCollection<string> _orderStatuses = new();

    [ObservableProperty]
    private string _selectedOrderStatus = "";

    private List<OrderStatusDTO> _orderStatusDTOs;

    // === СЕРВИСЫ ===
    IOrderService _orderService;

    public OrderDetailsViewModel(IOrderService orderService)
    {
        _orderService = orderService;

        Title = "Детали заказа";
        InitializeOrderStatuses();
    }

    public void InitializeOrderStatuses()
    {
        OrderStatuses.Clear();

        _orderStatusDTOs = _orderService.GetAllOrderStatuses(); 

        foreach (var orderStatus in _orderStatusDTOs)
        {
            OrderStatuses.Add(orderStatus.StatusName);
        }
    }

    partial void OnOrderChanged(OrderModel value)
    {
        SelectedOrderStatus = Order.Status;
    }

    [RelayCommand]
    private async Task EditOrder()
    {
        if (Order == null) return;

        IsBusy = true;

        try
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Изменение заказа",
                $"Вы хотите изменить заказ на {Order.Brand} {Order.Model}?",
                "Да",
                "Отмена"
            );

            if (confirm)
            {
                // Логика редактирования заказа

                OrderDTO orderDTO = _orderService.GetOrder(Order.Id);
                OrderStatusDTO newOrderStatusDTO = _orderStatusDTOs.FirstOrDefault(i => i.StatusName == SelectedOrderStatus);
                orderDTO.StatusId = newOrderStatusDTO.Id;
                _orderService.UpdateOrder(orderDTO);

                await Shell.Current.DisplayAlert("Успех!", $"Заказ на {Order.Brand} {Order.Model} успешно отредактирован", "OK");

                await CloseModal();
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CloseModal()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}