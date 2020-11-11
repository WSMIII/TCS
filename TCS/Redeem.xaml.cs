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
    /// Interaction logic for Redeem.xaml
    /// </summary>
    public partial class Redeem : Page
    {
        private MainWindow window_r;
        public Redeem(MainWindow in_window)
        {
            InitializeComponent();

            window_r = in_window;
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 4;
            window_r.Content = window_r.states.home;
        }

        private void new_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 4;
            window_r.Content = window_r.states.create;
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 4;
            window_r.Content = window_r.states.open;
        }

        private void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 4;
            window_r.Content = window_r.states.redeem;
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 4;
            window_r.Content = window_r.states.settings;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void redeemButton_Click(object sender, RoutedEventArgs e)
        {

            Core.redeemCert(redeemCode.Text.ToString());
        }

        void connectToCertificate(Core.Certificate in_cert)
        {

        }
    }
}
