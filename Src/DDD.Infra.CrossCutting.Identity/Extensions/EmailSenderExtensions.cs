using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DDD.Application.ViewModels;
using DDD.Infra.CrossCutting.Identity.Services;

namespace DDD.Infra.CrossCutting.Identity.Extensions
{
    public static class EmailSenderExtensions
    {
        private static readonly string _SIGNATURE = "<br><br>Best regards<br><br>Made with love for e-books lovers<br>Click2read administrator";
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, string password = "")
        {
            string body = "<html><body> " +
                $"Please confirm your account by clicking this : <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>";
            if (!string.IsNullOrWhiteSpace(password))
            {
                body += " <br> Your current password is: " + password;
            }
            body += " </body></html>";
            return emailSender.SendEmailAsync(email, "Confirm your email", body);
        }
        public static Task SendEmailConfirmationAndJoinCommunityAsync(this IEmailSender emailSender, string communityManagerName, string email,
            string communityName, string link, string password = "")
        {
            string body = "<html><body> " +
                $"Dear book passionate, <br><br>" + 
                $"You have been invited by {communityManagerName} to join the Click2read platform as a privileged member of the community : {communityName}. <br><br>" +
                $"This membership will grant you special promotions and discounts among our selection of e-books inside the platform.<br><br>" +
                $"Please confirm your account and join our community by clicking on this <a href='{HtmlEncoder.Default.Encode(link)}'><b>link</b></a>.<br>";

            if (!string.IsNullOrWhiteSpace(password))
            {
                body += " <br> Your current password is: " + password;
            }
            body += _SIGNATURE;
            body += " </body></html>";
            string subject = $"Confirm your email and join our community {communityName}";
            return emailSender.SendEmailAsync(email, subject, body, subject);
        }

        //Join Request Email
        public static Task SendContractByEmailAsync(this IEmailSender emailSender, string email, string _mailText)
        {
            return emailSender.SendEmailAsync(email, "Signature du Contrat", _mailText);
        }

        public static Task SendChangePasswordviaEmailAsync(this IEmailSender emailSender, string email, string _mailText)
        {
            return emailSender.SendEmailAsync(email, "Changement de Mot de Passe", _mailText);
        }

        public static Task SendContactEmailAsync(this IEmailSender emailSender, string FromEmail,string FirstName, string LastName, string subject, string BodyContent)
        {
            string body = "<html><body>"
                + "\n  Nom de l'expéditeur : " + $"{FirstName} {LastName}"
                +"\n E-mail : " + FromEmail
                +"\n Sujet : " + subject
                + "\n Contenu du message : \n " + BodyContent
                + "\n  </body></html>";

            return emailSender.SendEmailAsync2(FromEmail, subject, body);
        }
        public static Task SendCompetitionEmailAsync(this IEmailSender emailSender, string FromEmail, string VoucherNumber, double? VoucherValue, string subject, string Description)
        {
            string body = "<html><body>" +
               "<br/> E-mail : " + FromEmail
               + "<br/> Sujet : " + subject
               + "<br/> Numéro du bon d'achat : " + VoucherNumber
               + "<br/> Valeur du bon d'achat : " + VoucherValue
               + "<br/> Liste des livres désirée : " + Description
                 + "  </body></html>";
            return emailSender.SendEmailAsync2(FromEmail, subject, body);
        }
        public static Task SendMothersDayEmailAsync(this IEmailSender emailSender, string FromEmail, string ReceiverEmail, string subject, string Description)
        {
            string body = "<html><body>"
              + "<br/> E-mail de l'offrant: " + FromEmail
               +"<br/> E-mail du detinataire: " + ReceiverEmail
               + "<br/> Sujet : " + subject
               + "<br/> Le livre à offrir : " + Description
                 + "  </body></html>";
            return emailSender.SendEmailAsync2(FromEmail, subject, body);
        }
        public static Task SendLaureatEmailAsync(this IEmailSender emailSender, string FromEmail, string VoucherNumber, double? VoucherValue, string subject, string Description)
        {
            string body = "<html><body>" +
               "<br/> E-mail : " + FromEmail
               + "<br/> Sujet : " + subject
               + "<br/> Valeur du bon d'achat : " + VoucherValue
               + "<br/> Liste des livres désirée : " + Description
                 + "  </body></html>";
            return emailSender.SendEmailAsync2(FromEmail, subject, body);
        }
    }
}
