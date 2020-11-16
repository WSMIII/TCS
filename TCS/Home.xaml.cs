using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCS
{
    public partial class Home : Page
    {
        MainWindow window_h;

        // CONSTRUCTOR
        public Home(MainWindow in_window)
        {
            InitializeComponent();
            baseState();

            window_h = in_window;
        }

        public void baseState()
        {
            yearAmount.Text = Core.AmountYear.ToString();
            monthAmount.Text = Core.AmountMonth.ToString();
            activeAmount.Text = Core.AmountActive.ToString();
            redeemedAmount.Text = Core.AmountRedeemed.ToString();
        }

        // NAVIGATION FUNCTIONS
        void new_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.create.baseState();
            window_h.Content = window_h.states.create;            
        }
        void open_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.open.baseState();
            window_h.Content = window_h.states.open;
        }
        void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.redeem.baseState();
            window_h.Content = window_h.states.redeem;
        }
        void settings_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.settings.baseState();
            window_h.Content = window_h.states.settings;
        }
        void createCert_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.create.baseState();
            window_h.Content = window_h.states.create;
        }
        void openCert_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.open.baseState();
            window_h.Content = window_h.states.open;
        }
        void redeemCert_Click(object sender, RoutedEventArgs e)
        {
            window_h.lastPage = 1;
            window_h.states.redeem.baseState();
            window_h.Content = window_h.states.redeem;
        }
        void exitSys_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
