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

        [ObservableProperty]
        private ObservableCollection<string> _bodyTypes = new();

        [ObservableProperty]
        private ObservableCollection<string> _engineTypes = new();

        [ObservableProperty]
        private ObservableCollection<string> _transmissionTypes = new();

        [ObservableProperty]
        private ObservableCollection<string> _driveTypes = new();

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

        // === ДАННЫЕ ===
        private List<CarModel> _allCars = new();
        private const int _pageSize = 20;
        private int _pageCount = 1;
        private int _currentPage = 1;

        [ObservableProperty]
        private string _curPage = "1";

        [ObservableProperty]
        private bool _canGoNextPage = true;

        [ObservableProperty]
        private bool _canGoPrevPage = false;

        [ObservableProperty]
        private bool _hasFooterPageButtons = false;

        // === СЕРВИСЫ ===
        private ICarService _carService;
        private ICarConfigurationService _carConfigService;

        public CarsViewModel(ICarService carService, ICarConfigurationService carConfigService)
        {
            _carService = carService;
            _carConfigService = carConfigService;

            Title = "Автомобили";
            Initialize();
        }

        private void Initialize()
        {
            // Инициализация списков
            InitializeBrands();
            InitializeCarConfigurations();
            InitializeFilterOptions();

            // Загрузка машин
            LoadAllCars();
            LoadCurrentPageCars();
        }

        private void InitializeBrands()
        {
            Brands.Clear();
            Brands.Add("Любой");

            List<BrandDTO> brandDTOs = _carConfigService.GetAllBrands();

            foreach (var b in brandDTOs)
            {
                Brands.Add(b.BrandName);
            }
        }

        private void InitializeCarConfigurations()
        {
            BodyTypes.Clear();
            BodyTypes.Add("Любой");
            EngineTypes.Clear();
            EngineTypes.Add("Любой");
            TransmissionTypes.Clear();
            TransmissionTypes.Add("Любая");
            DriveTypes.Clear();
            DriveTypes.Add("Любой");

            List<BodyTypeDTO> bodyTypeDTOs = _carConfigService.GetAllBodyTypes();
            List<EngineTypeDTO> engineTypeDTOs = _carConfigService.GetAllEngineTypes();
            List<TransmissionTypeDTO> transmissionTypeDTOs = _carConfigService.GetAllTransmissionTypes();
            List<DriveTypeDTO> driveTypeDTOs = _carConfigService.GetAllDriveTypes();

            foreach (var b in bodyTypeDTOs)
            {
                BodyTypes.Add(b.BodyName);
            }
            foreach (var e in engineTypeDTOs)
            {
                EngineTypes.Add(e.EngineName);
            }
            foreach (var t in transmissionTypeDTOs)
            {
                TransmissionTypes.Add(t.TransmissionName);
            }
            foreach (var d in driveTypeDTOs)
            {
                DriveTypes.Add(d.DriveName);
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
            LoadCurrentPageCars();
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
            ApplyFilters();
        }

        [RelayCommand]
        private void NextPage()
        {
            if (_currentPage < _pageCount) _currentPage++;

            CurPage = _currentPage.ToString();

            if (_currentPage > 1) CanGoPrevPage = true;
            else CanGoPrevPage = false;
            if (_currentPage < _pageCount) CanGoNextPage = true;
            else CanGoNextPage = false;

            LoadCurrentPageCars();
        }

        [RelayCommand]
        private void PrevPage()
        {
            if (_currentPage > 1) _currentPage--;

            CurPage = _currentPage.ToString();

            if (_currentPage > 1) CanGoPrevPage = true;
            else CanGoPrevPage = false;
            if (_currentPage < _pageCount) CanGoNextPage = true;
            else CanGoNextPage = false;

            LoadCurrentPageCars();
        }

        [RelayCommand]
        private void FirstPage()
        {
            _currentPage = 1;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;

            LoadCurrentPageCars();
        }

        [RelayCommand]
        private void LastPage()
        {
            _currentPage = _pageCount;

            CurPage = _currentPage.ToString();

            CanGoPrevPage = _currentPage > 1 ? true : false;
            CanGoNextPage = false;

            LoadCurrentPageCars();
        }

        // === ОБРАБОТЧИКИ ИЗМЕНЕНИЙ ===

        partial void OnSelectedBrandChanged(string value)
        {
            UpdateModelsForBrand(value);
            SelectedModel = Models[0];
        }

        private void UpdateModelsForBrand(string brand)
        {
            Models.Clear();
            Models.Add("Любая");

            if (brand == "Любой" || string.IsNullOrEmpty(brand))
                return;

            // Получаем уникальные модели для выбранной марки
            var brandModels = _allCars
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

        private void LoadAllCars()
        {
            Cars.Clear();

            List<CarDTO> carDTOs = _carService.GetAllCars();

            foreach (var car in carDTOs)
            {
                _allCars.Add(new CarModel(car));
            }

            UpdateModelsForBrand("Любой");

            // Инициализируем отфильтрованные данные
            FilterCars();
        }

        private void FilterCars()
        {
            var filtered = _allCars.ToList();

            // Фильтрация по марке
            if (SelectedBrand != "Любой" && !string.IsNullOrEmpty(SelectedBrand))
                filtered = filtered.Where(c => c.Brand == SelectedBrand).ToList();

            // Фильтрация по модели
            if (SelectedModel != "Любая" && !string.IsNullOrEmpty(SelectedModel))
                filtered = filtered.Where(c => c.Model == SelectedModel).ToList();

            // Фильтрация по типу кузова
            if (SelectedBodyType != "Любой" && !string.IsNullOrEmpty(SelectedBodyType))
                filtered = filtered.Where(c => c.BodyType == SelectedBodyType).ToList();

            // Фильтрация по типу двигателя
            if (SelectedEngineType != "Любой" && !string.IsNullOrEmpty(SelectedEngineType))
                filtered = filtered.Where(c => c.EngineType == SelectedEngineType).ToList();

            // Фильтрация по типу трансмиссии
            if (SelectedTransmission != "Любая" && !string.IsNullOrEmpty(SelectedTransmission))
                filtered = filtered.Where(c => c.Transmission == SelectedTransmission).ToList();

            // Фильтрация по типу привода
            if (SelectedDriveType != "Любой" && !string.IsNullOrEmpty(SelectedDriveType))
                filtered = filtered.Where(c => c.DriveType == SelectedDriveType).ToList();

            // Фильтрация по объему двигателя
            if (!string.IsNullOrEmpty(EngineVolumeFrom))
                filtered = filtered.Where(c => c.EngineVolume >= Decimal.Parse(EngineVolumeFrom)).ToList();

            if (!string.IsNullOrEmpty(EngineVolumeTo))
                filtered = filtered.Where(c => c.EngineVolume <= Decimal.Parse(EngineVolumeTo)).ToList();

            // Фильтрация по мощности двигателя
            if (!string.IsNullOrEmpty(EnginePowerFrom))
                filtered = filtered.Where(c => c.Power >= int.Parse(EnginePowerFrom)).ToList();

            if (!string.IsNullOrEmpty(EnginePowerTo))
                filtered = filtered.Where(c => c.Power <= int.Parse(EnginePowerTo)).ToList();

            // Поиск по тексту
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(c =>
                    c.Brand.ToLower().Contains(searchLower) ||
                    c.Model.ToLower().Contains(searchLower) ||
                    c.VIN.ToLower().Contains(searchLower))
                    .ToList();
            }

            // Обновляем отфильтрованную коллекцию
            FilteredCars.Clear();
            foreach (var car in filtered)
            {
                FilteredCars.Add(car);
            }

            _pageCount = (int)Math.Ceiling((decimal)FilteredCars.Count / _pageSize);
            _currentPage = 1;
            CanGoPrevPage = false;
            CanGoNextPage = _currentPage < _pageCount ? true : false;
        }

        private void LoadCurrentPageCars()
        {
            var currIndex = _currentPage - 1;

            var startIndex = currIndex * 20;
            var endIndex = (currIndex * 20 + 19) > FilteredCars.Count() ? FilteredCars.Count() - 1 : (currIndex * 20 + 19);

            Cars.Clear();
            for (int i  = startIndex; i < endIndex; i++)
            {
                Cars.Add(FilteredCars[i]);
            }

            if (Cars.Count >= 5) HasFooterPageButtons = true;
            else HasFooterPageButtons = false;
        }
    }
}