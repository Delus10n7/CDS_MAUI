using Microsoft.Maui.Controls.Shapes;

namespace CDS_MAUI;

public partial class CarsPage : ContentPage
{
    private bool _filtersVisible = false;

    public CarsPage()
	{
		InitializeComponent();
        Application.Current.UserAppTheme = AppTheme.Dark;

        InitializeBrandPicker();
        LoadModelsForBrand("");
        InitializeOtherPickers();
        InitializeTestCars();
    }

    private void InitializeBrandPicker()
    {
        var brands = new List<string> { "Любой", "Audi", "BMW", "Mercedes", "Toyota", "Honda" };
        foreach (var brand in brands)
        {
            BrandPicker.Items.Add(brand);
        }
        BrandPicker.SelectedIndex = 0;
    }

    private void InitializeOtherPickers()
    {
        TransmissionPicker.SelectedIndex = 0;
        DriveTypePicker.SelectedIndex = 0;
        BodyTypePicker.SelectedIndex = 0;
        EngineTypePicker.SelectedIndex = 0;
    }

    private void OnBrandPickerSelectedIndexChanged(object sender, EventArgs e)
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

        if (models.Count > 1) ModelPicker.IsEnabled = true;
        else ModelPicker.IsEnabled = false;
        ModelPicker.Items.Clear();

        foreach (var model in models)
        {
            ModelPicker.Items.Add(model);
        }

        // Сбрасываем выбор модели при смене марки
        ModelPicker.SelectedIndex = 0;
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        _filtersVisible = !_filtersVisible;

        // Обновляем текст кнопки
        FilterButton.Text = _filtersVisible ? "Скрыть" : "Фильтры";

        if (_filtersVisible)
        {
            // Показываем панель с анимацией
            FilterPanel.IsVisible = true;

            await Task.WhenAll(
                FilterPanel.FadeTo(1, 300, Easing.CubicOut),
                FilterPanel.TranslateTo(0, 0, 300, Easing.CubicOut)
            );
        }
        else
        {
            // Скрываем панель с анимацией
            await Task.WhenAll(
                FilterPanel.FadeTo(0, 200, Easing.CubicIn),
                FilterPanel.TranslateTo(0, -20, 200, Easing.CubicIn)
            );

            FilterPanel.IsVisible = false;
        }
    }

    private void OnResetFiltersClicked(object sender, EventArgs e)
    {
        // Сброс всех фильтров
        BrandPicker.SelectedIndex = 0;
        ModelPicker.SelectedIndex = 0;
        TransmissionPicker.SelectedIndex = 0;
        DriveTypePicker.SelectedIndex = 0;
        EngineTypePicker.SelectedIndex = 0;
        BodyTypePicker.SelectedIndex = 0;
        EngineVolumeFrom.Text = string.Empty;
        EngineVolumeTo.Text = string.Empty;
        EnginePowerFrom.Text = string.Empty;
        EnginePowerTo.Text = string.Empty;
    }

    private void InitializeTestCars()
    {
        // Создаем тестовые данные автомобилей
        var testCars = new List<Car>
        {
            new Car
            {
                Brand = "Audi",
                Model = "A4",
                Year = 2022,
                EngineVolume = 2.0,
                Power = 190,
                Price = 3_200_000,
                Transmission = "Автоматическая",
                DriveType = "Полный",
                EngineType = "Бензин",
                BodyType = "Седан",
                Color = "Черный",
                Mileage = 15000
            },
            new Car
            {
                Brand = "BMW",
                Model = "3 Series",
                Year = 2023,
                EngineVolume = 2.0,
                Power = 184,
                Price = 3_500_000,
                Transmission = "Автоматическая",
                DriveType = "Задний",
                EngineType = "Бензин",
                BodyType = "Седан",
                Color = "Белый",
                Mileage = 45000
            },
            new Car
            {
                Brand = "Mercedes",
                Model = "C-Class",
                Year = 2021,
                EngineVolume = 1.5,
                Power = 170,
                Price = 3_800_000,
                Transmission = "Автоматическая",
                DriveType = "Задний",
                EngineType = "Бензин",
                BodyType = "Седан",
                Color = "Белый",
                Mileage = 105000
            }
        };

        UpdateCarsDisplay(testCars);
    }

    private void UpdateCarsDisplay(List<Car> cars)
    {
        CarsContainer.Children.Clear();

        foreach (var car in cars)
        {
            var card = CreateCarCard(car);
            CarsContainer.Children.Add(card);
        }
    }

    private Border CreateCarCard(Car car)
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
                    Text = $"{car.Brand} {car.Model}",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                },

                new Label
                {
                    Text = $"{car.Year} год | {car.EngineVolume}л | {car.Power} л.с.",
                    FontSize = 14
                },

                new Label
                {
                    Text = $"{car.Transmission} | {car.DriveType}",
                    FontSize = 12
                },

                new Label
                {
                    Text = $"{car.Price:N0} руб.",
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
            Command = new Command(() => ShowCarDetails(car))
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

    private async void ShowCarDetails(Car car)
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
            Text = $"{car.Brand} {car.Model}",
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
                CreateDetailRow("Год выпуска:", car.Year.ToString()),
                CreateDetailRow("Объем двигателя:", $"{car.EngineVolume} л"),
                CreateDetailRow("Мощность:", $"{car.Power} л.с."),
                CreateDetailRow("Коробка передач:", car.Transmission),
                CreateDetailRow("Привод:", car.DriveType),
                CreateDetailRow("Тип двигателя:", car.EngineType),
                CreateDetailRow("Тип кузова:", car.BodyType),
                CreateDetailRow("Цвет:", car.Color ?? "Черный"),
                CreateDetailRow("Пробег:", $"{car.Mileage:N0} км"),
            }
        };
        Grid.SetColumn(detailsLayout, 1);

        contentGrid.Children.Add(largeImage);
        contentGrid.Children.Add(detailsLayout);
        Grid.SetRow(contentGrid, 2);

        // Блок с ценой и кнопкой "Оформить"
        var priceLayout = new VerticalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 30),
            Children =
        {
            new Label
            {
                Text = $"{car.Price:N0} руб.",
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
            },
            new Button
            {
                Text = "Оформить заказ",
                CornerRadius = 10,
                HeightRequest = 50,
                WidthRequest = 220,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                Command = new Command(() => ProcessOrder(car))
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

    private async void ProcessOrder(Car car)
    {
        bool result = await DisplayAlert(
            "Оформление заказа",
            $"Вы уверены, что хотите оформить заказ на {car.Brand} {car.Model} за {car.Price:N0} руб.?",
            "Да, оформить",
            "Отмена"
        );

        if (result)
        {
            await DisplayAlert("Успех", "Заказ успешно оформлен!", "OK");
            await Navigation.PopModalAsync(); // Закрываем подробную карточку
        }
    }

    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double EngineVolume { get; set; }
        public int Power { get; set; }
        public decimal Price { get; set; }
        public string Transmission { get; set; }
        public string DriveType { get; set; }
        public string EngineType { get; set; }
        public string BodyType { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
    }
}