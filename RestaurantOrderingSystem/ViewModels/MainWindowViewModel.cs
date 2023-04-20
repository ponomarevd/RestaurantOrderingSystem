using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _footerItems = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
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
                    Icon = SymbolRegular.Table32,
                    PageType = typeof(Views.Pages.TablePage),
                    ToolTip = "Столики",
                    IconForeground = Brushes.Black
                },

                new NavigationItem()
                {
                    Content = "Акции",
                    PageTag = "promotions",
                    Icon = SymbolRegular.ShoppingBagPercent20,
                    PageType = typeof(Views.Pages.PromotionsPage),
                    ToolTip = "Акции",
                    IconForeground = Brushes.Black
                }   
            };

            FooterItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Корзина",
                    PageTag = "cart",
                    Icon = SymbolRegular.Cart16,
                    PageType = typeof(Views.Pages.CartPage),
                    ToolTip = "Корзина",
                },

                new NavigationItem()
                {
                    Content = "Вход",
                    PageTag = "login",
                    Icon = SymbolRegular.Person16,
                    PageType = typeof(Views.Pages.LoginPage),
                    ToolTip = "Вход в аккаунт",
                }
            };

            _isInitialized = true;
        }
    }
}
