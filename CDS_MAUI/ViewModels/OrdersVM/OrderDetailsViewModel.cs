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
    ICarService _carService;

    public OrderDetailsViewModel(IOrderService orderService, ICarService carService)
    {
        _orderService = orderService;
        _carService = carService;

        Title = "Детали заказа";
        InitializeOrderStatuses();
    }

    public void InitializeOrderStatuses()
    {
        OrderStatuses.Clear();

        _orderStatusDTOs = _orderService.GetAllOrderStatuses(); 

        foreach (var orderStatus in _orderStatusDTOs)
        {
            if (orderStatus.StatusName != "Отменен")
                OrderStatuses.Add(orderStatus.StatusName);
        }
    }

    partial void OnOrderChanged(OrderModel value)
    {
        SelectedOrderStatus = value.Status;
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
    private async Task CancelOrder()
    {
        if (Order == null) return;

        IsBusy = true;

        try
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Отменение заказа",
                $"Вы хотите отменить заказ на {Order.Brand} {Order.Model}?",
                "Да",
                "Отмена"
            );

            if (confirm)
            {
                // Логика отмены заказа

                OrderDTO orderDTO = _orderService.GetOrder(Order.Id);
                orderDTO.StatusId = 4; // Отменен
                _orderService.UpdateOrder(orderDTO);

                CarDTO carDTO = _carService.GetCar((int)orderDTO.CarId);
                carDTO.AvailabilityId = 1; // В наличии
                _carService.UpdateCar(carDTO);

                await Shell.Current.DisplayAlert("Успех!", $"Заказ на {Order.Brand} {Order.Model} успешно отменен", "OK");

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