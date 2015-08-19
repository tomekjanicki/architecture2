using System.Net.Mail;
using Architecture2.Common.Exception;
using Architecture2.Common.Mail.Exception;
using Architecture2.Common.Test;
using NUnit.Framework;

namespace Architecture2.Common.Unit.Test.Exception
{
    public class WhenHandlingAction : BaseTest
    {
        private ExceptionConverter _exceptionConverter;

        public override void TestFixtureSetUp()
        {
            var types = new[] { typeof(SmtpException) };
            _exceptionConverter = new ExceptionConverter(types, exception => new MailServiceException(exception));
        }

        [Test]
        public void ShouldThrowWrapedException_IfConfiguredExceptionTypeIsThrown()
        {
            Assert.Catch<MailServiceException>(() => _exceptionConverter.HandleAction(() => { throw new SmtpException(); }));
        }

        [Test]
        public void ShouldThrowWrapedException_IfConfiguredInheritedExceptionTypeIsThrown()
        {
            Assert.Catch<MailServiceException>(() => _exceptionConverter.HandleAction(() => { throw new SmtpFailedRecipientException(); }));
        }

        [Test]
        public void ShouldThrowOriginalException_IfNotConfiguredExceptionTypeIsThrown()
        {
            Assert.Catch<System.Exception>(() => _exceptionConverter.HandleAction(() => { throw new System.Exception(); }));
        }

    }
}
