using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
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
        private RestaurantDbContext _dbContext;
        private MainWindowViewModel _mainWindowViewModel;
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

        [ObservableProperty]
        private bool _btnAddIsHitTestVisible = true;

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
        private async void ToCartClick(string? parameter)
        {
            try
            {
                if (_mainWindowViewModel.IsUserAuthorized)
                {
                    BtnAddIsHitTestVisible = false;
                    SnackbarMessage = "Товар успешно добавлен в корзину";
                    SnackbarAppearance = "Success";

                    Food SelectedFoodModel = MenuItemsSecondary.FirstOrDefault(x => x.FoodName == parameter);

                    //Создание корзины, если ее нет
                    if (await Task.Run(() => _dbContext.Cart.FirstOrDefault(x => x.UserID == _mainWindowViewModel.UserID)) == null)
                    {
                        await _dbContext.Cart.AddAsync(new Cart()
                        {
                            UserID = _mainWindowViewModel.UserID
                        });

                        await _dbContext.SaveChangesAsync();
                    }

                    //Получение корзины пользователя
                    Cart CartModel = await Task.Run(() => _dbContext.Cart.FirstOrDefault(x => x.UserID == _mainWindowViewModel.UserID));

                    //Получение контейнера еды для корзины пользователя
                    FoodContain foodContain = await Task.Run(() => _dbContext.FoodContain.FirstOrDefault(x => x.FoodID == SelectedFoodModel.FoodID && x.CartID == CartModel.CartID));

                    //Инкремент количества выбранной еды, если она уже добавлена, если нет то добавление
                    if (foodContain == null)
                    {
                        await _dbContext.FoodContain.AddAsync(new FoodContain()
                        {
                            CartID = CartModel.CartID,
                            FoodID = SelectedFoodModel.FoodID,
                            Count = 1
                        });
                        _mainWindowViewModel.BadgeValue++;
                    }
                    else
                    {
                        if(foodContain.Count == 5)
                        {
                            SnackbarMessage = "Достигнут лимит количества в корзине для этого товара";
                            SnackbarAppearance = "Danger";
                        }
                        else
                        {
                            foodContain.Count++;
                            _mainWindowViewModel.BadgeValue++;
                        }      
                    }

                    await _dbContext.SaveChangesAsync();

                    BtnAddIsHitTestVisible = true;
                }
                else
                {
                    SnackbarMessage = "Войдите в аккаунт";
                    SnackbarAppearance = "Danger";
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
