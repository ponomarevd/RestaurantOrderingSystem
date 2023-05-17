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
    public partial class CardDataWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _cardNumber;

        [ObservableProperty]
        private string _month;

        [ObservableProperty]
        private string _year;

        [ObservableProperty]
        private string _cvv;

        [RelayCommand]
        private void Confirm()
        {
            string CVV = Cvv;
        }
    }
}
