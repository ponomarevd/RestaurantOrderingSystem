using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext _dbContext;

        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItemsMain;

        [ObservableProperty]
        private ObservableCollection<ButtonItem> _filterButtons;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItemsSecondary;
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
        private void FilterButtonClick(string parameter)
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

                var foodModel = _dbContext.Food.FirstOrDefault(x => x.FoodName == "Рататуй");
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
