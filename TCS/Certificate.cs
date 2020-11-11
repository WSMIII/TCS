using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using System.IO;
using System.CodeDom.Compiler;

namespace TCS
{
    public class Certificate
    {
        int id;
        readonly string name;
        readonly string fromName;
        readonly string email;
        readonly string message;
        readonly string service;
        string code;
        readonly double amount;
        bool[] redeem;
        DateTime date;
        DateTime expDate;

        public Certificate(Certificate in_other)
        {
            id = in_other.getID();
            name = new string(in_other.getName());
            fromName = new string(in_other.getFrom());
            email = new string(in_other.getEmail());
            message = new string(in_other.getMessage());
            service = new string(in_other.getService());
            amount = in_other.getAmount();
            redeem = in_other.getRedeemed();
            date = in_other.getDate();
            date = in_other.getExpDate();

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
                switch(i)
                {
                    case 0:
                        id = Int32.Parse(line);
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

                        for (int j = 0; j < tempAmount-1; j++)
                        {
                            line = sr.ReadLine();
                            redeem[i] = Boolean.Parse(line);
                        }
                        break;
                    case 10:
                        code = line;
                        break;
                }
                line = sr.ReadLine();
            }
        }

        public int getID()
        {
            return id;
        }
        public string getName()
        {
            return name;
        }
        public string getFrom()
        {
            return fromName;
        }
        public string getEmail()
        {
            return email;
        }
        public string getMessage()
        {
            return message;
        }
        public string getService()
        {
            return service;
        }
        public string getCode()
        {
            string temp = code;

            return temp;
        }
        public double getAmount()
        {
            return amount;
        }
        public bool[] getRedeemed()
        {
            bool[] temp = new bool[redeem.Length];

            for (int i = 0; i < redeem.Length - 1; i++)
            {
                temp[i] = redeem[i];
            }

            return temp;
        }
        public DateTime getDate()
        {
            DateTime temp = date;

            return temp;
        }
        public DateTime getExpDate()
        {
            DateTime temp = expDate;

            return temp;
        }

        public bool redeemCode(string in_code)
        {
            if (in_code == code) return true;

            return false;
        }

        public static void generateFiles()
        {
            StreamWriter sw = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\main.tcs");
            StreamWriter sw_1 = new StreamWriter("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\ids.tcs");

            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--ALL TIME");
            sw.WriteLine(0);
            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--THIS YEAR");
            sw.WriteLine(0);
            sw.WriteLine("AMOUNT OF CERTIFICATES CREATED--THIS MONTH");
            sw.WriteLine(0);
            sw.WriteLine("AMOUNT OF CERTIFICATES REDEEMED");
            sw.WriteLine(0);
            sw.WriteLine("AMOUNT OF ACTIVE CERTIFICATES");
            sw.WriteLine(0);

            sw_1.WriteLine("");

            sw.Close();
            sw_1.Close();
        }

        void generateID()
        {
            StreamReader sr = new StreamReader("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\main.tcs");
            StreamReader sr_1 = new StreamReader("D:\\VS Repos\\Applications\\TCS\\TCS\\docs\\ids.tcs");
            Random rand = new Random();
            string line = sr.ReadLine();
            bool match = false;

            id = rand.Next(1000000);

            while (line != "AMOUNT OF CERTIFICATES CREATED--ALL TIME")
            {
                line = sr.ReadLine();
            }
            line = sr.ReadLine();
            int aTCerts = Int32.Parse(line);

            if (aTCerts != 0)
            {
                int[] ids = new int[aTCerts];
                string tempLine = sr_1.ReadLine();

                for (int i = 0; i < aTCerts; i++)
                {
                    ids[i] = Int32.Parse(tempLine);
                    tempLine = sr_1.ReadLine();
                }

                do
                {
                    match = true;
                    for (int i = 0; i <= aTCerts; i++)
                    {
                        if (id == ids[i])
                        {
                            id = rand.Next(1000000);
                            match = false;
                            break;
                        }
                    }
                }
                while (!match);
            }

            sr.Close();
            sr_1.Close();
        }

        void generateCode()
        {
            Random rand = new Random();

            for (int i = 0; i < 11; i++)
            {
                if (i < 2 || (i > 4 && i < 8))
                {
                    code = code + (char)(rand.Next(26) + 65);
                }
                else
                {
                    code = code + (char)rand.Next(10);
                }
            }
        }
    }
}
