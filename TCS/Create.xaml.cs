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

        public void baseState()
        {
            string tempDate = DateTime.Now.ToString();
            string tempExpDate = DateTime.Now.AddMonths(window_c.states.settings.getMonths()).ToString();

            for (int i = 0; i < 11; i++)
            {
                tempDate = tempDate.Remove(tempDate.Length - 1);
                tempExpDate = tempExpDate.Remove(tempExpDate.Length - 1);
            }

            certDate.Text = tempDate;
            expDate.Text = tempExpDate;

            toCertForm();

            toName.Text = "";
            fromName.Text = "";
            comboBox1.Text = "Services";
            serviceAmount.Text = "Amount";
            toEmail.Text = "";
            fromEmail.Text = "";
            message.Text = "";
            redeemAmount.Text = "";

            checkBoxTitle.Text = "Click 'Create' when ready.";
            toText.Text = "";
            fromText.Text = "";
            toEmailText.Text = "";
            fromEmailText.Text = "";
            serviceText.Text = "";
            amountText.Text = "";
            messageText.Text = "";
            generateCert.Content = "Create";
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
            if (checkBoxTitle.Text.ToString() == "Click 'Create' when ready.")
            {
                checkBoxTitle.Text = "Are you sure? Check info below to make sure!";
                connectContent();
            }
            else if (checkBoxTitle.Text.ToString() == "Are you sure? Check info below to make sure!")
            {
                Core.createCert(toName.Text.ToString(), fromName.Text.ToString(), toEmailText.Text.ToString(), fromEmailText.Text.ToString(), message.Text.ToString(), comboBox1.Text.ToString(), Double.Parse(serviceAmount.Text.ToString()), Int32.Parse(redeemAmount.Text.ToString()), DateTime.Now, window_c.states.settings);
                checkBoxTitle.Text = "Enter email information below.";
                toEmailForm();
                toEmailTextBox.Text = toEmailText.Text.ToString();
                fromEmailTextBox.Text = fromEmailText.Text.ToString();
                generateCert.Content = "Email";
            }
            else if (checkBoxTitle.Text.ToString() == "Enter email information below.")
            {
                Core.queueEmail(toName.Text.ToString(), fromName.Text.ToString(), toEmailTextBox.Text.ToString(), fromEmailTextBox.Text.ToString());
                toEmptyForm();
                checkBoxTitle.Text = "Email has been sent. Please click the reset button to create a new certificate.";
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            baseState();
        }

        void connectContent()
        {
            toText.Text = toName.Text.ToString();
            fromText.Text = fromName.Text.ToString();
            toEmailText.Text = toEmail.Text.ToString();
            fromEmailText.Text = fromEmail.Text.ToString();
            serviceText.Text = comboBox1.Text.ToString();
            amountText.Text = serviceAmount.Text.ToString();
            messageText.Text = message.Text.ToString();
        }

        void toCertForm()
        {
            toText.Visibility = Visibility.Visible;
            fromText.Visibility = Visibility.Visible;
            toEmailText.Visibility = Visibility.Visible;
            fromEmailText.Visibility = Visibility.Visible;
            serviceText.Visibility = Visibility.Visible;
            amountText.Visibility = Visibility.Visible;
            messageText.Visibility = Visibility.Visible;
            toTextBase.Visibility = Visibility.Visible;
            fromTextBase.Visibility = Visibility.Visible;
            toEmailTextBase.Visibility = Visibility.Visible;
            fromEmailTextBase.Visibility = Visibility.Visible;
            serviceTextBase.Visibility = Visibility.Visible;
            amountTextBase.Visibility = Visibility.Visible;
            messageTextBase.Visibility = Visibility.Visible;

            toEmailTextBox.Visibility = Visibility.Collapsed;
            fromEmailTextBox.Visibility = Visibility.Collapsed;
        }

        void toEmailForm()
        {
            toText.Visibility = Visibility.Collapsed;
            fromText.Visibility = Visibility.Collapsed;
            toEmailText.Visibility = Visibility.Collapsed;
            fromEmailText.Visibility = Visibility.Collapsed;
            serviceText.Visibility = Visibility.Collapsed;
            amountText.Visibility = Visibility.Collapsed;
            messageText.Visibility = Visibility.Collapsed;
            toTextBase.Visibility = Visibility.Collapsed;
            fromTextBase.Visibility = Visibility.Collapsed;
            toEmailTextBase.Visibility = Visibility.Visible;
            fromEmailTextBase.Visibility = Visibility.Visible;
            serviceTextBase.Visibility = Visibility.Collapsed;
            amountTextBase.Visibility = Visibility.Collapsed;
            messageTextBase.Visibility = Visibility.Collapsed;

            toEmailTextBox.Visibility = Visibility.Visible;
            fromEmailTextBox.Visibility = Visibility.Visible;
        }

        void toEmptyForm()
        {
            toText.Visibility = Visibility.Collapsed;
            fromText.Visibility = Visibility.Collapsed;
            toEmailText.Visibility = Visibility.Collapsed;
            fromEmailText.Visibility = Visibility.Collapsed;
            serviceText.Visibility = Visibility.Collapsed;
            amountText.Visibility = Visibility.Collapsed;
            messageText.Visibility = Visibility.Collapsed;
            toTextBase.Visibility = Visibility.Collapsed;
            fromTextBase.Visibility = Visibility.Collapsed;
            toEmailTextBase.Visibility = Visibility.Collapsed;
            fromEmailTextBase.Visibility = Visibility.Collapsed;
            serviceTextBase.Visibility = Visibility.Collapsed;
            amountTextBase.Visibility = Visibility.Collapsed;
            messageTextBase.Visibility = Visibility.Collapsed;

            toEmailTextBox.Visibility = Visibility.Collapsed;
            fromEmailTextBox.Visibility = Visibility.Collapsed;
        }
    }
}
