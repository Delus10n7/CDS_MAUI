using Microsoft.Maui.Controls.Shapes;

namespace CDS_MAUI;

public partial class OrdersPage : ContentPage
{
    private bool _filtersVisible = false;

    public OrdersPage()
	{
		InitializeComponent();

        InitializeOrderBrandPicker();
        LoadModelsForBrand("");
        InitializeOtherPickers();
        InitializeTestOrders();
    }

    private void InitializeOrderBrandPicker()
    {
        var brands = new List<string> { "Любой", "Audi", "BMW", "Mercedes", "Toyota", "Honda" };
        foreach (var brand in brands)
        {
            OrderBrandPicker.Items.Add(brand);
        }
        OrderBrandPicker.SelectedIndex = 0;
    }

    private void InitializeOtherPickers()
    {
        ManagerPicker.SelectedIndex = 0;
        ClientEntry.Text = string.Empty;
        OrderPriceFrom.Text = string.Empty;
        OrderPriceTo.Text = string.Empty;
    }

    private void OnOrderBrandPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        if (picker.SelectedIndex == -1 || picker.SelectedIndex == 0)
        {
            LoadModelsForBrand("");
            return;
        }

        var selectedBrand = picker.SelectedItem.ToString();
        LoadModelsForBrand(selectedBrand);
    }

    private void LoadModelsForBrand(string brand)
    {
        // Примеры моделей для каждой марки
        var models = brand switch
        {
            "Audi" => new List<string> { "Любая", "A3", "A4", "A5", "A6", "A7", "A8", "Q3", "Q5", "Q7" },
            "BMW" => new List<string> { "Любая", "1 Series", "3 Series", "5 Series", "7 Series", "X1", "X3", "X5" },
            "Mercedes" => new List<string> { "Любая", "A-Class", "C-Class", "E-Class", "S-Class", "GLA", "GLC", "GLE" },
            "Toyota" => new List<string> { "Любая", "Camry", "Corolla", "RAV4", "Highlander", "Land Cruiser", "Prius" },
            "Honda" => new List<string> { "Любая", "Accord", "Civic", "CR-V", "Pilot", "HR-V", "Odyssey" },
            _ => new List<string> { "Любая" }
        };

        if (models.Count > 1) OrderModelPicker.IsEnabled = true;
        else OrderModelPicker.IsEnabled = false;
        OrderModelPicker.Items.Clear();

        foreach (var model in models)
        {
            OrderModelPicker.Items.Add(model);
        }

        // Сбрасываем выбор модели при смене марки
        OrderModelPicker.SelectedIndex = 0;
    }

    private async void OnOrderFilterButtonClicked(object sender, EventArgs e)
    {
        _filtersVisible = !_filtersVisible;

        // Обновляем текст кнопки
        OrderFilterButton.Text = _filtersVisible ? "Скрыть" : "Фильтры";

        if (_filtersVisible)
        {
            // Показываем панель с анимацией
            OrderFilterPanel.IsVisible = true;

            await Task.WhenAll(
                OrderFilterPanel.FadeTo(1, 300, Easing.CubicOut),
                OrderFilterPanel.TranslateTo(0, 0, 300, Easing.CubicOut)
            );
        }
        else
        {
            // Скрываем панель с анимацией
            await Task.WhenAll(
                OrderFilterPanel.FadeTo(0, 200, Easing.CubicIn),
                OrderFilterPanel.TranslateTo(0, -20, 200, Easing.CubicIn)
            );

            OrderFilterPanel.IsVisible = false;
        }
    }

    private void OnOrderResetFiltersClicked(object sender, EventArgs e)
    {
        OrderBrandPicker.SelectedIndex = 0;
        OrderModelPicker.SelectedIndex = 0;
        ManagerPicker.SelectedIndex = 0;
        ClientEntry.Text = string.Empty;
        OrderPriceFrom.Text = string.Empty;
        OrderPriceTo.Text = string.Empty;
    }

    private void InitializeTestOrders()
    {
        var testOrders = new List<Order>()
        {
            new Order
            {
                Brand = "Honda",
                Model = "Accord",
                VIN = "JHMCM56557C404453",
                CustomerName = "Лебедев С.В.",
                ManagerName = "Анисимов И.Н.",
                Price = (decimal)800000.00,
                Date = DateTime.Now
            },
            new Order
            {
                Brand = "BMW",
                Model = "7 Series",
                VIN = "WBA7E0C59GGM56193",
                CustomerName = "Ивин Г.А.",
                ManagerName = "Крылов М.Ю.",
                Price = (decimal)2150000.00,
                Date = DateTime.Now
            }
        };

        UpdateOrdersDisplay(testOrders);
    }

    private void UpdateOrdersDisplay(List<Order> orders)
    {
        OrdersContainer.Children.Clear();

        foreach (var order in orders)
        {
            var card = CreateOrderCard(order);
            OrdersContainer.Children.Add(card);
        }
    }

    private Border CreateOrderCard(Order order)
    {
        // Создаем основную карточку
        var mainCard = new Border
        {
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
            Content = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(180) }, // Изображение
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, // Информация
                new ColumnDefinition { Width = new GridLength(150) }  // Кнопка
            },
                Padding = new Thickness(15)
            }
        };

        // Изображение (левая часть)
        var imagePlaceholder = CreateImagePlaceholder(150, 120);
        Grid.SetColumn(imagePlaceholder, 0);

        // Краткая информация (центральная часть)
        var infoLayout = new VerticalStackLayout
        {
            Spacing = 4,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = $"{order.Brand} {order.Model}",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                },

                new Label
                {
                    Text = $"{order.VIN}",
                    FontSize = 14
                },

                new Label
                {
                    Text = $"{order.Date}",
                    FontSize = 12
                },

                new Label
                {
                    Text = $"{order.Price:N0} руб.",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                }
            }
        };
        Grid.SetColumn(infoLayout, 1);

        // Кнопка "Подробнее" (правая часть)
        var detailsButton = new Button
        {
            Text = "Подробнее",
            CornerRadius = 8,
            HeightRequest = 35,
            WidthRequest = 150,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.End,
            Command = new Command(() => ShowOrderDetails(order))
        };
        Grid.SetColumn(detailsButton, 2);

        // Добавляем элементы в Grid
        var grid = (Grid)mainCard.Content;
        grid.Children.Add(imagePlaceholder);
        grid.Children.Add(infoLayout);
        grid.Children.Add(detailsButton);

        return mainCard;
    }

    private Border CreateImagePlaceholder(int width, int height)
    {
        return new Border
        {
            WidthRequest = width,
            HeightRequest = height,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image
                    {
                        Source = "dotnet_bot.scale-100.png",
                        HeightRequest = height * 0.6,
                        WidthRequest = width * 0.6,
                        Aspect = Aspect.AspectFit
                    },
                    new Label
                    {
                        Text = "Фото авто",
                        FontSize = 10,
                        HorizontalOptions = LayoutOptions.Center
                    }
                }
            }
        };
    }

    private async void ShowOrderDetails(Order order)
    {
        // Создаем подробную карточку
        var detailCard = new Border
        {
            Margin = new Thickness(20, 50),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }, // Кнопка закрытия
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }, // Заголовок
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },  // Контент
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }   // Цена и кнопка
                },
                Padding = new Thickness(0)
            }
        };

        // Кнопка закрытия (в правом верхнем углу)
        var closeButton = new Button
        {
            Text = "X",
            FontSize = 24,
            WidthRequest = 40,
            HeightRequest = 40,
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 10, 10, 0),
            Command = new Command(async () => await Navigation.PopModalAsync())
        };
        Grid.SetRow(closeButton, 0);
        Grid.SetColumn(closeButton, 0);

        // Заголовок (марка и модель)
        var titleLabel = new Label
        {
            Text = $"{order.Brand} {order.Model}",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10),
        };
        Grid.SetRow(titleLabel, 1);

        // Основной контент (изображение + детали)
        var contentGrid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, // Изображение
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }  // Информация
        },
            Margin = new Thickness(20, 0)
        };

        // Большое изображение
        var largeImage = CreateImagePlaceholder(420, 380);
        Grid.SetColumn(largeImage, 0);

        // Детальная информация
        var detailsLayout = new VerticalStackLayout
        {
            Spacing = 12,
            Padding = new Thickness(20, 0),
            Children =
            {
                CreateDetailRow("VIN:", order.VIN),
                CreateDetailRow("Имя клиента:", order.CustomerName),
                CreateDetailRow("Имя менеджера:", order.ManagerName),
                CreateDetailRow("Дата заказа:", order.Date.ToString()),
            }
        };
        Grid.SetColumn(detailsLayout, 1);

        contentGrid.Children.Add(largeImage);
        contentGrid.Children.Add(detailsLayout);
        Grid.SetRow(contentGrid, 2);

        // Блок с ценой и кнопкой "Изменить"
        var priceLayout = new VerticalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 30),
            Children =
        {
            new Label
            {
                Text = $"{order.Price:N0} руб.",
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
            },
            new Button
            {
                Text = "Изменить заказ",
                CornerRadius = 10,
                HeightRequest = 50,
                WidthRequest = 220,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            }
        }
        };
        Grid.SetRow(priceLayout, 3);

        // Добавляем все элементы в основной Grid
        var grid = (Grid)detailCard.Content;
        grid.Children.Add(closeButton);
        grid.Children.Add(titleLabel);
        grid.Children.Add(contentGrid);
        grid.Children.Add(priceLayout);

        // Показываем как модальное окно
        var detailPage = new ContentPage
        {
            Content = new ScrollView
            {
                Content = detailCard
            },
            BackgroundColor = Color.FromArgb("#80000000") // Полупрозрачный фон
        };

        await Navigation.PushModalAsync(detailPage);
    }

    private Grid CreateDetailRow(string title, string value)
    {
        // Создаем Label для значения
        var valueLabel = new Label
        {
            Text = value,
            FontSize = 14
        };

        // Создаем Grid
        var grid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
        },
            Children =
            {
                new Label
                {
                    Text = title,
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold
                },
                valueLabel
            }
        };
        Grid.SetColumn(valueLabel, 1);

        return grid;
    }

    public class Order
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}