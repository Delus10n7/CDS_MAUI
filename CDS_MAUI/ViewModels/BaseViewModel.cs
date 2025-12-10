using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _hasError;

        public bool IsNotBusy => !IsBusy;

        [RelayCommand]
        protected virtual async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        protected void ClearErrors()
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }
    }
}
