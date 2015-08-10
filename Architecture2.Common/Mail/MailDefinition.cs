using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;

namespace Architecture2.Common.Mail
{
    [Serializable]
    public class MailDefinition
    {
        public IReadOnlyCollection<string> Recipients { get; set; }
        public IReadOnlyCollection<string> CcRecipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string From { get; set; }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public MailMessage ConvertToMailMessage()
        {
            var message = new MailMessage { From = new MailAddress(From), IsBodyHtml = false };
            Recipients?.ToList().ForEach(x => message.To.Add(x));
            CcRecipients?.ToList().ForEach(x => message.CC.Add(x));
            message.Subject = Subject;
            message.Body = Content;
            return message;
        }

        public byte[] ToBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, this);
                return memoryStream.GetBuffer();
            }
        }

        public static MailDefinition FromBytes(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
                return (MailDefinition)new BinaryFormatter().Deserialize(memoryStream);
        }

    }
}
