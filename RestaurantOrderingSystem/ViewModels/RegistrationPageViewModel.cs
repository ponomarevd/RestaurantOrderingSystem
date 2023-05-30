using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class RegistrationPageViewModel : ObservableObject, INavigationAware
    {
        private RestaurantDbContext _dbContext;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private DateTime _birthday = DateTime.Now;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userMail = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
        private string _userPassword = string.Empty;

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            _dbContext = await Task.Run(() => new RestaurantDbContext());
        }
        [RelayCommand(CanExecute = nameof(CheckFields))]
        private async void CreateAccount()
        {
            User RepetativeUser = await Task.Run(() => _dbContext.User.FirstOrDefault(x => x.UserMail == UserMail));
            if(RepetativeUser == null)
            {
                User userModel = new User()
                {
                    UserName = UserName,
                    UserPassword = UserPassword,
                    UserMail = UserMail,
                    RoleID = 2,
                    UserBirthday = Birthday
                };

                await _dbContext.User.AddAsync(userModel);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                MessageBox.Show("Пользователь с такой почтой уже существует"); // Заменить на snackbar
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
                || UserPassword.Trim() == string.Empty);
        }
        public void OnNavigatedFrom()
        {
        }
    }
}
