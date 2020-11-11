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
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        private MainWindow window_c;

        public Create(MainWindow in_window)
        {
            InitializeComponent();

            window_c = in_window;
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.Content = window_c.states.home;
        }

        private void new_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.Content = window_c.states.create;
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.Content = window_c.states.open;
        }

        private void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.Content = window_c.states.redeem;
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.Content = window_c.states.settings;
        }
        private void generateCert_Click(object sender, RoutedEventArgs e)
        {
            Core.createCert(toName.Text.ToString(), fromName.Text.ToString(), email.Text.ToString(), message.Text.ToString(), comboBox1.Text.ToString(), Double.Parse(serviceAmount.Text.ToString()), Int32.Parse(redeemAmount.Text.ToString()), DateTime.Now, window_c.states.settings);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
