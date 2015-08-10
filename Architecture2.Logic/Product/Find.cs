using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Architecture2.Common.Database;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.FluentValidation;
using Architecture2.Common.SharedStruct;
using Dapper;
using FluentValidation;
using MediatR;

namespace Architecture2.Logic.Product
{
    public class Find
    {
        public class Query : IRequest<Result>
        {
            public string Sort { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public int? PageSize { get; set; }
            public int? Skip { get; set; }
        }


        public class QueryValidator : AbstractClassValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(query => query.PageSize).NotNull().InclusiveBetween(1, int.MaxValue);
                RuleFor(query => query.Skip).NotNull().InclusiveBetween(0, int.MaxValue);
            }

        }

        public class Result
        {
            public Result(Paged<ProductItem> results)
            {
                Results = results;
            }

            public Paged<ProductItem> Results { get;  }
        }

        public class ProductItem
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public DateTime? Date { get; set; }
            public bool CanDelete { get; set; }
            public byte[] Version { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly IRepository _repository;
            private readonly IValidator<Query> _validator;

            public QueryHandler(IRepository repository, IValidator<Query> validator)
            {
                _repository = repository;
                _validator = validator;
            }

            public Result Handle(Query query)
            {
                _validator.ValidateAndThrow(query);

                var result = _repository.GetData(query);

                return result;
            }

        }

        public interface IRepository
        {
            Result GetData(Query query);
        }

        public class Repository : IRepository
        {
            private const string SelectProductQuery = @"SELECT ID, CODE, NAME, PRICE, VERSION, CASE WHEN ID < 20 THEN GETDATE() ELSE NULL END DATE, CASE WHEN O.PRODUCTID IS NULL THEN 1 ELSE 0 END CANDELETE FROM DBO.PRODUCTS P LEFT JOIN (SELECT DISTINCT PRODUCTID FROM DBO.ORDERSDETAILS) O ON P.ID = O.PRODUCTID {0} {1}";
            private const string CountProductQuery = @"SELECT COUNT(*) FROM DBO.PRODUCTS {0}";

            private readonly ICommand _command;

            public Repository(ICommand command)
            {
                _command = command;
            }

            public Result GetData(Query query)
            {
                Debug.Assert(query.Skip != null, $"{nameof(query.Skip)} != null");
                Debug.Assert(query.PageSize != null, $"{nameof(query.PageSize)} != null");

                var whereFragment = GetWhereFragment(query.Code, query.Name);
                var pagedFragment = CommandHelper.GetPagedFragment(new Page(query.PageSize.Value, query.Skip.Value), GetTranslatedSort(query.Sort));

                var countQuery = string.Format(CountProductQuery, whereFragment.Query);
                var selectQuery = string.Format(SelectProductQuery, whereFragment.Query, pagedFragment.Query);
                var count = _command.Query<int>(countQuery, whereFragment.Parameters).Single();
                whereFragment.Parameters.AddDynamicParams(pagedFragment.Parameters);
                var select = _command.Query<ProductItem>(selectQuery, whereFragment.Parameters);

                return new Result(new Paged<ProductItem>(count, select));
            }

            private static CommandHelper.Result GetWhereFragment(string code, string name)
            {
                var dp = new DynamicParameters();
                var criteria = new List<string>();
                if (!string.IsNullOrEmpty(code))
                    CommandHelper.SetValues(criteria, dp, CommandHelper.GetLikeCaluse("CODE", "CODE", code));
                if (!string.IsNullOrEmpty(name))
                    CommandHelper.SetValues(criteria, dp, CommandHelper.GetLikeCaluse("NAME", "NAME", name));
                return CommandHelper.GetWhereStringWithParams(criteria, dp);
            }

            private string GetTranslatedSort(string modelColumn)
            {
                return CommandHelper.GetTranslatedSort(modelColumn, $"{nameof(ProductItem.Code)} ASC", new[]
                {
                    nameof(ProductItem.Id),
                    nameof(ProductItem.Code),
                    nameof(ProductItem.Name),
                    nameof(ProductItem.Price),
                    nameof(ProductItem.Date)
                });
            }

        }


    }
}
