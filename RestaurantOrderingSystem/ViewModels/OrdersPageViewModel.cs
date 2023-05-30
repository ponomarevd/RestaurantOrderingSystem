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
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class OrdersPageViewModel : ObservableObject, INavigationAware
    {
        private INavigationService? navService;
        private RestaurantDbContext _dbContext;
        private MainWindowViewModel _mainWindowViewModel;
        private OrderDetailsPageViewModel _orderDetailsPageViewModel;

        [ObservableProperty]
        private ObservableCollection<Order> _orderItems;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(OrdersVisibility))]
        private Visibility _progressRingVisibility = Visibility.Hidden;

        [ObservableProperty]
        private Visibility _emptyOrdersVisibility = Visibility.Hidden;
        public Visibility OrdersVisibility => ProgressRingVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

        private async void InitializeViewModel()
        {
            ProgressRingVisibility = Visibility.Visible;

            _mainWindowViewModel = App.GetService<MainWindowViewModel>();
            _dbContext = await Task.Run(() => new RestaurantDbContext());

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.OrderContain).Where(x => x.UserID == _mainWindowViewModel.UserID)));

            if (OrderItems.Count == 0)
            {
                ProgressRingVisibility = Visibility.Hidden;
                EmptyOrdersVisibility = Visibility.Visible;
            }
            else
            {
                ProgressRingVisibility = Visibility.Hidden;
                EmptyOrdersVisibility = Visibility.Hidden;
            }
        }

        [RelayCommand]
        private void ShowDetails(int OrderId)
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.OrderDetailsPage));

            _orderDetailsPageViewModel = App.GetService<OrderDetailsPageViewModel>();
            _orderDetailsPageViewModel.OrderID = OrderId;
        }

        [RelayCommand]
        private void GoToMenu()
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.MenuPage));
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }
    }
}
