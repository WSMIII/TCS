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
using System.Runtime.CompilerServices;

namespace TCS
{
    public static class Core
    {
        public static int curCert = 0;
        public static List<Certificate> certsFound = new List<Certificate>();

        // APPLICATION DATA
        public static string root = @"D:\VS Repos\Applications\TCS\TCS\";

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
        static List<EmailInfo> emailQueue = new List<EmailInfo>();

        public static int AmountAllTime { get { return amountAllTime; } }
        public static int AmountYear { get { return amountYear; } }
        public static int AmountMonth { get { return amountMonth; } }
        public static int AmountRedeemed { get { return amountRedeemed; } }
        public static int AmountActive { get { return amountActive; } }
        public static int ExpMonthAmount { get { return expAmount; } }
        public static bool ServiceDisp { get { return serviceDisp; } }
        public static bool AmountDisp { get { return amountDisp; } }

        // CONSTRUCTOR
        public static void initCore()
        {
            readMain();
            readIDs();
            readSettings();

            if (DateTime.Now.Month > lastDate.Month) amountMonth = 0; // RESETTING EVERY NEW MONTH
            else if (DateTime.Now.Year > lastDate.Year) amountYear = 0; // RESETTING EVERY NEW YEAR
        }

        // DECONSTRUCTOR
        public static void deCore(Settings in_settings)
        {
            writeMain();
            writeIDs();
            writeSettings(in_settings);

            for (int i = 0; i < emailQueue.Count; i++)
            {
                //SendMail(emailQueue[i]).Wait();
            }
        }

        // FEATURES FOR APPLICATION
        public static void createCert(string in_name, string in_fName, string in_toEmail, string in_fromEmail, string in_message, string in_service, double in_amount, int in_rAmount, DateTime in_date, Settings in_settings)
        {
            Certificate cert = new Certificate(in_name, in_fName, in_toEmail, in_fromEmail, in_message, in_service, in_amount, in_rAmount, in_date, in_settings);
            ids.Add(cert.ID);

            writeCert(cert);
            createPDF(cert, in_settings);

            amountAllTime += 1;
            amountYear += 1;
            amountMonth += 1;
            amountActive += 1;
        }

        public static Certificate lookupCert(string in_text, int in_case)
        {
            certsFound.RemoveAll(CertsFoundPredicate);
            curCert = 0;

            string textMatch = "";

            for (int i = 0; i < ids.Count; i++)
            {
                Certificate temp = new Certificate(root + @"docs\certificates\" + ids[i] + ".tcs");

                switch (in_case)
                {
                    case 0:
                        textMatch = temp.name.ToLower();
                        break;
                    case 1:
                        textMatch = temp.fromName.ToLower();
                        break;
                    case 2:
                        textMatch = temp.toEmail.ToLower();
                        break;
                    case 3:
                        textMatch = temp.fromEmail.ToLower();
                        break;
                    case 4:
                        textMatch = temp.Code;
                        break;
                    default:
                        textMatch = temp.ID;
                        break;
                }

                if (in_text.Length >= 5)
                {
                    if (textMatch.Contains(in_text.Remove(in_text.Length - 2))) certsFound.Add(temp);
                }
                else
                {
                    if (textMatch.Contains(in_text) || in_text == textMatch) certsFound.Add(temp);
                }
            }
            if (certsFound.Count > 0) return certsFound[curCert];

            return new Certificate();
        }

        public static Certificate redeemCert(string in_code, string in_service, int in_amount)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                Certificate temp = new Certificate(root + @"docs\certificates\" + ids[i] + ".tcs");

                if (in_code == temp.Code)
                {
                    if (temp.Redeem.Length == 0)
                    {
                        amountRedeemed++;

                        temp.redeemCert(in_service, in_amount);
                        writeCert(temp);

                        return temp;
                    }
                    else
                    {
                        if (!temp.redemptionState()) amountRedeemed++;

                        temp.redeemCert();
                        writeCert(temp);

                        return temp;
                    }
                }
            }
            return new Certificate();
        }

