using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models;
using RestaurantOrderingSystem.Models.DbTables;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
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
        private string? _searchText;

        [ObservableProperty]
        private Visibility _progressRingVisibility = Visibility.Visible;

        [ObservableProperty]
        private bool _searchIsEnabled = false;

        [ObservableProperty]
        private string _buttonContent = "В корзину";

        [ObservableProperty]
        private bool _filtersIsEnabled = false;

        [ObservableProperty]
        private ObservableCollection<FilterButtonItem>? _filterButtons;

        [ObservableProperty]
        private ObservableCollection<Food>? _menuItemsSecondary;
        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private async void TextChanged()
        {
            MenuItemsSecondary = await Task.Run(() => new ObservableCollection<Food>(MenuItemsMain.Where(u => u.FoodName.ToLower().Contains(SearchText.ToLower())).ToList()));
        }

        private async void InitializeViewModel()
        {
            _dbContext = await Task.Run(()=>new RestaurantDbContext());

            MenuItemsMain = await Task.Run(()=>new ObservableCollection<Food>(_dbContext.Food.ToList()));

            foreach (Food food in MenuItemsMain)
            {
                food.ToCartButtonItem = new ToCartButtonItem()
                {
                    Content = "В корзину",
                    Command = new RelayCommand<string>(ToCartClick),
                    ItemName = food.FoodName,
                    BorderBrush = new BrushConverter().ConvertFrom("#188851") as SolidColorBrush,
                    Foreground = new BrushConverter().ConvertFrom("#188851") as SolidColorBrush
                };
            }

            MenuItemsSecondary = await Task.Run(()=>MenuItemsMain);

            FilterButtons = await Task.Run(() => new ObservableCollection<FilterButtonItem>()
            {
                new FilterButtonItem
                {
                    Content = "Вся еда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new FilterButtonItem
                {
                    Content = "Первые блюда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new FilterButtonItem
                {
                    Content = "Вторые блюда",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new FilterButtonItem
                {
                    Content = "Закуски",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new FilterButtonItem
                {
                    Content = "Деликатесы",
                    Command = new RelayCommand<string>(FilterButtonClick)
                },
                new FilterButtonItem
                {
                    Content = "Напитки",
                    Command = new RelayCommand<string>(FilterButtonClick)
                }
            });

            _isInitialized = true;
            ProgressRingVisibility = Visibility.Hidden;
            SearchIsEnabled = true;
            FiltersIsEnabled = true;
 
            
        }

        [RelayCommand]
        private void ToCartClick(string parameter)
        {
            Food SelectedFoodModel = MenuItemsSecondary.FirstOrDefault(x => x.FoodName == parameter);

            switch (SelectedFoodModel.ToCartButtonItem.Content)
            {
                case "Убрать":
                    SelectedFoodModel.ToCartButtonItem.Content = "В корзину";
                    SelectedFoodModel.ToCartButtonItem.BorderBrush = new BrushConverter().ConvertFrom("#188851") as SolidColorBrush;
                    SelectedFoodModel.ToCartButtonItem.Foreground = new BrushConverter().ConvertFrom("#188851") as SolidColorBrush;
                    CollectionViewSource.GetDefaultView(MenuItemsSecondary).Refresh();
                    break;
                case "В корзину":
                    SelectedFoodModel.ToCartButtonItem.Content = "Убрать";
                    SelectedFoodModel.ToCartButtonItem.BorderBrush = Brushes.DarkRed;
                    SelectedFoodModel.ToCartButtonItem.Foreground = Brushes.DarkRed;
                    CollectionViewSource.GetDefaultView(MenuItemsSecondary).Refresh();
                    break;
            }
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
