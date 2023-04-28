using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using RestaurantOrderingSystem.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;
        private INavigationService? navService;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private Visibility _loginUserControlVisibility = Visibility.Hidden;

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        [RelayCommand]
        private void OpenLoginGrid()
        {
            if(LoginUserControlVisibility == Visibility.Visible)
                LoginUserControlVisibility = Visibility.Hidden;
            else
                LoginUserControlVisibility = Visibility.Visible;
        }

        [RelayCommand]
        private void OpenCart()
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.CartPage));
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "Global Food";
            Accent.Apply(Color.FromRgb(24, 136, 81));


            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Главная",
                    PageTag = "home",
                    Icon = SymbolRegular.Home20,
                    PageType = typeof(Views.Pages.HomePage),
                    ToolTip = "Главная",
                    IconForeground = Brushes.Black
                },

                new NavigationItem()
                {
                    Content = "Меню",
                    PageTag = "menu",
                    Icon = SymbolRegular.Food16,
                    PageType = typeof(Views.Pages.MenuPage),
                    ToolTip = "Меню",
                    IconForeground = Brushes.Black
                },

                new NavigationItem()
                {
                    Content = "Столики",
                    PageTag = "tables",
                    Icon = SymbolRegular.Table48,
                    PageType = typeof(Views.Pages.TablePage),
                    ToolTip = "Столики",
                    IconForeground = Brushes.Black
                },

                new NavigationItem()
                {
                    Content = "Заказы",
                    PageTag = "orders",
                    Icon = SymbolRegular.Check20,
                    PageType = typeof(Views.Pages.OrdersPage),
                    ToolTip = "Заказы",
                    IconForeground = Brushes.Black
                },

                new NavigationItem()
                {
                    Visibility = Visibility.Hidden,
                    Content = "Корзина",
                    PageTag = "cart",
                    Icon = SymbolRegular.Cart16,
                    PageType = typeof(Views.Pages.CartPage),
                    ToolTip = "Корзина",
                }
            };

            _isInitialized = true;
        }
    }
}
