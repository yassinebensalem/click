using System.Threading.Tasks;

namespace DDD.Infra.CrossCutting.Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string displayName = "Clik2Read Confirmation");
        Task SendEmailAsync2(string SenderEmail, string subject, string htmlMessage);
    }
}
