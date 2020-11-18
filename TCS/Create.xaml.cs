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
    public partial class Create : Page
    {
        private MainWindow window_c;

        // CONSTRUCTOR
        public Create(MainWindow in_window)
        {
            InitializeComponent();

            window_c = in_window;
        }

        // RESET PAGE TO BLANK SLATE
        public void baseState()
        {
            // ADJUST VISIBILITY OF NECCESSARY DATA FIELDS IN THE SECONDARY FORM
            toCertForm();

            // RESETTING DATA FIELDS IN PRIMARY FORM
            toName.Text = "";
            fromName.Text = "";
            certDate.Text = Core.TrimDate(DateTime.Now);
            expDate.Text = Core.TrimDate(DateTime.Now.AddMonths(window_c.states.settings.Months));
            comboBox1.Text = "Services";
            serviceAmount.Text = "Amount";
            toEmail.Text = "";
            fromEmail.Text = "";
            message.Text = "";
            redeemAmount.Text = "";

            // RESETTING DATA FIELDS IN SECONDARY FORM
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

        // NAVIGATION FUNCTIONS
        void home_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.states.home.baseState();
            window_c.Content = window_c.states.home;
        }
        void new_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.states.create.baseState();
            window_c.Content = window_c.states.create;
        }
        void open_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.states.open.baseState();
            window_c.Content = window_c.states.open;
        }
        void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.states.redeem.baseState();
            window_c.Content = window_c.states.redeem;
        }
        void settings_Click(object sender, RoutedEventArgs e)
        {
            window_c.lastPage = 3;
            window_c.states.settings.baseState();
            window_c.Content = window_c.states.settings;
        }
        void backButton_Click(object sender, RoutedEventArgs e)
        {
            switch(window_c.lastPage)
            {
                case 1:
                    window_c.lastPage = 3;
                    window_c.Content = window_c.states.home;
                    break;
                case 2:
                    window_c.lastPage = 3;
                    window_c.states.settings.baseState();
                    window_c.Content = window_c.states.settings;
                    break;
                case 3:
                    window_c.lastPage = 3;
                    window_c.states.create.baseState();
                    window_c.Content = window_c.states.create;
                    break;
                case 4:
                    window_c.lastPage = 3;
                    window_c.states.open.baseState();
                    window_c.Content = window_c.states.open;
                    break;
                case 5:
                    window_c.lastPage = 3;
                    window_c.states.redeem.baseState();
                    window_c.Content = window_c.states.redeem;
                    break;
            }
        }

        // CREATE CERTIFICATE BUTTON
        void generateCert_Click(object sender, RoutedEventArgs e)
        {
            int redemptionAmount = 0;
            double certAmount = 0;

            autofillData(ref redemptionAmount, ref certAmount);

            switch (checkBoxTitle.Text.ToString())
            {                
                // INITIAL CHECK
                case "Click 'Create' when ready.":
                    checkBoxTitle.Text = "Are you sure? Check info below to make sure!";
                    connectContent();
                    break;
                // CONFIRMATION CHECK
                case "Are you sure? Check info below to make sure!":
                    Core.createCert(toName.Text.ToString(), fromName.Text.ToString(), toEmailText.Text.ToString(), fromEmailText.Text.ToString(), message.Text.ToString(), comboBox1.Text.ToString(), certAmount, redemptionAmount, DateTime.Parse(certDate.Text.ToString() + " 12:00:00AM"), window_c.states.settings);
                    checkBoxTitle.Text = "Enter email information below.";
                    toEmailForm();
                    toEmailTextBox.Text = toEmailText.Text.ToString();
                    fromEmailTextBox.Text = fromEmailText.Text.ToString();
                    generateCert.Content = "Email";
                    break;
                // EMAIL CHECK
                case "Enter email information below.":
                    Core.queueEmail(toName.Text.ToString(), fromName.Text.ToString(), toEmailTextBox.Text.ToString(), fromEmailTextBox.Text.ToString());
                    toEmptyForm();
                    checkBoxTitle.Text = "Email has been sent. Please click the reset button to create a new certificate.";
                    break;
            }            
        }

        // RESET PAGE BUTTON
        void resetButton_Click(object sender, RoutedEventArgs e)
        {
            baseState();
        }

        // CONNECTION DATA TO DATA FIELDS
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

        // FILL IN CERTAIN DATA FIELDS DEPENDING ON SERVICE
        void autofillData(ref int in_redempAmount, ref double in_certAmount)
        {
            switch (comboBox1.Text.ToString())
            {
                case "$$$":
                    in_redempAmount = 0;
                    break;
                case "Spiritual Hypnosis (1)":
                    in_redempAmount = 1;
                    in_certAmount = 200;
                    break;
                case "Spiritual Hypnosis (3)":
                    in_redempAmount = 3;
                    in_certAmount = 525;
                    break;
                case "Spiritual Hypnosis (6)":
                    in_redempAmount = 6;
                    in_certAmount = 900;
                    break;
                case "Intuitive Guidance":
                    in_redempAmount = 1;
                    in_certAmount = 125;
                    break;
                case "(4 Week Class) Deep Stress Relief and Healing":
                    in_redempAmount = 1;
                    in_certAmount = 40;
                    break;
                case "(4 Week Class) Soul Travel for Discovery of Self":
                    in_redempAmount = 1;
                    in_certAmount = 40;
                    break;
                case "(4 Week Class) Psychic/Mediumship Development":
                    in_redempAmount = 1;
                    in_certAmount = 40;
                    break;
                case "(4 Week Class) Spiritual Hands on Healing":
                    in_redempAmount = 1;
                    in_certAmount = 40;
                    break;
            }

            if (serviceAmount.Text.ToString() != "Amount")
            {
                in_certAmount = Double.Parse(serviceAmount.Text.ToString());
            }
            if (redeemAmount.Text.ToString() != "")
            {
                in_redempAmount = (redeemAmount.Text.ToString() == "-1") ? 1000 : Int32.Parse(redeemAmount.Text.ToString());
            }
        }

        // ADJUST VISIBILITY OF DATA FIELDS TO DISPLAY CERTIFICATE INFORMATION
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
        // ADJUST VISIBILITY OF DATA FIELDS TO DISPLAY EMAIL INFORMATION
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
        // ADJUST VISIBILITY OF DATA FIELDS TO HIDDEN
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

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri(Core.root + @"graphics\TCS_reset_form_img.png"));
            }
        }

        private void Image_Loaded_1(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri(Core.root + @"graphics\TCS_back_img_1.png"));
            }
        }
    }
}
