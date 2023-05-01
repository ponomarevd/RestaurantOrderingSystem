using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class LoginUserControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _email;

        [RelayCommand]
        private void Login()
        {
        }
    }
}
