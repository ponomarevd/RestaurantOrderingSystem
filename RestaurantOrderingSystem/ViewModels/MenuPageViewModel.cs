using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext _dbContext;

        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<Food> _menuItems;
        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _dbContext = new RestaurantDbContext();
            MenuItems = new ObservableCollection<Food>(_dbContext.Food.ToList());
            _isInitialized = true;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        /*[RelayCommand] //ДОБАВИТЬ КАРТИНКУ В БД
        private void ButtonClick()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            byte[] imageB = File.ReadAllBytes(ofd.FileName);

            var foodModel = _dbContext.Food.FirstOrDefault(x => x.FoodName == "Луковый суп");
            foodModel.FoodImage = imageB;
            _dbContext.SaveChangesAsync();
        }*/
    }
}
