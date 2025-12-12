using CDS_Interfaces.DTO;
using CDS_Interfaces.Service;
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
        private ObservableCollection<CarModel> _filteredCars = new();

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

        // === СЕРВИСЫ ===
        private ICarService _carService;
        private IBrandService _brandService;

        // === Страницы ===
        private const int PageSize = 20; // По 20 авто за раз
        private int _currentPage = 0;
        private bool _isLoadingMore = false;

        public CarsViewModel(ICarService carService, IBrandService brandService)
        {
            _carService = carService;
            _brandService = brandService;

            Title = "Автомобили";
            Initialize();
        }

        private void Initialize()
        {
            // Инициализация списков
            InitializeBrands();
            InitializeFilterOptions();

            // Загрузка машин
            LoadCars();
        }

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Любой");

            List<BrandDTO> brandDTOs = _brandService.GetAllBrands();

            foreach (var b in brandDTOs)
            {
                Brands.Add(b.BrandName);
            }
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

        private void LoadCars()
        {
            Cars.Clear();

            List<CarDTO> carDTOs = _carService.GetAllCars();

            //foreach (var car in carDTOs)
            //{
            //    Cars.Add(new CarModel(car));
            //}

            for (int i = 0; i < 20; i++)
            {
                Cars.Add(new CarModel(carDTOs[i]));
            }

            UpdateModelsForBrand("Любой");

            // Инициализируем отфильтрованные данные
            FilterCars();
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

            // Обновляем отфильтрованную коллекцию
            FilteredCars.Clear();
            foreach (var car in filtered.ToList())
            {
                FilteredCars.Add(car);
            }

            // Обновляем модели для выбранного бренда
            UpdateModelsForBrand(SelectedBrand);
        }
    }
}