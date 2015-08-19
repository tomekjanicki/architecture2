using System.Diagnostics;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.Exception.Logic.Constraint;
using Architecture2.Common.Test;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Helper = Architecture2.Common.FluentValidation.Helper;

namespace Architecture2.Logic.Unit.Test.Product
{
    public static class Delete
    {
        public class WhenDeletingProduct : BaseTest
        {
            private Logic.Product.Delete.CommandHandler _sut;

            private IValidator<Logic.Product.Delete.Command> _validator;
            private Logic.Product.Delete.IDeleteProductRepository _repository;

            public override void SetUp()
            {
                base.SetUp();
                _validator = Substitute.For<IValidator<Logic.Product.Delete.Command>>();
                _repository = Substitute.For<Logic.Product.Delete.IDeleteProductRepository>();
                _sut = new Logic.Product.Delete.CommandHandler(_validator, _repository);
            }

            [Test]
            public void ShouldDelete_IfCommandIsValid()
            {
                var version = new byte[0];

                var command = new Logic.Product.Delete.Command { Id = 1, Version = version };

                Debug.Assert(command.Id != null, $"{nameof(command.Id)} != null");

                _validator.Validate(command).Returns(new ValidationResult());

                _repository.GetRowVersion(command.Id.Value).Returns(version);

                _repository.Can(command.Id.Value).Returns(true);

                _sut.Handle(command);

                _repository.Received().Execute(command.Id.Value);
            }

            [Test]
            public void ShouldThrowException_IfCommandIsInvalid()
            {
                var command = new Logic.Product.Delete.Command();

                _validator.Validate(command).Returns(Helper.GetErrorValidationResult());

                Assert.Catch<ValidationException>(() => _sut.Handle(command));
            }

            [Test]
            public void ShouldThrowException_IfProductNotFound()
            {
                var version = new byte[0];

                var command = new Logic.Product.Delete.Command { Id = 1, Version = version };

                Debug.Assert(command.Id != null, $"{nameof(command.Id)} != null");

                _validator.Validate(command).Returns(new ValidationResult());

                _repository.GetRowVersion(command.Id.Value).Returns((byte[])null);

                Assert.Catch<NotFoundException<Logic.Product.Delete.Command>>(() => _sut.Handle(command));
            }

            [Test]
            public void ShouldThrowException_IfProductCannotBeDeleted()
            {
                var version = new byte[0];

                var command = new Logic.Product.Delete.Command { Id = 1, Version = version };

                Debug.Assert(command.Id != null, $"{nameof(command.Id)} != null");

                _validator.Validate(command).Returns(new ValidationResult());

                _repository.GetRowVersion(command.Id.Value).Returns(version);

                _repository.Can(command.Id.Value).Returns(false);

                Assert.Catch<ForeignKeyException<Logic.Product.Delete.Command>>(() => _sut.Handle(command));
            }

            [Test]
            public void ShouldThrowException_IfCommandHasWrongVersion()
            {
                var version1 = new byte[0];

                var version2 = new byte[] {5};

                var command = new Logic.Product.Delete.Command { Id = 1, Version = version1 };

                Debug.Assert(command.Id != null, $"{nameof(command.Id)} != null");

                _validator.Validate(command).Returns(new ValidationResult());

                _repository.GetRowVersion(command.Id.Value).Returns(version2);

                Assert.Catch<OptimisticConcurrencyException<Logic.Product.Delete.Command>>(() => _sut.Handle(command));
            }

        }

        public class WhenValidatingDeleteProductCommand : BaseTest
        {
            private Logic.Product.Delete.CommandValidator _sut;

            public override void TestFixtureSetUp()
            {
                base.TestFixtureSetUp();
                new Startup().Configure();
            }

            public override void SetUp()
            {
                base.SetUp();
                _sut = new Logic.Product.Delete.CommandValidator();
            }

            [Test]
            public void ShouldReturnNoErrors_IfCommandIsValid()
            {
                var command = new Logic.Product.Delete.Command {Id = 1, Version = new byte[] {5}};

                var result = _sut.Validate(command);

                Assert.That(result.Errors.Count == 0);
            }

            [Test]
            public void ShouldReturnError_IfCommandHasIdEqualToNull()
            {
                var command = new Logic.Product.Delete.Command { Version = new byte[] { 5 } };

                var result = _sut.Validate(command);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.Delete.Command.Id)));
            }


            [Test]
            public void ShouldReturnError_IfCommandHasVersionEqualToNull()
            {
                var command = new Logic.Product.Delete.Command { Id = 1 };

                var result = _sut.Validate(command);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.Delete.Command.Version)));
            }

            [Test]
            public void ShouldReturnError_IfCommandHasVersionEqualToEmptyArray()
            {
                var command = new Logic.Product.Delete.Command { Id = 1, Version = new byte[0]};

                var result = _sut.Validate(command);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.Delete.Command.Version)));
            }

            [Test]
            public void ShouldReturnError_IfCommandIsNull()
            {
                var result = _sut.Validate((Logic.Product.Delete.Command)null);

                Assert.That(result.Errors.Count > 0);
            }

        }
    }
}
