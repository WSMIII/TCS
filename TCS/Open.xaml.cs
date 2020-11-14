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

        public void baseState()
        {
            toName.Text = "";
            fromName.Text = "";
            toEmail.Text = "";
            fromEmail.Text = "";
            code.Text = "";
            id.Text = "";

            toInfo.Text = "";
            fromInfo.Text = "";
            toEmailInfo.Text = "";
            fromEmailInfo.Text = "";
            dateInfo.Text = "";
            expDateInfo.Text = "";
            serviceInfo.Text = "";
            serviceAmountInfo.Text = "";
            messageInfo.Text = "";
            amountInfo.Text = "";
            redeemText.Text = "This has been redeemed # times on the the dates below.";
            redeemInfo.Text = "";
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
            else if (toEmail.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(toEmail.Text.ToString(), 2);
                connectToCertificate(cert);
            }
            else if (fromEmail.Text.ToString().Length > 0)
            {
                cert = Core.lookupCert(fromEmail.Text.ToString(), 3);
                connectToCertificate(cert);
            }
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
                string tempDate = in_cert.date.ToString();
                string tempExpDate = in_cert.expDate.ToString();

                for (int i = 0; i < 11; i++)
                {
                    tempDate = tempDate.Remove(tempDate.Length - 1);
                    tempExpDate = tempExpDate.Remove(tempExpDate.Length - 1);
                }

                toInfo.Text = in_cert.name;
                fromInfo.Text = in_cert.fromName;
                dateInfo.Text = tempDate;
                expDateInfo.Text = tempExpDate;
                toEmailInfo.Text = in_cert.toEmail;
                fromEmailInfo.Text = in_cert.fromEmail;
                serviceInfo.Text = in_cert.service;
                serviceAmountInfo.Text = "$" + in_cert.amount.ToString();
                messageInfo.Text = in_cert.message;

                for (int i = 0; i < in_cert.Redeem.Length; i++)
                {
                    if (in_cert.Redeem[i])
                    {
                        amountRedeemed++;
                    }
                }
                maxAmountUse = in_cert.Redeem.Length;

                amountInfo.Text = maxAmountUse.ToString();
                redeemText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the the dates below.";

                if (amountRedeemed > 0)
                {
                    redeemInfo.Text = "";

                    for (int i = 0; i < amountRedeemed; i++)
                    {
                        if (in_cert.RedeemDates[i] != new DateTime())
                        {
                            string tempRedeemDate = in_cert.RedeemDates[i].ToString();

                            for (int j = 0; j < 11; j++)
                            {
                                tempRedeemDate = tempRedeemDate.Remove(tempRedeemDate.Length - 1);
                            }

                            redeemInfo.Text = redeemInfo.Text.ToString() + (i + 1) + ")   " + tempRedeemDate + "\n";
                        }
                    }
                }
                else
                {
                    redeemInfo.Text = "";
                }
            }
            else
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
                amountInfo.Text = maxAmountUse.ToString();
                redeemText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the the dates below.";
                redeemInfo.Text = "N/A";
            }
        }
    }
}
