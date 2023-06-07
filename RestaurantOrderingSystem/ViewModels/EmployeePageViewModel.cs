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
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class EmployeePageViewModel : ObservableObject, INavigationAware
    {
        private INavigationService? navService;
        private RestaurantDbContext _dbContext;
        private MainWindowViewModel _mainWindowViewModel;

        [ObservableProperty]
        private ObservableCollection<Order> _orderItems;
        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        private async void InitializeViewModel()
        {
            _mainWindowViewModel = App.GetService<MainWindowViewModel>();
            _dbContext = await Task.Run(() => new RestaurantDbContext());

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.OrderContain).Include(x => x.User).Where(x => x.OrderStatus != "Получен")));
        }

        [RelayCommand]
        private async void IsPaidChanged(object parameter)
        {
            //Order orderModel = await Task.Run(() => _dbContext.Order.FirstOrDefault(x => x.OrderID == OrderID));
        }
    }
}
