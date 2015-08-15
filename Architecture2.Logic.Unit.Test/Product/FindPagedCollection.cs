using System.Collections.Generic;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using Architecture2.Common.Test;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Helper = Architecture2.Common.FluentValidation.Helper;

namespace Architecture2.Logic.Unit.Test.Product
{
    public class FindPagedCollection
    {
        public class QueryHandlerTest : BaseTest
        {
            private Logic.Product.FindPagedCollection.QueryHandler _sut;
            private IPagedCollectionRepository<Logic.Product.FindPagedCollection.ProductItem, Logic.Product.FindPagedCollection.Query> _collectionRepository;
            private IValidator<Logic.Product.FindPagedCollection.Query> _validator;

            public override void SetUp()
            {
                base.SetUp();
                _collectionRepository = Substitute.For<IPagedCollectionRepository<Logic.Product.FindPagedCollection.ProductItem, Logic.Product.FindPagedCollection.Query>>();
                _validator = Substitute.For<IValidator<Logic.Product.FindPagedCollection.Query>>();
                _sut = new Logic.Product.FindPagedCollection.QueryHandler(_collectionRepository, _validator);
            }

            [Test]
            public void Handle_ValidArgument_ReturnsData()
            {
                var query = new Logic.Product.FindPagedCollection.Query();

                _validator.Validate(query).Returns(new ValidationResult());

                _collectionRepository.Get(query).Returns(new PagedCollectionResult<Logic.Product.FindPagedCollection.ProductItem>(new Paged<Logic.Product.FindPagedCollection.ProductItem>(0, new List<Logic.Product.FindPagedCollection.ProductItem>())));

                var result = _sut.Handle(query);

                Assert.That(result != null);
            }

            [Test]
            public void Handle_InvalidArgument_ThrowsException()
            {
                var query = new Logic.Product.FindPagedCollection.Query();

                _validator.Validate(query).Returns(Helper.GetErrorValidationResult());

                Assert.Catch<ValidationException>(() => _sut.Handle(query));
            }

        }

        public class QueryValidatorTest : BaseTest
        {
            private Logic.Product.FindPagedCollection.QueryValidator _sut;

            public override void TestFixtureSetUp()
            {
                base.TestFixtureSetUp();
                new Startup().Configure();
            }

            public override void SetUp()
            {
                base.SetUp();
                _sut = new Logic.Product.FindPagedCollection.QueryValidator();
            }

            [Test]
            public void Validate_ValidArgument_NoErrors ()
            {
                var query = new Logic.Product.FindPagedCollection.Query {PageSize = 20, Skip = 5};

                var result = _sut.Validate(query);

                Assert.That(result.Errors.Count == 0);
            }

            [Test]
            public void Validate_PageSizeNull_Error()
            {
                var query = new Logic.Product.FindPagedCollection.Query { Skip = 5 };

                var result = _sut.Validate(query);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.FindPagedCollection.Query.PageSize)));
            }

            [Test]
            public void Validate_SkipNull_Error()
            {
                var query = new Logic.Product.FindPagedCollection.Query { PageSize = 1 };

                var result = _sut.Validate(query);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.FindPagedCollection.Query.Skip)));
            }

            [Test]
            public void Validate_PageSizeTooSmall_Error()
            {
                var query = new Logic.Product.FindPagedCollection.Query { PageSize  = 0, Skip = 5 };

                var result = _sut.Validate(query);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.FindPagedCollection.Query.PageSize)));
            }

            [Test]
            public void Validate_SkipTooSmall_Error()
            {
                var query = new Logic.Product.FindPagedCollection.Query { Skip = -1, PageSize = 1};

                var result = _sut.Validate(query);

                Assert.That(Common.Test.Helper.HasError(result, nameof(Logic.Product.FindPagedCollection.Query.Skip)));
            }

            [Test]
            public void Validate_NullArgument_Error()
            {
                var result = _sut.Validate((Logic.Product.FindPagedCollection.Query)null);

                Assert.That(result.Errors.Count > 0);
            }


        }
    }
}
