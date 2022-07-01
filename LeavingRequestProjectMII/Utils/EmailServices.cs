using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Utils
{
    public class EmailServices
    {
        public static void SendEmail(string emailTo, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.ethereal.email");

            mail.From = new MailAddress(VariableServices.EMAIL);
            mail.To.Add(emailTo);
            mail.Subject = subject;

            mail.IsBodyHtml = true;
            mail.Body = body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(VariableServices.EMAIL, VariableServices.PASSWORD);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            

        }
    }
}