        // IMPORTANT STATIC FUNCTIONS.
        public static void queueEmail(string in_toName, string in_fromName, string in_toEmail, string in_fromEmail)
        {
            EmailInfo info = new EmailInfo(ids[ids.Count - 1], in_toName, in_fromName, in_toEmail, in_fromEmail);
            emailQueue.Add(info);
        }

        public static string TrimDate(DateTime in_date)
        {
            string date = in_date.ToString();

            for (int i = 0; i < 11; i++)
            {
                date = date.Remove(date.Length - 1);
            }
            return date;
        }

        public static string FluffCode(string in_code)
        {
            string code = "";

            for (int i = 0; i < 11; i++)
            {
                code = (i < 10) ? code + in_code.ElementAt<char>(i).ToString() + "  " : code + in_code.ElementAt<char>(i).ToString();
            }
            return code;
        }

        public static bool CertsFoundPredicate(Certificate in_cert)
        {
            return true;
        }

        // PRIVATE FUNCTIONS
        static void readMain()
        {
            StreamReader sr = new StreamReader(root + @"docs\main.tcs");

            while (!sr.EndOfStream)
            {
                switch (sr.ReadLine())
                {
                    case "LAST ACCESS DATE":
                        lastDate = DateTime.Parse(sr.ReadLine());
                        break;
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
            StreamReader sr = new StreamReader(root + @"docs\ids.tcs");

            while (!sr.EndOfStream)
            {
                ids.Add(sr.ReadLine());
            }
            sr.Close();
        }

        static void readSettings()
        {
            StreamReader sr = new StreamReader(root + @"docs\settings.tcs");

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
            File.Delete(root + @"docs\main.tcs");

            StreamWriter sw = new StreamWriter(root + @"docs\main.tcs");

            sw.WriteLine("LAST ACCESS DATE");
            sw.WriteLine(DateTime.Now);
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
            File.Delete(root + @"docs\ids.tcs");

            StreamWriter sw = new StreamWriter(root + @"docs\ids.tcs");

            for (int i = 0; i < ids.Count; i++)
            {
                sw.WriteLine(ids[i]);
            }
            sw.Close();
        }

        static void writeSettings(Settings in_settings)
        {
            File.Delete(root + @"docs\settings.tcs");

            StreamWriter sw = new StreamWriter(root + @"docs\settings.tcs");

            sw.WriteLine("AMOUNT OF MONTHS UNTIL EXPIRATION");
            sw.WriteLine(in_settings.Months);
            sw.WriteLine("DISPLAY SERVICE");
            sw.WriteLine(in_settings.ServiceDisp);
            sw.WriteLine("DISPLAY AMOUNT");
            sw.WriteLine(in_settings.AmountDisp);
            sw.Close();
        }

        static void writeCert(Certificate in_cert)
        {
            if (File.Exists(root + @"docs\certificates\" + in_cert.ID + ".tcs")) File.Delete(root + @"docs\certificates\" + in_cert.ID + ".tcs");
            
            StreamWriter sw = new StreamWriter(root + @"docs\certificates\" + in_cert.ID + ".tcs");

            sw.WriteLine(in_cert.ID);
            sw.WriteLine(in_cert.name);
            sw.WriteLine(in_cert.fromName);
            sw.WriteLine(in_cert.toEmail);
            sw.WriteLine(in_cert.fromEmail);
            sw.WriteLine(in_cert.message);
            sw.WriteLine(in_cert.service);
            sw.WriteLine(in_cert.Amount);
            sw.WriteLine(in_cert.date);
            sw.WriteLine(in_cert.expDate);
            sw.WriteLine(in_cert.Redeem.Length);
            for (int i = 0; i < in_cert.Redeem.Length; i++)
            {
                sw.WriteLine(in_cert.Redeem[i]);
                sw.WriteLine(in_cert.RedeemDates[i]);
            }
            sw.WriteLine(in_cert.MoneyDates.Count);
            for (int i = 0; i < in_cert.MoneyDates.Count; i++)
            {
                sw.WriteLine(in_cert.MoneyDates[i].serviceRedeem);
                sw.WriteLine(in_cert.MoneyDates[i].amountRedeem);
            }
            sw.WriteLine(in_cert.Code);

            sw.Close();
        }

        static void createPDF(Certificate in_cert, Settings in_settings)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Transformational Gift Certificate - " + in_cert.ID;

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            XFont font_hugeBold = new XFont("Georgia", 35, XFontStyle.BoldItalic);
            XFont font_bigBold = new XFont("Georgia", 15, XFontStyle.BoldItalic);
            XFont font_big = new XFont("Georgia", 15, XFontStyle.Regular);
            XFont font_normal = new XFont("Georgia", 12, XFontStyle.Regular);
            XFont font_small = new XFont("Georgia", 10, XFontStyle.Regular);
            XBrush blueBrush = new XSolidBrush(XColor.FromArgb(79, 100, 157));
            XImage image = (in_settings.ServiceDisp && in_cert.service != "$$$") ? XImage.FromFile(root + @"graphics\TCS_Gift_Certificate_Full.png") : XImage.FromFile(root + @"graphics\TCS_Gift_Certificate_No_Service.png");

            XRect dear_Rect = new XRect(85, 95, 400, 30);
            XRect from_Rect_1 = new XRect(445, 524, 160, 50);
            XRect msg_Rect = new XRect(65, 150, 480, 320);
            XRect to_Rect = new XRect(50, 644, 150, 20);
            XRect from_Rect_2 = new XRect(73, 677, 150, 20);
            XRect service_Rect = new XRect(90, 711, 150, 50);
            XRect isDate_Rect = new XRect(360, 647, 90, 20);
            XRect expDate_Rect = new XRect(392, 681, 90, 20);
            XRect redeem_Rect = new XRect(361, 710, 20, 20);
            XRect amount_Rect = new XRect(510, 583, 30, 30);
            XRect code_Rect = new XRect(100, 747, 250, 20);

            string tempDate = TrimDate(in_cert.date);
            string tempExpDate = TrimDate(in_cert.expDate);
            string tempCode = FluffCode(in_cert.Code);

            gfx.DrawImage(image, 0, 0);
            tf.DrawString(in_cert.name, font_big, blueBrush, dear_Rect);
            tf.DrawString(in_cert.fromName, font_big, blueBrush, from_Rect_1);
            tf.DrawString(in_cert.name, font_normal, blueBrush, to_Rect);
            tf.DrawString(in_cert.fromName, font_normal, blueBrush, from_Rect_2);
            tf.DrawString(tempDate, font_small, blueBrush, isDate_Rect);
            tf.DrawString(tempExpDate, font_small, blueBrush, expDate_Rect);
            if (in_settings.ServiceDisp && in_cert.service != "$$$")
            {
                tf.DrawString(in_cert.service, font_normal, blueBrush, service_Rect);
                if (in_cert.Redeem.Length.ToString() == "1000")
                {
                    tf.DrawString("INF", font_big, blueBrush, redeem_Rect);
                }
                else
                {
                    tf.DrawString(in_cert.Redeem.Length.ToString(), font_big, blueBrush, redeem_Rect);
                }
            }
            if (in_settings.AmountDisp || in_cert.service == "$$$") tf.DrawString("$" + in_cert.Amount.ToString(), font_hugeBold, blueBrush, amount_Rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(in_cert.message, font_normal, blueBrush, msg_Rect);
            tf.DrawString(tempCode, font_bigBold, blueBrush, code_Rect);
            
            document.Save(root + @"docs\pdfs\" + in_cert.ID + "_pdf.pdf");
        }

        static async Task SendMail(EmailInfo in_info)
        {
            var apiKey = EnvironmentData.SENDGRID_APIKEY;
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage();

            // CREATING EMAIL MESSAGE
            message.SetFrom(new EmailAddress("deb@debrataubenslag.com", "Debra Taubenslag"));
            message.AddTo(new EmailAddress(in_info.toEmail, in_info.toName));
            message.AddCc(new EmailAddress(in_info.fromEmail, in_info.fromName));
            message.AddCc(new EmailAddress("deb@debrataubenslag.com", "Debra Taubenslag"));
            message.SetSubject("A Transformational Gift From " + in_info.fromName);
            message.AddContent(MimeType.Html, "<p style=\"font-family:'Georgia';color:#4f649d\"><strong style=\"font-size:25px\">Surprise " + in_info.toName + "!!</strong><br />" + in_info.fromName + " has sent you a Gift Certificate for the transformational healing services of <strong><i>Debra Taubenslag</i></strong>.<br />Be sure to download and save the attached certificate for future use.<br />To learn more about <strong><i>Debra Taubenslag</i></strong> and what you can use this certificate for please go to <a href=\"https://www.debrataubenslag.com\">debrataubenslag.com.</a><br /><br />Yours Truly,<br /><i style=\"font-size:25px\">Debra Taubenslag</i><br /><i>The Transformational Mentor</i></p>");
            message.AddAttachment("TCS_Gift_Certificate_" + in_info.id + ".pdf", Convert.ToBase64String(File.ReadAllBytes(root + @"docs\pdfs\" + in_info.id + "_pdf.pdf")));

            var response = await client.SendEmailAsync(message).ConfigureAwait(false);
        }

        // PUBLIC CUSTOM DATA TYPE
        public class Certificate
        {
            public struct MoneyRedeem
            {
                public string serviceRedeem { get; }
                public int amountRedeem { get; }

                public MoneyRedeem(string in_serviceR, int in_amountR)
                {
                    serviceRedeem = in_serviceR;
                    amountRedeem = in_amountR;
                }
            }

            // PRIVATE VARIABLES
            string id;
            string code;
            double amount;
            double lastAmount;
            bool[] redeem;
            DateTime[] redeemDates;
            List<MoneyRedeem> moneyDates = new List<MoneyRedeem>();

            // PUBLIC VARIABLES
            public string ID 
            { 
                get { return id; }
            }
            public string name { get; }
            public string fromName { get; }
            public string toEmail { get; }
            public string fromEmail { get; }
            public string message { get; }
            public string service { get; }
            public string Code 
            { 
                get { return code; }
            }
            public double Amount { get { return amount; } }
            public double LastAmount { get { return lastAmount; } }
            public bool[] Redeem { get { return redeem; } }
            public DateTime date { get; }
            public DateTime expDate { get; }
            public DateTime[] RedeemDates { get { return redeemDates; } }
            public List<MoneyRedeem> MoneyDates { get { return moneyDates; } }

            // CONSTRUCTORS
            public Certificate()
            {
                name = "DOES NOT EXIST";
            }
            public Certificate(Certificate in_other)
            {
                id = in_other.id;
                name = in_other.name;
                fromName = in_other.fromName;
                toEmail = in_other.toEmail;
                fromEmail = in_other.fromEmail;
                message = in_other.message;
                service = in_other.service;
                amount = in_other.amount;
                lastAmount = in_other.amount;
                redeem = in_other.redeem;
                date = in_other.date;
                date = in_other.expDate;
                redeemDates = in_other.redeemDates;
                moneyDates = in_other.moneyDates;

                generateCode();
            }
            public Certificate(string in_name, string in_fName, string in_toEmail, string in_fromEmail, string in_message, string in_service, double in_amount, int in_rAmount, DateTime in_date, Settings in_settings)
            {
                name = in_name;
                fromName = in_fName;
                toEmail = in_toEmail;
                fromEmail = in_fromEmail;
                message = in_message;
                service = in_service;
                amount = in_amount;
                lastAmount = amount;
                date = in_date;
                expDate = date.AddMonths(in_settings.Months);
                redeem = new bool[in_rAmount];
                redeemDates = new DateTime[in_rAmount];

                generateCode();
                generateID();
            }
            public Certificate(string in_path)
            {
                StreamReader sr = new StreamReader(in_path);
                string line = sr.ReadLine();

                for (int i = 0; i < 13; i++)
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
                            toEmail = line;
                            break;
                        case 4:
                            fromEmail = line;
                            break;
                        case 5:
                            message = line;
                            break;
                        case 6:
                            service = line;
                            break;
                        case 7:
                            amount = Double.Parse(line);
                            lastAmount = amount;
                            break;
                        case 8:
                            date = DateTime.Parse(line);
                            break;
                        case 9:
                            expDate = DateTime.Parse(line);
                            break;
                        case 10:
                            int tempAmount = Int32.Parse(line);
                            redeem = new bool[tempAmount];
                            redeemDates = new DateTime[tempAmount];

                            for (int j = 0; j < tempAmount; j++)
                            {
                                line = sr.ReadLine();
                                redeem[j] = Boolean.Parse(line);
                                line = sr.ReadLine();
                                redeemDates[j] = DateTime.Parse(line);
                            }
                            break;
                        case 11:
                            int moneyRedeemSize = Int32.Parse(line);

                            for (int k = 0; k < moneyRedeemSize; k++)
                            {
                                moneyDates.Add(new MoneyRedeem(sr.ReadLine(), Int32.Parse(sr.ReadLine())));
                            }
                            break;
                        case 12:
                            code = line;
                            break;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        
            // PUBLIC FUNCTIONS
            public void redeemCert()
            {
                if (!(DateTime.Now > expDate))
                {
                    for (int i = 0; i < redeem.Length; i++)
                    {
                        if (!redeem[i])
                        {
                            redeem[i] = true;
                            redeemDates[i] = DateTime.Now;
                            break;
                        }
                    }
                }
            }

            public void redeemCert(string in_service, int in_amount)
            {
                if (amount > in_amount)
                {
                    lastAmount = amount;
                    amount -= in_amount;
                    moneyDates.Add(new MoneyRedeem(in_service, in_amount));
                }
            }

            public bool redemptionState()
            {
                if (DateTime.Now > expDate) return true;

                for (int i = 0; i < Redeem.Length; i++)
                {
                    if (Redeem[i] == true) return true;
                }
                return false;
            }

            // PRIVATE FUNCTIONS
            void generateID()
            {
                string tempID = name[0].ToString() + fromName[0].ToString() + toEmail[0].ToString() + fromEmail[0].ToString() + message[0].ToString() + service[2].ToString() + service[service.Length - 1].ToString() + redeem.Length.ToString();
                int multiple = 0;

                for (int i = 0; i < ids.Count; i++)
                {
                    multiple += ((tempID + multiple.ToString()) == id) ? 1 : 0;
                }
                id = tempID + multiple.ToString();
            }

            void generateCode()
            {
                Random rand = new Random();
                bool uniqueCode = false;

                while (!uniqueCode)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        code = (i < 2 || (i > 4 && i < 8)) ? (code + ((char)(rand.Next(26) + 65)).ToString()) : (code + rand.Next(10).ToString());                        
                    }

                    if (ids.Count == 0)
                    {
                        uniqueCode = true;
                    }
                    else
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            Certificate temp = new Certificate(root + @"docs\certificates\" + ids[i] + ".tcs");

                            if (code == temp.code)
                            {
                                uniqueCode = false;
                                break;
                            }
                            uniqueCode = true;
                        }
                    }
                }
            }        
        }

        // PRIVATE CUSTOM DATA TYPE
        struct EmailInfo // NECCESSARY INFORMATION TO SEND A EMAIL IN A DATA CONTAINER
        {
            // PUBLIC VARIABLES
            public string id { get; }
            public string toName { get; }
            public string fromName { get; }
            public string toEmail { get; }
            public string fromEmail { get; }

            // CONSTRUCTORS
            public EmailInfo(string in_id, string in_toName, string in_fromName, string in_toEmail, string in_fromEmail)
            {
                id = in_id;
                toName = in_toName;
                fromName = in_fromName;
                toEmail = in_toEmail;
                fromEmail = in_fromEmail;
            }
        }
    }
}
