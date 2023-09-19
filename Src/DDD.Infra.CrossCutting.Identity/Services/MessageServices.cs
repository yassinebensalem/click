using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DDD.Infra.CrossCutting.Identity.Services
{
    public class AuthEmailMessageSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage, string displayName = "Clik2Read Confirmation")
        {
            string fromMail = "noreply@clik2read.com";// "miplivrelstore@gmail.com";
            string fromPassword = "NoreplyClik2read@2022";// "gumowyblzwgvqqaq";


            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail, displayName);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(fromMail, fromPassword);
                client.Host = "ssl0.ovh.net";// "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(message);
            }
        }

        public async Task SendEmailAsync2(string SenderEmail, string subject, string htmlMessage)
        {
            string fromMail = "noreply@clik2read.com";// "miplivrelstore@gmail.com";
            string fromPassword = "NoreplyClik2read@2022";// "gumowyblzwgvqqaq";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail, "Clik2Read Confirmation");
            message.Subject = subject;
            message.To.Add(new MailAddress("contact@clik2read.com"/*"miplivrelstore@gmail.com"*/));
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(fromMail, fromPassword);
                client.Host = "ssl0.ovh.net"; /*"smtp.gmail.com";*/
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(message);
            }
        }
    }

    public class AuthSMSMessageSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
