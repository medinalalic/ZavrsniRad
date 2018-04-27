using System;
using System.Net.Mail;

namespace ZavršniRad.Email
{
    public class SendEmail
    {
        public bool SendMail(string email,string body)
        {
            string emailSender = "omer_avdic@hotmail.com";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailSender);
            mailMessage.To.Add(email);
            mailMessage.Subject = "Izmjena lozinke";
            mailMessage.Body = body;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("omer_avdic@hotmail.com","benzema!");// Enter seders User name and password
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return true;
            }
          
        }
    }
}