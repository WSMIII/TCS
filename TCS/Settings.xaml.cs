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
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private MainWindow window_s;
        private int monthsUntilExp = Core.getExpMonthAmount();

        public Settings(MainWindow in_window)
        {
            InitializeComponent();
            curExpAmount.Text = "Current amount of months until expiration for future gift certificates is: " + monthsUntilExp;
            if (Core.getServiceDisp())
            {
                dispServiceButton.Content = "X";
            }
            if (Core.getAmountDisp())
            {
                dispAmountButton.Content = "X";
            }

            window_s = in_window;
        }

        public int getMonths()
        {
            return monthsUntilExp;
        }

        public bool getServiceDisp()
        {
            if (dispServiceButton.Content == null)
            {
                return false;
            }
            return true;
        }

        public bool getAmountDisp()
        {
            if (dispAmountButton.Content == null)
            {
                return false;
            }
            return true;
        }

        // HEADER MENU METHODS
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.Content = window_s.states.home;
        }

        private void newCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.Content = window_s.states.create;
        }

        private void openCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.Content = window_s.states.open;
        }

        private void redeemCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.Content = window_s.states.redeem;
        }

        // SETTING METHODS
        private void expButton_Click(object sender, RoutedEventArgs e)
        {
            if (expAmount.Text != "")
            {
                monthsUntilExp = Convert.ToInt32(expAmount.Text.ToString());
                curExpAmount.Text = "Current amount of months until expiration for future gift certificates is: " + monthsUntilExp;
            }

            //Try catch for faulty inputs
        }

        private void dispServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (dispServiceButton.Content == null)
            {
                dispServiceButton.Content = "X";
            }
            else
            {
                dispServiceButton.Content = null;
            }
        }

        private void dispAmountButton_Click(object sender, RoutedEventArgs e)
        {
            if (dispAmountButton.Content == null)
            {
                dispAmountButton.Content = "X";
            }
            else
            {
                dispAmountButton.Content = null;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
