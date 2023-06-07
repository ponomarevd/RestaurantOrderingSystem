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
using System.Windows.Controls;
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
        private string _userIDText;

        [ObservableProperty]
        private ObservableCollection<Order> _orderItems;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InterfaceIsEnabled))]
        private Visibility _progressRingVisibility = Visibility.Visible;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InterfaceIsEnabled))]
        private Visibility _emptyOrderVisibility = Visibility.Hidden;

        public bool InterfaceIsEnabled => ProgressRingVisibility == Visibility.Visible || EmptyOrderVisibility == Visibility.Visible ? false : true;

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }

        private async void InitializeViewModel()
        {
            ProgressRingVisibility = Visibility.Visible;

            _mainWindowViewModel = await Task.Run(() => App.GetService<MainWindowViewModel>());
            _dbContext = await Task.Run(() => new RestaurantDbContext());

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.User).Where(x => x.OrderStatus != "Получен")));


            if (OrderItems.Count == 0)
                EmptyOrderVisibility = Visibility.Visible;
            else
                EmptyOrderVisibility = Visibility.Hidden;
                 
            ProgressRingVisibility = Visibility.Hidden;
        }

        [RelayCommand]
        private async void IsPaidChanged(object parameter)
        {
            ProgressRingVisibility = Visibility.Visible;

            var values = (object[])parameter;

            int OrderID = (int)values[0];
            ComboBoxItem SelectedItem = (ComboBoxItem)values[1];
            string value = SelectedItem.Content.ToString();

            Order orderModel = await Task.Run(() => _dbContext.Order.FirstOrDefault(x => x.OrderID == OrderID));

            if (value == "Нет")
                orderModel.IsPaid = false;
            else
                orderModel.IsPaid = true;

            await _dbContext.SaveChangesAsync();
            InitializeViewModel();
        }
        [RelayCommand]
        private async void StatusChanged(object parameter)
        {
            ProgressRingVisibility = Visibility.Visible;

            var values = (object[])parameter;

            int OrderID = (int)values[0];
            ComboBoxItem SelectedItem = (ComboBoxItem)values[1];
            string status = SelectedItem.Content.ToString();

            Order orderModel = await Task.Run(() => _dbContext.Order.FirstOrDefault(x => x.OrderID == OrderID));
            if(status == "Получен")
            {
                orderModel.OrderStatus = status;
                orderModel.IsPaid = true;
            }
            else
                orderModel.OrderStatus = status;

            await _dbContext.SaveChangesAsync();
            InitializeViewModel();
        }

        [RelayCommand]
        private async void GetAllOrders()
        {
            ProgressRingVisibility = Visibility.Visible;

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.User).Where(x => x.OrderStatus != "Получен")));

            ProgressRingVisibility = Visibility.Hidden;
        }

        [RelayCommand]
        private async void GetTodayOrders()
        {
            ProgressRingVisibility = Visibility.Visible;

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.User).Where(x => x.OrderStatus != "Получен" && x.OrderDate == DateTime.Today)));

            ProgressRingVisibility = Visibility.Hidden;
        }
        [RelayCommand]
        private async void IDTextChanged()
        {
            /*ProgressRingVisibility = Visibility.Visible;

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.User).Where(x => x.User.UserID == int.Parse(UserIDText))));

            ProgressRingVisibility = Visibility.Hidden;*/
        }
    }
}
