using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing; 
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.VisualBasic.CompilerServices;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Diagnostics;
using PdfSharp.Drawing.Layout;
using System.CodeDom.Compiler;

namespace TCS
{
    public static class Core
    {
        //THE LAST DATE THIS APPLICATION WAS OPENED
        static DateTime lastDate;

        //CERTIFICATE DATA
        static int amountAllTime = 0;
        static int amountYear = 0;
        static int amountMonth = 0;
        static int amountRedeemed = 0;
        static int amountActive = 0;

        //CERTIFICATE EDITS
        static int expAmount = 0;
        static bool serviceDisp = false;
        static bool amountDisp = false;

        //CURRENT CERTIFICATES
        static List<string> ids = new List<string>();

        public static int getAmountAllTime()
        {
            return amountAllTime;
        }

        public static int getAmountYear()
        {
            return amountYear;
        }

        public static int getAmountMonth()
        {
            return amountMonth;
        }

        public static int getAmountRedeemed()
        {
            return amountRedeemed;
        }

        public static int getAmountActive()
        {
            return amountActive;
        }

        public static int getExpMonthAmount()
        {
            return expAmount;
        }

        public static bool getServiceDisp()
        {
            return serviceDisp;
        }

        public static bool getAmountDisp()
        {
            return amountDisp;
        }

        public static void generateFiles()
        {
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\settings.tcs");
            sw.WriteLine("AMOUNT OF MONTHS UNTIL EXPIRATION");
            sw.WriteLine(0);
            sw.WriteLine("DISPLAY SERVICE");
            sw.WriteLine(true);
            sw.WriteLine("DISPLAY AMOUNT");
            sw.WriteLine(true);
            sw.Close();
        }

        public static void initCore()
        {
            readMain();
            readIDs();
            readSettings();
        }

        public static void deCore(Settings in_settings)
        {
            writeMain();
            writeIDs();
            writeSettings(in_settings);
            //SendMail().Wait();
        }

        public static void createCert(string in_name, string in_fName, string in_email, string in_message, string in_service, double in_amount, int in_rAmount, DateTime in_date, Settings in_settings)
        {
            Certificate cert = new Certificate(in_name, in_fName, in_email, in_message, in_service, in_amount, in_rAmount, in_date, in_settings);
            //ids.Add(cert.id);

            //writeCert(cert);
            //createPDF(cert);

            amountAllTime += 1;
            amountYear += 1;
            amountMonth += 1;
            amountActive += 1;
        }

