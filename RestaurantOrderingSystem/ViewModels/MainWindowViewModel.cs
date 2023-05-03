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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
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
        private RestaurantDbContext _dbContext;
        private bool _isInitialized = false;

        private INavigationService? navService;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private bool _isCartFilled = false;

        [ObservableProperty]
        private bool _isLoginFilled = false;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private Visibility _loginGridVisibility = Visibility.Hidden;

        [ObservableProperty]
        private string _emailText;

        private int _badgeValue;
        public int BadgeValue
        {
            get => _badgeValue;
            set
            {
                _badgeValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BadgeVisibility));
            }
        }

        private bool _isUserAuthorized = false;
        public bool IsUserAuthorized
        {
            get => _isUserAuthorized;
            set
            {
                _isUserAuthorized = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCartVisible));
                OnPropertyChanged(nameof(IsLoginBtnVisible));
                OnPropertyChanged(nameof(IsLogoutBtnVisible));
            }
        }
        public Visibility BadgeVisibility => BadgeValue > 0 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsCartVisible => IsUserAuthorized == true ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsLoginBtnVisible => IsUserAuthorized == true ? Visibility.Hidden : Visibility.Visible;
        public Visibility IsLogoutBtnVisible => IsUserAuthorized == true ? Visibility.Visible : Visibility.Hidden;
        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        [RelayCommand]
        private void OpenLoginGrid()
        {
            if(LoginGridVisibility == Visibility.Visible)
            {
                IsLoginFilled = false;
                LoginGridVisibility = Visibility.Hidden;
            }
            else
            {
                IsLoginFilled = true;
                LoginGridVisibility = Visibility.Visible;
            } 
        }

        [RelayCommand]
        private void Logout()
        {
            IsUserAuthorized = false;
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.HomePage));
        }

        [RelayCommand]
        private void OpenCart()
        {
            IsCartFilled = true;
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.CartPage));
        }

        [RelayCommand]
        private void Login(PasswordBox? passwordBox)
        {
            try
            {
                User? userModel = _dbContext.User.FirstOrDefault(u => u.UserMail == EmailText && u.UserPassword == passwordBox.Password);

                if(userModel != null)
                {
                    switch (userModel.RoleID)
                    {
                        case 1:
                            break;
                        case 2:
                            LoginGridVisibility = Visibility.Hidden;
                            IsLoginFilled = false;
                            EmailText = string.Empty;
                            passwordBox.Password = string.Empty;
                            IsUserAuthorized = true;
                            break;
                        case 3:
                            break;
                    }
                }
                else
                    System.Windows.MessageBox.Show("Ошибка входа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        [RelayCommand]
        private void GoToSignUp()
        {
            LoginGridVisibility = Visibility.Hidden;
            IsLoginFilled = false;
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.RegistrationPage));
        }

        private async void InitializeViewModel()
        {
            try
            {
                ApplicationTitle = "Global Food";
                Accent.Apply(Color.FromRgb(24, 136, 81));
                _dbContext = await Task.Run(() => new RestaurantDbContext());

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
                },

                new NavigationItem()
                {
                    Visibility = Visibility.Hidden,
                    Content = "Регистрация",
                    PageTag = "registration",
                    PageType = typeof(Views.Pages.RegistrationPage),
                    ToolTip = "Регистрация",
                }
            };

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
    }
}
