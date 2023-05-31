using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Core;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class RegistrationPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext _dbContext;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private DateTime _userBirthday = DateTime.Now;

        [ObservableProperty]
        private string _snackbarMessage = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userMail = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userPassword = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FieldsVisibility))]
        private Visibility _progressRingVisibility = Visibility.Hidden;
        public Visibility FieldsVisibility => ProgressRingVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }
        public void OnNavigatedFrom()
        {
        }
        private async void InitializeViewModel()
        {
            _dbContext = await Task.Run(() => new RestaurantDbContext());
        }


        [RelayCommand(CanExecute = nameof(CheckFields))]
        private async void CreateAccount(Snackbar snackbar)
        {
            try
            {
                ProgressRingVisibility = Visibility.Visible;

                User RepetativeUser = await Task.Run(() => _dbContext.User.FirstOrDefaultAsync(x => x.UserMail == UserMail));
                if (RepetativeUser == null)
                {
                    await _dbContext.User.AddAsync(new User()
                    {
                        UserName = UserName,
                        UserPassword = UserPassword,
                        UserMail = UserMail,
                        RoleID = 2,
                        UserBirthday = UserBirthday
                    });

                    await _dbContext.SaveChangesAsync();

                    UserName = UserMail = string.Empty;
                    UserBirthday = DateTime.Now;

                    ProgressRingVisibility = Visibility.Hidden;

                    SnackbarMessage = "Аккаунт создан";
                    snackbar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                    snackbar.Show();
                }
                else
                {
                    ProgressRingVisibility = Visibility.Hidden;

                    SnackbarMessage = "Пользователь с такой почтой уже существует";
                    snackbar.Appearance = Wpf.Ui.Common.ControlAppearance.Danger;
                    snackbar.Show();
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
            return !(Regex.IsMatch(UserMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) == false
                || UserMail == null
                || UserMail.Trim() == string.Empty
                || UserName == null
                || UserName.Trim() == string.Empty
                || UserPassword == null
                || UserPassword.Trim() == string.Empty
                || UserBirthday > DateTime.Now);
        }
    }
}
