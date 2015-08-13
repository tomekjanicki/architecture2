using System.Net.Mail;
using Architecture2.Common.Exception;
using Architecture2.Common.IoC;
using Architecture2.Common.Mail.Exception;
using Architecture2.Common.Mail.Interface;

namespace Architecture2.Common.Mail
{
    [RegisterType(Scope = RegisterTypeScope.Singleton)]
    public class MailService : IMailService
    {
        private readonly ExceptionConverter _exceptionConverter;
        public MailService()
        {
            var types = new[] { typeof(SmtpException) };
            _exceptionConverter = new ExceptionConverter(types, exception => new MailServiceException(exception));
        }

        public void Send(MailMessage message)
        {
            _exceptionConverter.HandleAction(() =>
            {
                using (var client = new SmtpClient())
                    client.Send(message);
            });
        }
    }
}
