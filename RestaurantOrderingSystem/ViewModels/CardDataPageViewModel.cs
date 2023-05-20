using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestaurantOrderingSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class CardDataPageViewModel : ObservableObject, INavigationAware
    {
        private MainWindowViewModel? _mainWindowViewModel;
        private CartPageViewModel? _cartPageViewModel;
        private INavigationService? navService;
        private RestaurantDbContext _dbContext;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _cardNumber;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _month;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _year;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _cvv;

        private async void InitializeViewModel()
        {
            _mainWindowViewModel = App.GetService<MainWindowViewModel>();
            _cartPageViewModel = App.GetService<CartPageViewModel>();
            _dbContext = await Task.Run(() => new RestaurantDbContext());
        }

        [RelayCommand(CanExecute = nameof(CheckFields))]
        private async void Confirm(PasswordBox passwordBox)
        {
            try
            {
                await Task.Run(() => _dbContext.FoodContain.RemoveRange(_cartPageViewModel.CartItems));
                await _dbContext.SaveChangesAsync();

                _mainWindowViewModel.BadgeValue = 0;

                CardNumber = string.Empty;
                Month = string.Empty;
                Year = string.Empty;

                navService = App.GetService<INavigationService>();
                navService.Navigate(typeof(Views.Pages.CartPage));

                _cartPageViewModel.SnackBar.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
        private bool CheckFields()
        {
            return !(CardNumber == null || CardNumber.Trim() == string.Empty ||
                   Cvv == null || Cvv.Trim() == string.Empty ||
                   Year == null || Year.Trim() == string.Empty ||
                   Month == null || Month.Trim() == string.Empty ||
                   Cvv.Length < 3 || Year.Length < 2 || Month.Length < 2 || CardNumber.Contains('_'));
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
