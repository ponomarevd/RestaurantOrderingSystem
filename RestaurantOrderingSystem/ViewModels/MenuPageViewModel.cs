using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models;
using RestaurantOrderingSystem.Models.DbTables;
using RestaurantOrderingSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext? _dbContext;
        private MainWindowViewModel? _mainWindowViewModel;
        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItemsMain;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItemsSecondary;

        [ObservableProperty]
        private Visibility _progressRingVisibility = Visibility.Visible;

        [ObservableProperty]
        private string? _searchText;

        [ObservableProperty]
        private string _snackbarMessage;

        [ObservableProperty]
        private string _snackbarAppearance;

        [ObservableProperty]
        private bool _interfaceIsEnabled = false;

        public void OnNavigatedFrom()
        {
        }
        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            try
            {
                _mainWindowViewModel = App.GetService<MainWindowViewModel>();

                _dbContext = await Task.Run(() => new RestaurantDbContext());

                MenuItemsMain = await Task.Run(() => new ObservableCollection<Food>(_dbContext.Food.ToList()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MenuItemsSecondary = await Task.Run(()=>MenuItemsMain);

            _isInitialized = true;
            ProgressRingVisibility = Visibility.Hidden;
            InterfaceIsEnabled = true;
        }


        [RelayCommand]
        private async void TextChanged()
        {
            MenuItemsSecondary = await Task.Run(() => new ObservableCollection<Food>(MenuItemsMain.Where(u => u.FoodName.ToLower().Contains(SearchText.ToLower())).ToList()));
        }


        [RelayCommand]
        private void ToCartClick(string? parameter)
        {
            if (_mainWindowViewModel.IsUserAuthorized)
            {
                Food SelectedFoodModel;
                SelectedFoodModel = MenuItemsSecondary.FirstOrDefault(x => x.FoodName == parameter);

                _mainWindowViewModel.BadgeValue++;

                SnackbarMessage = "Товар успешно добавлен в корзину";
                SnackbarAppearance = "Success";
            }
            else
            {
                SnackbarMessage = "Войдите в аккаунт";
                SnackbarAppearance = "Danger";
                return;
            }
        }


        [RelayCommand]
        private void FilterButtonClick(string? parameter)
        {
            switch(parameter)
            {
                case "Вся еда": MenuItemsSecondary = MenuItemsMain; break;
                case "Сбросить": MenuItemsSecondary = MenuItemsMain; break;

                case "Первые блюда": GetFoodByCategory("Первое блюдо"); break;
                case "Вторые блюда": GetFoodByCategory("Второе блюдо"); break;
                case "Закуски": GetFoodByCategory("Закуска"); break;
                case "Деликатесы": GetFoodByCategory("Деликатес"); break;
                case "Напитки": GetFoodByCategory("Напиток"); break;

                case "Популярное": GetFoodByStatus("Популярное"); break;
                case "Обычное": GetFoodByStatus("Обычное"); break;
                case "Удовлетворительное": GetFoodByStatus("Удовлетворительное"); break;

                case "50 - 100 ₽": GetFoodByPrice(50 ,100); break;
                case "100 - 500 ₽": GetFoodByPrice(100, 500); break;
                case "500 - 1000 ₽": GetFoodByPrice(500, 1000); break;

                case "Мясное": GetFoodByType("Мясное"); break;
                case "Овощное": GetFoodByType("Овощное"); break;
                
                default:
                    return;
            }
        }

        private void GetFoodByCategory(string Value)
        {
            MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == Value));
        }
        private void GetFoodByStatus(string Value)
        {
            MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodStatus == Value));
        }
        private void GetFoodByPrice(int LeftValue, int RightValue)
        {
            MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodPrice >= LeftValue && x.FoodPrice <= RightValue));
        }
        private void GetFoodByType(string Value)
        {
            MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodType == Value));
        }


        /*[RelayCommand] //ДОБАВИТЬ КАРТИНКУ В БД
        private void ButtonClick()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                byte[] imageB = File.ReadAllBytes(ofd.FileName);

                var foodModel = _dbContext.Food.FirstOrDefault(x => x.FoodName == "Улитки");
                foodModel.FoodImage = imageB;
                _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }*/
    }
}
