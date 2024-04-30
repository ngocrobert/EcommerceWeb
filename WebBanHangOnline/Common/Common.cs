﻿using System.Net.Mail;
using System.Net;


namespace WebBanHangOnline.Common
{
    public class Common
    {


        //private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        //private static string Email = ConfigurationManager.AppSettings["Email"];
        //private readonly IConfiguration _configuration;

        //public Common(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        //public static bool SendMail(string name, string subject, string content,
        //    string toMail, IConfiguration _configuration)
        //{
        //    bool rs = false;
        //    try
        //    {
        //        string password = _configuration["AppSettings:PasswordEmail"];
        //        string Email = _configuration["AppSettings:Email"];

        //        MailMessage message = new MailMessage();
        //        var smtp = new SmtpClient();
        //        {
        //            smtp.Host = "smtp.gmail.com"; //host name
        //            smtp.Port = 587; //port number
        //            smtp.EnableSsl = true; //whether your smtp server requires SSL
        //            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = new NetworkCredential()
        //            {
        //                UserName = Email,
        //                Password = password
        //            };
        //        }
        //        MailAddress fromAddress = new MailAddress(Email, name);
        //        message.From = fromAddress;
        //        message.To.Add(toMail);
        //        message.Subject = subject;
        //        message.IsBodyHtml = true;
        //        message.Body = content;
        //        smtp.Send(message);
        //        rs = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        rs = false;
        //    }
        //    return rs;
        //}

        //private IHostingEnvironment Environment { get; set; }
        //public IConfiguration Configuration { get; set; }

        public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                thapPhan += "#";
            }
            if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumformat = string.Format("0:#,##0{0}", thapPhan);
            str = String.Format("{" + snumformat + "}", GT);

            return str;
        }
        private static bool IsNumeric(object value)
        {
            return value is sbyte
                       || value is byte
                       || value is short
                       || value is ushort
                       || value is int
                       || value is uint
                       || value is long
                       || value is ulong
                       || value is float
                       || value is double
                       || value is decimal;
        }

        public static string HtmlRate(int rate)
        {
            var str = "";
            if(rate==1)
            {
                str += @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
            }
            if (rate == 2)
            {
                str += @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
            }
            if (rate == 3)
            {
                str += @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
            }
            if (rate == 4)
            {
                str += @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
            }
            if (rate == 5)
            {
                str += @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>
             <li><i class='fa fa-star' aria-hidden='true'></i></li>";
            }
            return str;
          
        }
    }
}
