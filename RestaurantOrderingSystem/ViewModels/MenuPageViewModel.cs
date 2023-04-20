using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    }
}
