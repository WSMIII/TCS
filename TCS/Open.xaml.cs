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
    /// Interaction logic for Open.xaml
    /// </summary>
    public partial class Open : Page
    {
        MainWindow window_o;

        public Open(MainWindow in_window)
        {
            InitializeComponent();

            window_o = in_window;
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.Content = window_o.states.home;
        }

        private void new_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.Content = window_o.states.create;
        }
        private void open_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.Content = window_o.states.open;
        }
        private void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.Content = window_o.states.redeem;
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            window_o.lastPage = 4;
            window_o.Content = window_o.states.settings;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lookupCert_Click(object sender, RoutedEventArgs e)
        {
            Core.Certificate cert;

            if (toName.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(toName.Text.ToString(), 0);
                connectToCertificate(cert);
            }
            else if (fromName.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(fromName.Text.ToString(), 1);
                connectToCertificate(cert);
            }
            else if (email.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(email.Text.ToString(), 2);
                connectToCertificate(cert);
            }
            /*else if (email.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(email.Text.ToString(), 3);
                connectToCertificate(cert);
            } Implement later */
            else if (code.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(code.Text.ToString(), 4);
                connectToCertificate(cert);
            }
            else if (id.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(id.Text.ToString(), 5);
                connectToCertificate(cert);
            }
        }

        private void connectToCertificate(Core.Certificate in_cert)
        {
            int amountRedeemed = 0;
            int maxAmountUse = 0;

            if (in_cert.name != "DOES NOT EXIST")
            {
                toInfo.Text = in_cert.name;
                fromInfo.Text = in_cert.fromName;
                dateInfo.Text = in_cert.date.ToString();
                expDateInfo.Text = in_cert.expDate.ToString();
                //emailInfo.Text = in_cert.email;
                serviceInfo.Text = in_cert.service;
                serviceAmountInfo.Text = "$" + in_cert.amount.ToString();
                messageInfo.Text = in_cert.code;

                for (int i = 0; i < in_cert.redeem.Length; i++)
                {
                    if (in_cert.redeem[i])
                    {
                        amountRedeemed++;
                    }
                }
                maxAmountUse = in_cert.redeem.Length;

                amountInfo.Text = maxAmountUse.ToString();
                redeemInfo.Text = amountRedeemed.ToString();
            }
            else
            {
                toInfo.Text = in_cert.name;
                fromInfo.Text = "N/A";
                dateInfo.Text = "N/A";
                expDateInfo.Text = "N/A";
                emailInfo.Text = "N/A";
                serviceInfo.Text = "N/A";
                serviceAmountInfo.Text = "N/A";
                messageInfo.Text = "N/A";
                amountInfo.Text = maxAmountUse.ToString();
                redeemInfo.Text = amountRedeemed.ToString();
            }
        }
    }
}
