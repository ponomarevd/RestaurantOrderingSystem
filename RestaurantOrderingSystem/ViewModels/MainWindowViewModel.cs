using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
        private MenuPageViewModel _menuPageViewModel;
        private bool _isInitialized = false;
        private INavigationService? navService;
        private User? userModel;
        private List<FoodContain> foodContainItems;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty] 
        [NotifyPropertyChangedFor(nameof(IsLoginFilled))]
        private Visibility _loginGridVisibility = Visibility.Hidden;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LoginInterfaceVisibility))]
        private Visibility _progressRingVisibility = Visibility.Hidden;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private string _snackbarMessage = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _emailText = Properties.Settings.Default.UserMail;

        [ObservableProperty]
        private string _passwordText;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCartVisible), nameof(IsLoginBtnVisible), nameof(IsLogoutBtnVisible))]
        private bool _isUserAuthorized = false;

        [ObservableProperty]
        private bool _isCartFilled = false;

        [ObservableProperty]
        private bool _rememberMeIsChecked = true;

        [ObservableProperty]
        private int _userID = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BadgeVisibility))]
        private int _badgeValue;
        public Visibility BadgeVisibility => BadgeValue > 0 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsCartVisible => IsUserAuthorized == true ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsLoginBtnVisible => IsUserAuthorized == true ? Visibility.Hidden : Visibility.Visible;
        public Visibility IsLogoutBtnVisible => IsUserAuthorized == true ? Visibility.Visible : Visibility.Hidden;
        public Visibility LoginInterfaceVisibility => ProgressRingVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        public bool IsLoginFilled => LoginGridVisibility == Visibility.Visible ? true : false;

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            try
            {
                ApplicationTitle = "Global Food";
                Accent.Apply(Color.FromRgb(24, 136, 81));

                _menuPageViewModel = App.GetService<MenuPageViewModel>();
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
                        IconForeground = Brushes.Black,
                        IsActive = true
                    },

                    new NavigationItem()
                    {
                        Content = "Меню",
                        PageTag = "menu",
                        Icon = SymbolRegular.Food16,
                        PageType = typeof(Views.Pages.MenuPage),
                        ToolTip = "Меню",
                        IconForeground = Brushes.Black
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


        [RelayCommand]
        private void OpenLoginGrid()
        {
            if(LoginGridVisibility == Visibility.Visible)
                LoginGridVisibility = Visibility.Hidden;
            else
                LoginGridVisibility = Visibility.Visible;
        }


        [RelayCommand]
        private async void Logout()
        {
            NavigationItems.RemoveAt(NavigationItems.Count - 1);
            IsUserAuthorized = false;
            BadgeValue = 0;

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


        [RelayCommand(CanExecute = nameof(CheckFields))]
        private async void Login(Snackbar snackbar)
        {
            try
            {
                ProgressRingVisibility = Visibility.Visible;

                _dbContext = new RestaurantDbContext();
                userModel =  await Task.Run(() => _dbContext.User.FirstOrDefaultAsync(u => u.UserMail == EmailText && u.UserPassword == PasswordText));

                if (userModel != null)
                {
                    if (RememberMeIsChecked)
                    {
                        Properties.Settings.Default.UserMail = userModel.UserMail;
                        Properties.Settings.Default.UserPassword = userModel.UserPassword;
                        Properties.Settings.Default.Save();
                    }
                    switch (userModel.RoleID)
                    {
                        case 1:
                            break;
                        case 2:
                            NavigationItems.Add(new NavigationItem()
                            {
                                Content = "Заказы",
                                PageTag = "orders",
                                Icon = SymbolRegular.Check20,
                                PageType = typeof(Views.Pages.OrdersPage),
                                ToolTip = "Заказы",
                                IconForeground = Brushes.Black
                            });
                            
                            foodContainItems = await Task.Run(() => _dbContext.FoodContain.Where(x => x.Cart.UserID == userModel.UserID).ToList());
                            foreach (FoodContain item in foodContainItems)
                                BadgeValue += item.Count;                

                            UserID = userModel.UserID;
                            IsUserAuthorized = true;

                            SnackbarMessage = "Успешный вход!";
                            snackbar.Appearance = ControlAppearance.Success;
                            snackbar.ShowAsync();

                            ProgressRingVisibility = Visibility.Hidden;
                            LoginGridVisibility = Visibility.Hidden;
                            break;
                        case 3:
                            break;
                    }
                }
                else
                {
                    ProgressRingVisibility = Visibility.Hidden;
                    snackbar.Appearance = ControlAppearance.Danger;
                    SnackbarMessage = "Неверные данные!";
                    snackbar.ShowAsync();
                    return;
                }
                    
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private bool CheckFields()
        {
            return !(Regex.IsMatch(EmailText, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) == false || EmailText == null || EmailText.Trim() == string.Empty);
        }


        [RelayCommand]
        private void GoToSignUp()
        {
            LoginGridVisibility = Visibility.Hidden;

            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.RegistrationPage));
        }
    }
}
