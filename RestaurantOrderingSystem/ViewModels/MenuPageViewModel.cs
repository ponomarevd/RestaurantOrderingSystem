using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models;
using RestaurantOrderingSystem.Models.DbTables;
using System.Collections.ObjectModel;
using System.Linq;
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext? _dbContext;

        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItemsMain;

        [ObservableProperty]
        private ObservableCollection<ButtonItem>? _filterButtons;

        [ObservableProperty]
        private ObservableCollection<Food>? _menuItemsSecondary;
        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _dbContext = new RestaurantDbContext();

            MenuItemsMain = new ObservableCollection<Food>(_dbContext.Food.ToList());
            MenuItemsSecondary = MenuItemsMain;

            FilterButtons = new ObservableCollection<ButtonItem>()
            {
                new ButtonItem
                {
                    Content = "Вся еда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new ButtonItem
                {
                    Content = "Первые блюда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new ButtonItem
                {
                    Content = "Вторые блюда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new ButtonItem
                {
                    Content = "Закуски",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new ButtonItem
                {
                    Content = "Деликатесы",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new ButtonItem
                {
                    Content = "Напитки",
                    Command = new RelayCommand<string>(FilterButtonClick)
                }
            };

            _isInitialized = true;
        }

        [RelayCommand]
        private void ToCartClick(string? obj)
        {
            
        }

        [RelayCommand]
        private void FilterButtonClick(string? parameter)
        {
            switch(parameter)
            {
                case "Вся еда":
                    MenuItemsSecondary = MenuItemsMain;
                    break;
                case "Первые блюда":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == "Первое блюдо"));
                    break;
                case "Вторые блюда":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == "Второе блюдо"));
                    break;
                case "Закуски":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == "Закуска"));
                    break;
                case "Деликатесы":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == "Деликатес"));
                    break;
                case "Напитки":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodCategory == "Напиток"));
                    break;
                case "Популярное":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodStatus == "Популярное"));
                    break;
                case "Обычное":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodStatus == "Обычное"));
                    break;
                case "Удовлетворительное":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodStatus == "Удовлетворительное"));
                    break;
                case "50 - 100 ₽":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodPrice >= 50 && x.FoodPrice <= 100));
                    break;
                case "100 - 500 ₽":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodPrice >= 100 && x.FoodPrice <= 500));
                    break;
                case "500 - 1000 ₽":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodPrice >= 500 && x.FoodPrice <= 1000));
                    break;
                case "Мясное":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodType == "Мясное"));
                    break;
                case "Овощное":
                    MenuItemsSecondary = new ObservableCollection<Food>(MenuItemsMain.Where(x => x.FoodType == "Овощное"));
                    break;
                case "Сбросить":
                    MenuItemsSecondary = MenuItemsMain;
                    break;
                default:
                    return;
            }
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
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
