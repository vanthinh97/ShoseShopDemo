using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace AptechShoseShop.Models
{
    public class EmailManagement
    {
        public static void SendMail(string email, string subject, string body)
        {
            string emailServer = "thinhdevelopertest@gmail.com";
            string password = "12345678Th@";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailServer);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = body;

            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;


            //Create SMTP for send mail // 
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(emailServer, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);


        }
    }
}