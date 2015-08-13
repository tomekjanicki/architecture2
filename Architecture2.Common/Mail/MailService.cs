using System;
using System.Net.Mail;
using Architecture2.Common.IoC;
using Architecture2.Common.Mail.Interface;

namespace Architecture2.Common.Mail
{
    [RegisterType(Scope = RegisterTypeScope.Singleton)]
    public class MailService : IMailService
    {
        public void Send(MailMessage message)
        {
            //todo wrap exception
            throw new NotImplementedException();
        }
    }
}
