using System.Net.Mail;
using System.Net;
using HelloJobBackEnd.Services.Interface;

namespace HelloJobBackEnd.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string recipientEmail, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("hellojob440@gmail.com", "HelloJOB");
                mail.To.Add(new MailAddress(recipientEmail));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");

                    smtp.Send(mail);
                }
            }
        }
    }
}
