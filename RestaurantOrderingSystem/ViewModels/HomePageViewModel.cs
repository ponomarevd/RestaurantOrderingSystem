using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Mvvm.Contracts;

namespace RestaurantOrderingSystem.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private INavigationService? navService;
        [RelayCommand]
        private void GoToMenu()
        {
            navService = App.GetService<INavigationService>();
            navService.Navigate(typeof(Views.Pages.MenuPage));
        }
    }
}
