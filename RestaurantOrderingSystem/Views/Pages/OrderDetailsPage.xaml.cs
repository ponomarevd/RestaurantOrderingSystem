﻿using System;
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

namespace RestaurantOrderingSystem.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderDetailsPage.xaml
    /// </summary>
    public partial class OrderDetailsPage : INavigableView<ViewModels.OrderDetailsPageViewModel>
    {
        public ViewModels.OrderDetailsPageViewModel ViewModel
        {
            get;
        }
        public OrderDetailsPage(ViewModels.OrderDetailsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
