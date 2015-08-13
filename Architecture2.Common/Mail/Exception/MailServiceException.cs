using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Base;

namespace Architecture2.Common.Mail.Exception
{
    [Serializable]
    public class MailServiceException : BaseException
    {
        public MailServiceException(System.Exception innerException)
            : base("Error during sending mail. See inner exception for details", innerException)
        {
        }

        public MailServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
