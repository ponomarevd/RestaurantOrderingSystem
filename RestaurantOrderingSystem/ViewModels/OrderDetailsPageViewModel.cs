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
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class OrderDetailsPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<OrderContain> _orderDetailItems;

        [ObservableProperty]
        private int _orderID;
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        private async void InitializeViewModel()
        {
            _dbContext = await Task.Run(() => new RestaurantDbContext());
            OrderDetailItems = await Task.Run(() => new ObservableCollection<OrderContain>(_dbContext.OrderContain.Include(x => x.Food).Where(x => x.OrderID == OrderID)));
        }
    }
}
