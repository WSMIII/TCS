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
    public partial class Redeem : Page
    {
        MainWindow window_r;

        // CONSTRUCTOR
        public Redeem(MainWindow in_window)
        {
            InitializeComponent();

            window_r = in_window;
        }

        // RESET PAGE TO BLANK SLATE
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

        // NAVIGATION FUNCTIONS
        void home_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 5;
            window_r.states.home.baseState();
            window_r.Content = window_r.states.home;
        }
        void new_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 5;
            window_r.states.create.baseState();
            window_r.Content = window_r.states.create;
        }
        void open_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 5;
            window_r.states.open.baseState();
            window_r.Content = window_r.states.open;
        }
        void redeem_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 5;
            window_r.states.redeem.baseState();
            window_r.Content = window_r.states.redeem;
        }
        void settings_Click(object sender, RoutedEventArgs e)
        {
            window_r.lastPage = 5;
            window_r.states.settings.baseState();
            window_r.Content = window_r.states.settings;
        }
        void backButton_Click(object sender, RoutedEventArgs e)
        {
            switch (window_r.lastPage)
            {
                case 1:
                    window_r.lastPage = 5;
                    window_r.Content = window_r.states.home;
                    break;
                case 2:
                    window_r.lastPage = 5;
                    window_r.states.settings.baseState();
                    window_r.Content = window_r.states.settings;
                    break;
                case 3:
                    window_r.lastPage = 5;
                    window_r.states.create.baseState();
                    window_r.Content = window_r.states.create;
                    break;
                case 4:
                    window_r.lastPage = 5;
                    window_r.states.open.baseState();
                    window_r.Content = window_r.states.open;
                    break;
                case 5:
                    window_r.lastPage = 5;
                    window_r.states.redeem.baseState();
                    window_r.Content = window_r.states.redeem;
                    break;
            }
        }

        // REDEEM CERTIFICATE BUTTON
        void redeemButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Certificate cert;

            if (redeemCode.Text.ToString().Length > 0)
            {
                cert = Core.redeemCert(redeemCode.Text.ToString(), serviceUsed.Text.ToString(), Int32.Parse(amountUsed.Text.ToString()));
                connectToCertificate(cert);
            }
        }

        // CONNECTION DATA TO DATA FIELDS
        void connectToCertificate(Core.Certificate in_cert)
        {
            int amountRedeemed = 0;
            int maxAmountUse = 0;

            if (in_cert.name != "DOES NOT EXIST") // WAS FOUND
            {
                if (in_cert.Redeem.Length == 0)
                {
                    if (in_cert.Amount == 0 || Int32.Parse(amountUsed.Text.ToString()) > in_cert.LastAmount)
                    {
                        resolveStateText.Text = "Certificate holds no more value or not enough. Refer below for more information.";
                    }
                    else
                    {
                        resolveStateText.Text = "Certificate has been successfully redeemed! Refer below for more information.";
                    }

                    maxAmountUse = in_cert.Redeem.Length;

                    redeemInfo.Text = maxAmountUse.ToString();
                    amountText.Text = "This has been redeemed " + in_cert.MoneyDates.Count.ToString() + " times on the dates below.";

                    if (in_cert.MoneyDates.Count > 0) // WHETHER TO DISPLAY REMPTION TIME STAMPS IF THEY EXIST
                    {
                        amountInfo.Text = ""; // RESETTING TIME STAMP FIELD

                        for (int i = 0; i < in_cert.MoneyDates.Count; i++) // DISPLAYING REDEMPTION TIME STAMPS
                        {
                            amountInfo.Text = amountInfo.Text.ToString() + (i + 1) + ")   " + Core.TrimDate(DateTime.Now) + "-(" + in_cert.MoneyDates[i].serviceRedeem + ": " + in_cert.MoneyDates[i].amountRedeem + ")\n";
                        }
                    }
                    else amountInfo.Text = "";
                }
                else
                {
                    resolveStateText.Text = (((DateTime.Now > in_cert.RedeemDates[in_cert.RedeemDates.Length - 1].AddSeconds(2)) && in_cert.RedeemDates[in_cert.RedeemDates.Length - 1] != new DateTime()) || DateTime.Now > in_cert.expDate)
                    ? "Certificate has been redeemed already or it is past the expiration date! Refer below for more information."
                    : "Certificate has been successfully redeemed! Refer below for more information.";

                    for (int i = 0; i < in_cert.Redeem.Length; i++) // HOW MANY HAVE BEEN REDEEMED
                    {
                        amountRedeemed += (in_cert.Redeem[i]) ? 1 : 0;
                    }
                    maxAmountUse = in_cert.Redeem.Length;

                    redeemInfo.Text = maxAmountUse.ToString();
                    amountText.Text = "This has been redeemed " + amountRedeemed.ToString() + " times on the dates below.";

                    if (amountRedeemed > 0) // WHETHER TO DISPLAY REMPTION TIME STAMPS IF THEY EXIST
                    {
                        amountInfo.Text = ""; // RESETTING TIME STAMP FIELD

                        for (int i = 0; i < amountRedeemed; i++) // DISPLAYING REDEMPTION TIME STAMPS
                        {
                            if (in_cert.RedeemDates[i] != new DateTime()) amountInfo.Text = amountInfo.Text.ToString() + (i + 1) + ")   " + Core.TrimDate(in_cert.RedeemDates[i]) + "\n";
                        }
                    }
                    else amountInfo.Text = "";
                }
                
                toName.Text = in_cert.name;
                fromName.Text = in_cert.fromName;
                date.Text = Core.TrimDate(in_cert.date);
                expDate.Text = Core.TrimDate(in_cert.expDate);
                serviceInfo.Text = in_cert.service;
                serviceAmountInfo.Text = "$" + in_cert.Amount.ToString();
            }
            else // WASN'T FOUND 
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

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                img.Source = new BitmapImage(new Uri(Core.root + @"graphics\TCS_back_img_1.png"));
            }
        }
    }
}
