using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class CartPageViewModel : ObservableObject, INavigationAware
    {
        private MainWindowViewModel? _mainWindowViewModel;
        private RestaurantDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<FoodContain> _cartItems;

        [ObservableProperty]
        private Visibility _progressRingVisibility = Visibility.Hidden;
        public void OnNavigatedFrom()
        {
            _mainWindowViewModel.IsCartFilled = false;
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        private async void InitializeViewModel()
        {
            try
            {
                ProgressRingVisibility = Visibility.Visible;

                _dbContext = await Task.Run(() => new RestaurantDbContext());
                _mainWindowViewModel = App.GetService<MainWindowViewModel>();

                CartItems = await Task.Run(() => new ObservableCollection<FoodContain>(_dbContext.FoodContain.Include(x => x.Food).Include(x => x.Cart).Where(x => x.Cart.UserID == _mainWindowViewModel.UserID)));

                ProgressRingVisibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
    }
}
