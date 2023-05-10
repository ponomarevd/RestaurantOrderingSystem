using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private int _productsCount;

        [ObservableProperty]
        private int _productsSummary;

        [ObservableProperty]
        private string _countText;

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
                ProductsCount = 0;
                ProductsSummary = 0;

                ProgressRingVisibility = Visibility.Visible;

                _dbContext = await Task.Run(() => new RestaurantDbContext());
                _mainWindowViewModel = App.GetService<MainWindowViewModel>();
                
                _mainWindowViewModel.IsCartFilled = true;
                ProductsCount = _mainWindowViewModel.BadgeValue;

                CartItems = await Task.Run(() => new ObservableCollection<FoodContain>(_dbContext.FoodContain.Include(x => x.Food).Include(x => x.Cart).Where(x => x.Cart.UserID == _mainWindowViewModel.UserID)));

                foreach(FoodContain item in CartItems)
                    ProductsSummary += item.Count * item.Food.FoodPrice;

                GetNumEnding(ProductsCount);

                ProgressRingVisibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }          
        }

        [RelayCommand]
        private void DecrementCount()
        {

        }

        [RelayCommand]
        private void IncrementCount()
        {

        }

        public void GetNumEnding(int iNumber)
        {
            iNumber = iNumber % 100;
            if (iNumber >= 11 && iNumber <= 19)
                CountText = "товаров";
            else
            {
                var i = iNumber % 10;
                switch (i)
                {
                    case (1):
                        CountText = "товар";
                        break;
                    case 2:
                    case 3:
                    case 4:
                        CountText = "товара";
                        break;
                    default:
                        CountText = "товаров";
                        break;
                }
            }
        }
    }
}
