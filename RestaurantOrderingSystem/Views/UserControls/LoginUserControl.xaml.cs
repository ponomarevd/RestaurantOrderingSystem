using RestaurantOrderingSystem.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Interfaces;

namespace RestaurantOrderingSystem.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void AdminCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(AdminCheckBox.IsChecked == true)
                WorkerCheckBox.IsChecked = false;
        }

        private void WorkerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (WorkerCheckBox.IsChecked == true)
                AdminCheckBox.IsChecked = false;
        }
    }
}
