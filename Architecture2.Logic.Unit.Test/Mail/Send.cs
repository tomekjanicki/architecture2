using System;
using System.Collections.Generic;
using Architecture2.Common.Exception;
using Architecture2.Common.Mail;
using Architecture2.Common.Mail.Interface;
using Architecture2.Common.Test;
using NSubstitute;
using NUnit.Framework;

namespace Architecture2.Logic.Unit.Test.Mail
{
    public class Send
    {
        public class CommandHandlerTest : BaseTest
        {
            private Logic.Mail.Send.CommandHandler _sut;
            private Logic.Mail.Send.IRepository _repository;
            private IMailService _mailService;

            public override void SetUp()
            {
                base.SetUp();
                _repository = Substitute.For<Logic.Mail.Send.IRepository>();
                _mailService = Substitute.For<IMailService>();
                _sut = new Logic.Mail.Send.CommandHandler(_repository, _mailService);
            }

            [Test]
            public void Handle_ValidItem_Sent()
            {
                var items = TenOldestMailDefinitionItems();

                _repository.Find10OldestMailDefinitions().Returns(items);

                var result = _sut.Handle();

                Assert.That(result != null && result.TotalQty == result.SuccessfulQty && result.TotalQty == items.Count);
            }

            [Test]
            public void Send_ThrowsBaseException_IsHandled()
            {
                var items = TenOldestMailDefinitionItems();

                _repository.Find10OldestMailDefinitions().Returns(items);

                _repository.When(repository => repository.UpdateFinished(Arg.Any<int>())).Do(info => { throw new DbException(""); });

                var result = _sut.Handle();

                Assert.That(result != null && result.SuccessfulQty == 0);
            }

            [Test]
            public void Send_ThrowsException_IsNotHandled()
            {
                var items = TenOldestMailDefinitionItems();

                _repository.Find10OldestMailDefinitions().Returns(items);

                _repository.When(repository => repository.UpdateFinished(Arg.Any<int>())).Do(info => { throw new Exception(""); });

                Assert.Catch<Exception>(() => _sut.Handle());
            }

            private static IReadOnlyCollection<Logic.Mail.Send.TenOldestMailDefinitionItem> TenOldestMailDefinitionItems()
            {
                var md = new MailDefinition
                {
                    Recipients = new[] {"example@example.pl"},
                    From = "example@example.pl"
                };

                return new List<Logic.Mail.Send.TenOldestMailDefinitionItem>
                {
                    new Logic.Mail.Send.TenOldestMailDefinitionItem {MailDefinition = md}
                };
            }
        }
    }
}