        public static Certificate lookupCert(string in_text, int in_case)
        {
            switch(in_case)
            {
                case 0:
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");
                        if (in_text == temp.name) return temp;
                    }
                    break;
                case 1:
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");
                        if (in_text == temp.fromName) return temp;
                    }
                    break;
                case 2:
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");
                        if (in_text == temp.email) return temp;
                    }
                    break;
                case 3:
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");
                        if (in_text == temp.code) return temp;
                    }
                    break;
                default:
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");
                        if (in_text == temp.id) return temp;
                    }
                    break;
            }
            return new Certificate();
        }

        public static Certificate redeemCert(string in_code)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                Certificate temp = new Certificate("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + ids[i] + ".tcs");

                if (in_code == temp.code)
                {
                    temp.redeemCert();
                    writeCert(temp);

                    return temp;
                }
            }
            return new Certificate();
        }

        static void readMain()
        {
            StreamReader sr = new StreamReader("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\main.tcs");

            while (!sr.EndOfStream)
            {
                switch (sr.ReadLine())
                {
                    case "AMOUNT OF CERTIFICATES CREATED--ALL TIME":
                        amountAllTime = Int32.Parse(sr.ReadLine());
                        break;
                    case "AMOUNT OF CERTIFICATES CREATED--THIS YEAR":
                        amountYear = Int32.Parse(sr.ReadLine());
                        break;
                    case "AMOUNT OF CERTIFICATES CREATED--THIS MONTH":
                        amountMonth = Int32.Parse(sr.ReadLine());
                        break;
                    case "AMOUNT OF CERTIFICATES REDEEMED":
                        amountRedeemed = Int32.Parse(sr.ReadLine());
                        break;
                    case "AMOUNT OF ACTIVE CERTIFICATES":
                        amountActive = Int32.Parse(sr.ReadLine());
                        break;
                }
            }
            sr.Close();
        }

        static void readIDs()
        {
            StreamReader sr = new StreamReader("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\ids.tcs");

            while (!sr.EndOfStream)
            {
                ids.Add(sr.ReadLine());
            }
            sr.Close();
        }

        static void readSettings()
        {
            StreamReader sr = new StreamReader("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\settings.tcs");

            while (!sr.EndOfStream)
            {
                switch (sr.ReadLine())
                {
                    case "AMOUNT OF MONTHS UNTIL EXPIRATION":
                        expAmount = Int32.Parse(sr.ReadLine());
                        break;
                    case "DISPLAY SERVICE":
                        serviceDisp = Boolean.Parse(sr.ReadLine());
                        break;
                    case "DISPLAY AMOUNT":
                        amountDisp = Boolean.Parse(sr.ReadLine());
                        break;
                }
            }
            sr.Close();
        }

        static void writeMain()
        {
            File.Delete(@"D:\VS Repos\Applications\TCS\TCS\docs\main.tcs");
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\main.tcs");

            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--ALL TIME");
            sw.WriteLine(amountAllTime);
            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--THIS YEAR");
            sw.WriteLine(amountYear);
            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--THIS MONTH");
            sw.WriteLine(amountMonth);
            sw.WriteLine("AMOUNT OF CERTIFICATES REDEEMED");
            sw.WriteLine(amountRedeemed);
            sw.WriteLine("AMOUNT OF ACTIVE CERTIFICATES");
            sw.WriteLine(amountActive);
            sw.Close();
        }

        static void writeIDs()
        {
            File.Delete(@"D:\VS Repos\Applications\TCS\TCS\docs\ids.tcs");
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\ids.tcs");

            for (int i = 0; i < ids.Count; i++)
            {
                sw.WriteLine(ids[i]);
            }
            sw.Close();
        }

        static void writeSettings(Settings in_settings)
        {
            File.Delete(@"D:\VS Repos\Applications\TCS\TCS\docs\settings.tcs");
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\settings.tcs");

            sw.WriteLine("AMOUNT OF MONTHS UNTIL EXPIRATION");
            sw.WriteLine(in_settings.getMonths());
            sw.WriteLine("DISPLAY SERVICE");
            sw.WriteLine(in_settings.getServiceDisp());
            sw.WriteLine("DISPLAY AMOUNT");
            sw.WriteLine(in_settings.getAmountDisp());
            sw.Close();
        }

        static void writeCert(Certificate in_cert)
        {
            if (File.Exists("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + in_cert.id + ".tcs"))
            {
                File.Delete("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + in_cert.id + ".tcs");
            }
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\certificates\\" + in_cert.id + ".tcs");

            sw.WriteLine(in_cert.id);
            sw.WriteLine(in_cert.name);
            sw.WriteLine(in_cert.fromName);
            sw.WriteLine(in_cert.email);
            sw.WriteLine(in_cert.message);
            sw.WriteLine(in_cert.service);
            sw.WriteLine(in_cert.amount);
            sw.WriteLine(in_cert.date);
            sw.WriteLine(in_cert.expDate);
            sw.WriteLine(in_cert.redeem.Length);
            for (int i = 0; i < in_cert.redeem.Length - 1; i++)
            {
                sw.WriteLine(in_cert.redeem[i]);
            }
            sw.WriteLine(in_cert.code);

            sw.Close();
        }

        static void createPDF(Certificate in_cert)
        {
            string tempDate = in_cert.date.ToString();
            string tempExpDate = in_cert.expDate.ToString();
            string tempCode = "";

            for (int i = 0; i < 11; i++)
            {
                tempDate = tempDate.Remove(tempDate.Length - 1);
                tempExpDate = tempExpDate.Remove(tempExpDate.Length - 1);

                if (i < 10)
                {
                    tempCode = tempCode + in_cert.code.ElementAt<char>(i).ToString() + "  ";
                }
                else if (i < 11)
                {
                    tempCode = tempCode + in_cert.code.ElementAt<char>(i).ToString();
                }
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Transformational Gift Certificate - " + in_cert.id;

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            XFont font_hugeBold = new XFont("Georgia", 35, XFontStyle.BoldItalic);
            XFont font_bigBold = new XFont("Georgia", 15, XFontStyle.BoldItalic);
            XFont font_big = new XFont("Georgia", 15, XFontStyle.Regular);
            XFont font_normal = new XFont("Georgia", 12, XFontStyle.Regular);
            XFont font_small = new XFont("Georgia", 10, XFontStyle.Regular);
            XBrush blueBrush = new XSolidBrush(XColor.FromArgb(79, 100, 157));
            XImage image = XImage.FromFile("D:\\VS Repos\\Applications\\TCS\\TCS\\graphics\\TCS_Gift_Certificate_Full.png");

            XRect dear_Rect = new XRect(85, 95, 400, 30);
            XRect from_Rect_1 = new XRect(445, 524, 160, 50);
            XRect msg_Rect = new XRect(65, 150, 480, 320);
            XRect to_Rect = new XRect(50, 644, 150, 20);
            XRect from_Rect_2 = new XRect(73, 677, 150, 20);
            XRect service_Rect = new XRect(90, 711, 150, 50);
            XRect isDate_Rect = new XRect(360, 647, 90, 20);
            XRect expDate_Rect = new XRect(392, 681, 90, 20);
            XRect redeem_Rect = new XRect(368, 710, 20, 20);
            XRect amount_Rect = new XRect(510, 583, 30, 30);
            XRect code_Rect = new XRect(100, 747, 250, 20);

            gfx.DrawImage(image, 0, 0);
            tf.DrawString(in_cert.name, font_big, blueBrush, dear_Rect);
            tf.DrawString(in_cert.fromName, font_big, blueBrush, from_Rect_1);
            tf.DrawString(in_cert.name, font_normal, blueBrush, to_Rect);
            tf.DrawString(in_cert.fromName, font_normal, blueBrush, from_Rect_2);
            tf.DrawString(in_cert.service, font_normal, blueBrush, service_Rect);
            tf.DrawString(tempDate, font_small, blueBrush, isDate_Rect);
            tf.DrawString(tempExpDate, font_small, blueBrush, expDate_Rect);
            tf.DrawString(in_cert.redeem.Length.ToString(), font_big, blueBrush, redeem_Rect);
            tf.DrawString("$" + in_cert.amount.ToString(), font_hugeBold, blueBrush, amount_Rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(in_cert.message, font_normal, blueBrush, msg_Rect);
            tf.DrawString(tempCode, font_bigBold, blueBrush, code_Rect);

            document.Save("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\pdfs\\4"/* + in_cert.id*/ + "_pdf.pdf");
        }

        static async Task SendMail()
        {
            var apiKey = EnvironmentData.SENDGRID_APIKEY;
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage();
            message.SetFrom(new EmailAddress("ceo@oustentertainment.com", "Contact Us"));
            message.AddTo(new EmailAddress("ceo@oustentertainment.com", "Contact Us"));
            message.SetSubject("TEST");
            message.AddContent(MimeType.Text, "Dear bla bla bla");

            var response = await client.SendEmailAsync(message);
        }

        public class Certificate
        {
            public string id;
            public string name;
            public string fromName;
            public string email;
            public string message;
            public string service;
            public string code;
            public double amount;
            public bool[] redeem;
            public DateTime date;
            public DateTime expDate;

            public Certificate()
            {
                name = "DOES NOT EXIST";
            }
            public Certificate(Certificate in_other)
            {
                id = in_other.id;
                name = in_other.name;
                fromName = in_other.fromName;
                email = in_other.email;
                message = in_other.message;
                service = in_other.service;
                amount = in_other.amount;
                redeem = in_other.redeem;
                date = in_other.date;
                date = in_other.expDate;

                generateCode();
            }
            public Certificate(string in_name, string in_fName, string in_email, string in_message, string in_service, double in_amount, int in_rAmount, DateTime in_date, Settings in_settings)
            {
                name = in_name;
                fromName = in_fName;
                email = in_email;
                message = in_message;
                service = in_service;
                amount = in_amount;
                date = in_date;
                expDate = date.AddMonths(in_settings.getMonths());
                redeem = new bool[in_rAmount];

                generateCode();
                generateID();
            }
            public Certificate(string in_path)
            {
                StreamReader sr = new StreamReader(in_path);
                string line = sr.ReadLine();

                for (int i = 0; i < 11; i++)
                {
                    switch (i)
                    {
                        case 0:
                            id = line;
                            break;
                        case 1:
                            name = line;
                            break;
                        case 2:
                            fromName = line;
                            break;
                        case 3:
                            email = line;
                            break;
                        case 4:
                            message = line;
                            break;
                        case 5:
                            service = line;
                            break;
                        case 6:
                            amount = Double.Parse(line);
                            break;
                        case 7:
                            date = DateTime.Parse(line);
                            break;
                        case 8:
                            expDate = DateTime.Parse(line);
                            break;
                        case 9:
                            int tempAmount = Int32.Parse(line);
                            redeem = new bool[tempAmount];

                            for (int j = 0; j < tempAmount - 1; j++)
                            {
                                line = sr.ReadLine();
                                redeem[j] = Boolean.Parse(line);
                            }
                            break;
                        case 10:
                            code = line;
                            break;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        
            public void redeemCert()
            {
                for (int i = 0; i < redeem.Length; i++)
                {
                    if (!redeem[i])
                    {
                        redeem[i] = true;
                        break;
                    }
                }
            }

            void generateID()
            {
                string tempID = name[0].ToString() + fromName[0].ToString() + email[0].ToString() + message[0].ToString() + service[2].ToString() + service[10].ToString() + redeem.Length.ToString();
                int multiple = 0;

                for (int i = 0; i < ids.Count; i++)
                {
                    if ((tempID + multiple.ToString()) == ids[i])
                    {
                        multiple++;
                    }
                }
                id = tempID + multiple.ToString();
            }

            void generateCode()
            {
                Random rand = new Random();

                for (int i = 0; i < 11; i++)
                {
                    if (i < 2 || (i > 4 && i < 8))
                    {
                        code = code + ((char)(rand.Next(26) + 65)).ToString();
                    }
                    else
                    {
                        code = code + rand.Next(10).ToString();
                    }
                }
            }
        
        }
    }
}
