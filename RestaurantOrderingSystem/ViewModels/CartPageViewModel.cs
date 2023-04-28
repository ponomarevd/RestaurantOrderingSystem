using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.ViewModels
{
    public class CartPageViewModel : ObservableObject, INavigationAware
    {
        private MainWindowViewModel? _mainWindowViewModel;

        public CartPageViewModel()
        {
            _mainWindowViewModel = App.GetService<MainWindowViewModel>();
        }
        public void OnNavigatedFrom()
        {
            _mainWindowViewModel.IsCartFilled = false;
        }

        public void OnNavigatedTo()
        {
        }
    }
}
