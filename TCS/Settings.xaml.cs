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
        MainWindow window_s;
        int monthsUntilExp = Core.ExpMonthAmount;

        // CONSTRUCTOR
        public Settings(MainWindow in_window)
        {
            InitializeComponent();

            curExpAmount.Text = "Current amount of months until expiration for future gift certificates is: " + monthsUntilExp;
            if (Core.ServiceDisp) dispServiceButton.Content = "X";
            if (Core.AmountDisp) dispAmountButton.Content = "X";
            
            window_s = in_window;
        }

        // RESET PAGE TO BLANK SLATE
        public void baseState()
        {
            expAmount.Text = "";
        }

        // GETTER FUNCTIONS
        public int Months { get { return monthsUntilExp; } }
        public bool ServiceDisp { get { return !(dispServiceButton.Content == null); } }
        public bool AmountDisp {  get { return !(dispAmountButton.Content == null); } }

        // NAVIGATION FUNCTIONS
        void Home_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.states.home.baseState();
            window_s.Content = window_s.states.home;
        }
        void newCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.states.create.baseState();
            window_s.Content = window_s.states.create;
        }
        void openCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.states.open.baseState();
            window_s.Content = window_s.states.open;
        }
        void redeemCert_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.states.redeem.baseState();
            window_s.Content = window_s.states.redeem;
        }
        void settings_Click(object sender, RoutedEventArgs e)
        {
            window_s.lastPage = 2;
            window_s.states.settings.baseState();
            window_s.Content = window_s.states.settings;
        }
        void backButton_Click(object sender, RoutedEventArgs e)
        {
            switch (window_s.lastPage)
            {
                case 1:
                    window_s.lastPage = 2;
                    window_s.Content = window_s.states.home;
                    break;
                case 2:
                    window_s.lastPage = 2;
                    window_s.states.settings.baseState();
                    window_s.Content = window_s.states.settings;
                    break;
                case 3:
                    window_s.lastPage = 2;
                    window_s.states.create.baseState();
                    window_s.Content = window_s.states.create;
                    break;
                case 4:
                    window_s.lastPage = 2;
                    window_s.states.open.baseState();
                    window_s.Content = window_s.states.open;
                    break;
                case 5:
                    window_s.lastPage = 2;
                    window_s.states.redeem.baseState();
                    window_s.Content = window_s.states.redeem;
                    break;
            }
        }

        // INTERACTIVE METHODS
        void expButton_Click(object sender, RoutedEventArgs e)
        {
            if (expAmount.Text != "")
            {
                monthsUntilExp = Convert.ToInt32(expAmount.Text.ToString());
                curExpAmount.Text = "Current amount of months until expiration for future gift certificates is: " + monthsUntilExp;
            }
            //Try catch for faulty inputs
        }

        void dispServiceButton_Click(object sender, RoutedEventArgs e)
        {
            dispServiceButton.Content = (dispServiceButton.Content == null) ? "X" : null;
        }

        void dispAmountButton_Click(object sender, RoutedEventArgs e)
        {
            dispAmountButton.Content = (dispAmountButton.Content == null) ? "X" : null;
        }

        private void rootButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
