using System.Net.Mail;
using Architecture2.Common.Exception;
using Architecture2.Common.Mail.Exception;
using Architecture2.Common.Test;
using NUnit.Framework;

namespace Architecture2.Common.Unit.Test.Exception
{
    public class ExceptionConverterTest : BaseTest
    {
        private ExceptionConverter _exceptionConverter;

        public override void TestFixtureSetUp()
        {
            var types = new[] { typeof(SmtpException) };
            _exceptionConverter = new ExceptionConverter(types, exception => new MailServiceException(exception));
        }

        [Test]
        public void HandleAction_ThrowConfiguredExceptionType_ThrowsWrapedException()
        {
            Assert.Catch<MailServiceException>(() => _exceptionConverter.HandleAction(() => { throw new SmtpException(); }));
        }

        [Test]
        public void HandleAction_ThrowInheritedExceptionType_ThrowsWrapedException()
        {
            Assert.Catch<MailServiceException>(() => _exceptionConverter.HandleAction(() => { throw new SmtpFailedRecipientException(); }));
        }

        [Test]
        public void HandleAction_ThrowNotConfiguredExceptionType_ThrowsOrginalException()
        {
            Assert.Catch<System.Exception>(() => _exceptionConverter.HandleAction(() => { throw new System.Exception(); }));
        }

    }
}
