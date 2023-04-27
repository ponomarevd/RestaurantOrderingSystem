using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Hosting;
using RestaurantOrderingSystem.Services;
using RestaurantOrderingSystem.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class HomePageViewModel : ObservableObject, INavigationAware
    {
        private INavigationService? navService;
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
        }
    }
}
