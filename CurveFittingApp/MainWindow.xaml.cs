using CurveFittingApp.ViewModels;
using System.Windows;

namespace CurveFittingApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}

