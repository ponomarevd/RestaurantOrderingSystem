using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantOrderingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;

namespace RestaurantOrderingSystem.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для CardDataWindow.xaml
    /// </summary>
    public partial class CardDataWindow : INavigableView<ViewModels.CardDataWindowViewModel>
    {
        public ViewModels.CardDataWindowViewModel ViewModel
        {
            get;
        }
        public CardDataWindow(ViewModels.CardDataWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        private void UiWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
