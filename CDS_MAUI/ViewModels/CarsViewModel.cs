using CDS_MAUI.Models;
using CDS_MAUI.Views.CarDetailsModal;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CDS_MAUI.Views.CarsPage;

namespace CDS_MAUI.ViewModels
{
    public partial class CarsViewModel : BaseViewModel
    {
        // === КОЛЛЕКЦИИ ДАННЫХ ===
        [ObservableProperty]
        private ObservableCollection<CarModel> _cars = new();

        [ObservableProperty]
        private ObservableCollection<string> _brands = new();

        [ObservableProperty]
        private ObservableCollection<string> _models = new();

        // === ФИЛЬТРЫ ===
        [ObservableProperty]
        private string _selectedBrand = "Любой";

        [ObservableProperty]
        private string _selectedModel = "Любая";

        [ObservableProperty]
        private string _selectedTransmission = "Любая";

        [ObservableProperty]
        private string _selectedDriveType = "Любой";

        [ObservableProperty]
        private string _selectedBodyType = "Любой";

        [ObservableProperty]
        private string _selectedEngineType = "Любой";

        // === ДИАПАЗОНЫ ФИЛЬТРОВ ===
        [ObservableProperty]
        private string _engineVolumeFrom = "";

        [ObservableProperty]
        private string _engineVolumeTo = "";

        [ObservableProperty]
        private string _enginePowerFrom = "";

        [ObservableProperty]
        private string _enginePowerTo = "";

        // === ПОИСК И СОСТОЯНИЕ ===
        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private bool _isFilterPanelVisible = false;

        [ObservableProperty]
        private CarModel _selectedCar;

        public CarsViewModel()
        {
            Title = "Автомобили";
            Initialize();
        }

        private void Initialize()
        {
            // Инициализация списков
            InitializeBrands();
            InitializeFilterOptions();

            // Загрузка тестовых данных
            LoadTestCars();
        }

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Любой");
            Brands.Add("Audi");
            Brands.Add("BMW");
            Brands.Add("Mercedes");
            Brands.Add("Toyota");
            Brands.Add("Honda");
        }

        private void InitializeFilterOptions()
        {
            Models.Clear();
            Models.Add("Любая");
        }

        // === КОМАНДЫ ===

        [RelayCommand]
        private void ToggleFilters()
        {
            IsFilterPanelVisible = !IsFilterPanelVisible;
        }

        [RelayCommand]
        private void ResetFilters()
        {
            SelectedBrand = "Любой";
            SelectedModel = "Любая";
            SelectedTransmission = "Любая";
            SelectedDriveType = "Любой";
            SelectedBodyType = "Любой";
            SelectedEngineType = "Любой";
            EngineVolumeFrom = "";
            EngineVolumeTo = "";
            EnginePowerFrom = "";
            EnginePowerTo = "";
            SearchText = "";

            UpdateModelsForBrand("Любой");
            FilterCars();
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            FilterCars();
            IsFilterPanelVisible = false;
        }

        [RelayCommand]
        private async Task ShowCarDetails(CarModel car)
        {
            if (car == null) return;

            SelectedCar = car;

            // Создаем словарь параметров
            var parameters = new Dictionary<string, object>
        {
            { "Car", car }
        };

            // Открываем модальное окно с параметрами
            await Shell.Current.GoToAsync(nameof(CarDetailsModal), true, parameters);
        }

        [RelayCommand]
        private async Task CreateOrder(CarModel car)
        {
            if (car == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Оформление заказа",
                $"Оформить заказ на {car.Brand} {car.Model} за {car.Price:N0} руб.?",
                "Да, оформить",
                "Отмена");

            if (confirm)
            {
                IsBusy = true;
                try
                {
                    // Логика создания заказа
                    await Task.Delay(500);

                    // Показываем уведомление
                    await Shell.Current.DisplayAlert("Успех", "Заказ оформлен!", "OK");

                    // Закрываем модальное окно
                    await Shell.Current.GoToAsync("..");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        [RelayCommand]
        private void Search()
        {
            FilterCars();
        }

        // === ОБРАБОТЧИКИ ИЗМЕНЕНИЙ ===

        partial void OnSelectedBrandChanged(string value)
        {
            UpdateModelsForBrand(value);
        }

        private void UpdateModelsForBrand(string brand)
        {
            Models.Clear();
            Models.Add("Любая");

            if (brand == "Любой" || string.IsNullOrEmpty(brand))
                return;

            // Получаем уникальные модели для выбранной марки
            var brandModels = Cars
                .Where(c => c.Brand == brand)
                .Select(c => c.Model)
                .Distinct()
                .OrderBy(m => m);

            foreach (var model in brandModels)
            {
                Models.Add(model);
            }
        }

        // === ЗАГРУЗКА ДАННЫХ ===

        private void LoadTestCars()
        {
            Cars.Clear();

            var testCars = new List<CarModel>
        {
            new CarModel
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
            new CarModel
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
            new CarModel
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

            foreach (var car in testCars)
            {
                Cars.Add(car);
            }

            UpdateModelsForBrand("Любой");
        }

        private void FilterCars()
        {
            var filtered = Cars.AsEnumerable();

            // Фильтрация по марке
            if (SelectedBrand != "Любой")
                filtered = filtered.Where(c => c.Brand == SelectedBrand);

            // Фильтрация по модели
            if (SelectedModel != "Любая")
                filtered = filtered.Where(c => c.Model == SelectedModel);

            // Поиск по тексту
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(c =>
                    c.Brand.ToLower().Contains(searchLower) ||
                    c.Model.ToLower().Contains(searchLower) ||
                    c.Color.ToLower().Contains(searchLower));
            }

            // Обновляем отображение
            var filteredList = filtered.ToList();
        }
    }
}