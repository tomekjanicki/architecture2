using System.Net.Mail;

namespace Architecture2.Common.Mail.Interface
{
    public interface IMailService
    {
        void Send(MailMessage message);
    }
}
