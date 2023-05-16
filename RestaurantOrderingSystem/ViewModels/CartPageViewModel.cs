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
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class CartPageViewModel : ObservableObject, INavigationAware
    {
        private MainWindowViewModel? _mainWindowViewModel;
        private RestaurantDbContext _dbContext;
        private INavigationService? navService;

        [ObservableProperty]
        private ObservableCollection<FoodContain> _cartItems;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InterfaceIsEnabled))]
        private Visibility _progressRingVisibility = Visibility.Hidden;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EmptyCartVisibility))]
        private Visibility _interfaceVisibility = Visibility.Visible;

        [ObservableProperty]
        private string _countText;

        [ObservableProperty]
        private int _productsCount;

        [ObservableProperty]
        private int _productsSummary;

        [ObservableProperty]
        private bool _buttonsCountIsEnabled = true;

        [ObservableProperty]
        private bool _deleteBtnIsEnabled = true;
        public bool InterfaceIsEnabled => ProgressRingVisibility == Visibility.Hidden ? true : false;
        public Visibility EmptyCartVisibility => InterfaceVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

        public void OnNavigatedFrom()
        {
            _mainWindowViewModel.IsCartFilled = false;
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
            _mainWindowViewModel.IsCartFilled = true;
        }

        private async void InitializeViewModel()
        {
            try
            {
                _mainWindowViewModel = App.GetService<MainWindowViewModel>();

                if(_mainWindowViewModel.BadgeValue == 0)
                    InterfaceVisibility = Visibility.Hidden;
                else
                {
                    InterfaceVisibility = Visibility.Visible;
                    ProductsCount = 0;
                    ProductsSummary = 0;

                    ProgressRingVisibility = Visibility.Visible;

                    _dbContext = await Task.Run(() => new RestaurantDbContext());

                    _mainWindowViewModel.IsCartFilled = true;
                    ProductsCount = _mainWindowViewModel.BadgeValue;

                    CartItems = await Task.Run(() => new ObservableCollection<FoodContain>(_dbContext.FoodContain.Include(x => x.Food).Include(x => x.Cart).Where(x => x.Cart.UserID == _mainWindowViewModel.UserID)));

                    foreach (FoodContain item in CartItems)
                        ProductsSummary += item.Count * item.Food.FoodPrice;

                    GetNumEnding(ProductsCount);

                    ProgressRingVisibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }          
        }

        [RelayCommand]
        private void GoToMenu()
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.MenuPage));
        }

        [RelayCommand]
        private void Checkout()
        {
            MessageBox.Show("Заказ оплачен");
        }

        [RelayCommand]
        private async void DecrementCount(int foodContainId)
        {
            ButtonsCountIsEnabled = false;
            FoodContain foodContainModel = await Task.Run(() => _dbContext.FoodContain.FirstOrDefault(x => x.FoodContainID == foodContainId));

            if (foodContainModel.Count == 1)
            {
                ButtonsCountIsEnabled = true;
                return;
            }
            else
            {
                foodContainModel.Count--;

                _mainWindowViewModel.BadgeValue--;
                ProductsCount--;

                await _dbContext.SaveChangesAsync();
                InitializeViewModel();
            }
            ButtonsCountIsEnabled = true;
        }

        [RelayCommand]
        private async void DeleteItem(int foodContainId)
        {
            DeleteBtnIsEnabled = false;

            FoodContain foodContainModel = await Task.Run(() => _dbContext.FoodContain.FirstOrDefault(x => x.FoodContainID == foodContainId));

            _mainWindowViewModel.BadgeValue -= foodContainModel.Count;
            ProductsCount -= foodContainModel.Count;

            _dbContext.FoodContain.Remove(foodContainModel);
            await _dbContext.SaveChangesAsync();

            InitializeViewModel();

            DeleteBtnIsEnabled = true;
        }

        [RelayCommand]
        private async void IncrementCount(int foodContainId)
        {
            ButtonsCountIsEnabled = false;
            FoodContain foodContainModel = await Task.Run(() => _dbContext.FoodContain.FirstOrDefault(x => x.FoodContainID == foodContainId));

            if (foodContainModel.Count == 5)
            {
                ButtonsCountIsEnabled = true;
                return;
            }
            else
            {
                foodContainModel.Count++;

                _mainWindowViewModel.BadgeValue++;
                ProductsCount++;

                await _dbContext.SaveChangesAsync();
                InitializeViewModel();
            }
            ButtonsCountIsEnabled = true;
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
