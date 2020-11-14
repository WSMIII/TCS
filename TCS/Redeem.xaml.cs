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

        public void baseState()
        {
            redeemCode.Text = "";
            resolveStateText.Text = "";
            toName.Text = "";
            fromName.Text = "";
            date.Text = "";
            expDate.Text = "";
            serviceInfo.Text = "";
            serviceAmountInfo.Text = "";
            redeemInfo.Text = "";
            amountText.Text = "This has been redeemed   times on the dates below.";
            amountInfo.Text = "";
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
            Core.Certificate cert;

            if (redeemCode.Text.ToString().Length > 0)
            {
                cert = Core.redeemCert(redeemCode.Text.ToString());
                connectToCertificate(cert);
            }
        }

        void connectToCertificate(Core.Certificate in_cert)
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

                if ((DateTime.Now > in_cert.RedeemDates[in_cert.RedeemDates.Length - 1].AddSeconds(2)) && in_cert.RedeemDates[in_cert.RedeemDates.Length - 1] != new DateTime())
                {
                    resolveStateText.Text = "Certificate has been redeemed already! Refer below for more information.";
                }
                else
                {
                    resolveStateText.Text = "Certificate has been successfully redeemed! Refer below for more information.";
                }
                toName.Text = in_cert.name;
                fromName.Text = in_cert.fromName;
                date.Text = tempDate;
                expDate.Text = tempExpDate;
                serviceInfo.Text = in_cert.service;
                serviceAmountInfo.Text = "$" + in_cert.amount.ToString();

                for (int i = 0; i < in_cert.Redeem.Length; i++)
                {
                    if (in_cert.Redeem[i])
                    {
                        amountRedeemed++;
                    }
                }
                maxAmountUse = in_cert.Redeem.Length;

                redeemInfo.Text = maxAmountUse.ToString();
                amountText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the dates below.";

                if (amountRedeemed > 0)
                {
                    amountInfo.Text = "";

                    for (int i = 0; i < amountRedeemed; i++)
                    {
                        if (in_cert.RedeemDates[i] != new DateTime())
                        {
                            string tempRedeemDate = in_cert.RedeemDates[i].ToString();

                            for (int j = 0; j < 11; j++)
                            {
                                tempRedeemDate = tempRedeemDate.Remove(tempRedeemDate.Length - 1);
                            }

                            amountInfo.Text = amountInfo.Text.ToString() + (i + 1) + ")   " + tempRedeemDate + "\n";
                        }
                    }
                }
                else
                {
                    amountInfo.Text = "";
                }
            }
            else
            {
                resolveStateText.Text = "Couldn't find a certificate with that code. Please try again.";
                toName.Text = in_cert.name;
                fromName.Text = "N/A";
                date.Text = "N/A";
                expDate.Text = "N/A";
                serviceInfo.Text = "N/A";
                serviceAmountInfo.Text = "N/A";
                redeemInfo.Text = maxAmountUse.ToString();
                amountText.Text = "This has been redeemed " + amountRedeemed.ToString() + "  times on the dates below.";
                amountInfo.Text = "N/A";
            }
        }
    }
}
