﻿using System;
using System.Collections.Generic;
using Architecture2.Common.Database.Exception;
using Architecture2.Common.Mail;
using Architecture2.Common.Mail.Interface;
using Architecture2.Common.Test;
using NSubstitute;
using NUnit.Framework;

namespace Architecture2.Logic.Unit.Test.Mail
{
    public static class Send
    {
        public class WhenSendingMails : BaseTest
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
            public void ShouldSendAll_IfNoExceptionIsThrown()
            {
                var items = TenOldestMailDefinitionItems();

                _repository.Find10OldestMailDefinitions().Returns(items);

                var result = _sut.Handle();

                Assert.That(result != null && result.TotalQty == result.SuccessfulQty && result.TotalQty == items.Count);
            }

            [Test]
            public void ShouldHandle_IfExceptionWhichInheritsFromBaseExceptionIsThrown()
            {
                var items = TenOldestMailDefinitionItems();

                _repository.Find10OldestMailDefinitions().Returns(items);

                _repository.When(repository => repository.UpdateFinished(Arg.Any<int>())).Do(info => { throw new DbException(null); });

                var result = _sut.Handle();

                Assert.That(result != null && result.SuccessfulQty == 0);
            }

            [Test]
            public void ShouldNotHandle_IfExceptionWhichDoesNotInheritFromBaseExceptionIsThrown()
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
