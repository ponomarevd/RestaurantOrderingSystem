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
        [NotifyPropertyChangedFor(nameof(PreparingBorderBackground), nameof(PreparingIconForeground), nameof(IsPaidBorderBackground), nameof(IsPaidIconForeground), nameof(ReadyBorderBackground), nameof(ReadyIconForeground))]
        private int _progressBarValue = 50;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(OrdersVisibility))]
        private Visibility _progressRingVisibility = Visibility.Visible;

        public Visibility OrdersVisibility => ProgressRingVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        public Brush PreparingBorderBackground => ProgressBarValue >= 50 ? (Brush)new BrushConverter().ConvertFrom("#188851") : (Brush)new BrushConverter().ConvertFrom("#ededed");
        public Brush PreparingIconForeground => ProgressBarValue >= 50 ? Brushes.White : Brushes.Black;
        public Brush IsPaidBorderBackground => ProgressBarValue >= 86 ? (Brush)new BrushConverter().ConvertFrom("#188851") : (Brush)new BrushConverter().ConvertFrom("#ededed");
        public Brush IsPaidIconForeground => ProgressBarValue >= 86 ? Brushes.White : Brushes.Black;
        public Brush ReadyBorderBackground => ProgressBarValue == 100 ? (Brush)new BrushConverter().ConvertFrom("#188851") : (Brush)new BrushConverter().ConvertFrom("#ededed");
        public Brush ReadyIconForeground => ProgressBarValue == 100 ? Brushes.White : Brushes.Black;

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

            _mainWindowViewModel = App.GetService<MainWindowViewModel>();
            _dbContext = await Task.Run(() => new RestaurantDbContext());

            OrderItems = await Task.Run(() => new ObservableCollection<Order>(_dbContext.Order.Include(x => x.OrderContain).Where(x => x.UserID == _mainWindowViewModel.UserID)));

            ProgressRingVisibility = Visibility.Hidden;
        }

        [RelayCommand]
        private void ShowDetails(int OrderId)
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.OrderDetailsPage));

            _orderDetailsPageViewModel = App.GetService<OrderDetailsPageViewModel>();
            _orderDetailsPageViewModel.OrderID = OrderId;
        }
    }
}
