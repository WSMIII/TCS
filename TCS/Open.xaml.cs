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
    public partial class Open : Page
    {
        MainWindow window_o;

        // CONSTRUCTOR
        public Open(MainWindow in_window)
        {
            InitializeComponent();

            window_o = in_window;
        }

        // RESET PAGE TO BLANK SLATE
        public void baseState()
        {
            Core.curCert = 0;

            // RESETTING DATA FIELDS IN LOOKUP FORM
            toName.Text = "";
            fromName.Text = "";
            toEmail.Text = "";
            fromEmail.Text = "";
            code.Text = "";
            id.Text = "";

            // RESETTING DATA FIELDS IN PRIMARY FORM
            toInfo.Text = "";
            fromInfo.Text = "";
            toEmailInfo.Text = "";
            fromEmailInfo.Text = "";
            dateInfo.Text = "";
            expDateInfo.Text = "";
            serviceInfo.Text = "";
            serviceAmountInfo.Text = "";
            messageInfo.Text = "";
            codeInfo.Text = "";
            amountInfo.Text = "";
            redeemText.Text = "This has been redeemed # times on the the dates below.";
            redeemInfo.Text = "";
        }

        // NAVIGATION FUNCTIONS
        void home_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.states.home.baseState();
            window_o.Content = window_o.states.home;
        }
        void new_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.states.create.baseState();
            window_o.Content = window_o.states.create;
        }
        void open_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.states.open.baseState();
            window_o.Content = window_o.states.open;
        }
        void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.states.redeem.baseState();
            window_o.Content = window_o.states.redeem;
        }
        void settings_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.states.settings.baseState();
            window_o.Content = window_o.states.settings;
        }
        void backButton_Click(object sender, RoutedEventArgs e)
        {
            switch (window_o.lastPage)
            {
                case 1:
                    window_o.lastPage = 4;
                    window_o.Content = window_o.states.home;
                    break;
                case 2:
                    window_o.lastPage = 4;
                    window_o.states.settings.baseState();
                    window_o.Content = window_o.states.settings;
                    break;
                case 3:
                    window_o.lastPage = 4;
                    window_o.states.create.baseState();
                    window_o.Content = window_o.states.create;
                    break;
                case 4:
                    window_o.lastPage = 4;
                    window_o.states.open.baseState();
                    window_o.Content = window_o.states.open;
                    break;
                case 5:
                    window_o.lastPage = 4;
                    window_o.states.redeem.baseState();
                    window_o.Content = window_o.states.redeem;
                    break;
            }
        }

        // LOOKUP CERTIFICATE BUTTON
        void lookupCert_Click(object sender, RoutedEventArgs e)
        {
            Core.Certificate cert = null;

            if (toName.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(toName.Text.ToString().ToLower(), 0);
            }
            else if (fromName.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(fromName.Text.ToString().ToLower(), 1);
            }
            else if (toEmail.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(toEmail.Text.ToString().ToLower(), 2);
            }
            else if (fromEmail.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(fromEmail.Text.ToString().ToLower(), 3);
            }
            else if (code.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(code.Text.ToString(), 4);
            }
            else if (id.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(id.Text.ToString(), 5);
            }

            connectToCertificate(cert);
        }

        void nextCertButton_Click(object sender, RoutedEventArgs e)
        {
            if (Core.curCert < Core.certsFound.Count - 1)
            {
                Core.curCert++;
                connectToCertificate(Core.certsFound[Core.curCert]);
            }
        }

        void prevCertButton_Click(object sender, RoutedEventArgs e)
        {
            if (Core.curCert > 0)
            {
                Core.curCert--;
                connectToCertificate(Core.certsFound[Core.curCert]);
            }
        }

        // CONNECTION DATA TO DATA FIELDS
        void connectToCertificate(Core.Certificate in_cert)
        {
            int amountRedeemed = 0;
            int maxAmountUse = 0;

            if (in_cert.name != "DOES NOT EXIST") // WAS FOUND
            {
                toInfo.Text = in_cert.name;
                fromInfo.Text = in_cert.fromName;
                dateInfo.Text = Core.TrimDate(in_cert.date); 
                expDateInfo.Text = Core.TrimDate(in_cert.expDate);
                toEmailInfo.Text = in_cert.toEmail;
                fromEmailInfo.Text = in_cert.fromEmail;
                serviceInfo.Text = in_cert.service;
                serviceAmountInfo.Text = "$" + in_cert.amount.ToString();
                messageInfo.Text = in_cert.message;
                codeInfo.Text = in_cert.Code;

                for (int i = 0; i < in_cert.Redeem.Length; i++) // HOW MANY HAVE BEEN REDEEMED
                {
                    amountRedeemed += (in_cert.Redeem[i]) ? 1 : 0;
                }
                maxAmountUse = in_cert.Redeem.Length; // HOW MANY TIMES CAN IT BE REDEEMED

                amountInfo.Text = maxAmountUse.ToString();
                redeemText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the the dates below.";

                if (amountRedeemed > 0) // WHETHER TO DISPLAY REDEMPTION TIME STAMPS IF THEY EXIST
                {
                    redeemInfo.Text = ""; // RESETTING TIME STAMP FIELD

                    for (int i = 0; i < amountRedeemed; i++) // DISPLAYING REDEMPTION TIME STAMPS
                    {
                        if (in_cert.RedeemDates[i] != new DateTime()) redeemInfo.Text = redeemInfo.Text.ToString() + (i + 1) + ")   " + Core.TrimDate(in_cert.RedeemDates[i]) + "\n";
                    }
                }
                else redeemInfo.Text = "";
            }
            else // WASN'T FOUND
            {
                toInfo.Text = in_cert.name;
                fromInfo.Text = "N/A";
                dateInfo.Text = "N/A";
                expDateInfo.Text = "N/A";
                toEmailInfo.Text = "N/A";
                fromEmailInfo.Text = "N/A";
                serviceInfo.Text = "N/A";
                serviceAmountInfo.Text = "N/A";
                messageInfo.Text = "N/A";
                codeInfo.Text = "N/A";
                amountInfo.Text = maxAmountUse.ToString();
                redeemText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the the dates below.";
                redeemInfo.Text = "N/A";
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri(Core.root + @"graphics\TCS_back_img_1.png"));
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

        private void Image_Loaded_2(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri(Core.root + @"graphics\TCS_back_img_1.png"));
            }
        }
    }
}
