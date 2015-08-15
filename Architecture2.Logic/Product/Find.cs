using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Architecture2.Common.Database;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.TemplateMethod.Interface.Query;
using Architecture2.Common.TemplateMethod.Query;
using Dapper;
using FluentValidation;

namespace Architecture2.Logic.Product
{
    public class Find
    {
        public class Query : SortPageSizeSkip<ProductItem>
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        [RegisterType]
        public class QueryValidator : SortPageSizeSkipValidator<ProductItem>
        {

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

        [RegisterType]
        public class QueryHandler : PagedCollectionQueryTemplateHandler<Query, ProductItem, IPagedCollectionRepository<ProductItem, Query>>
        {

            public QueryHandler(IPagedCollectionRepository<ProductItem, Query> pagedCollectionRepository, IValidator<Query> validator) : base(validator, pagedCollectionRepository)
            {
            }

        }

        [RegisterType]
        public class ProductItemCollectionRepository : IPagedCollectionRepository<ProductItem, Query>
        {
            private const string SelectProductQuery = @"SELECT ID, CODE, NAME, PRICE, VERSION, CASE WHEN ID < 20 THEN GETDATE() ELSE NULL END DATE, CASE WHEN O.PRODUCTID IS NULL THEN 1 ELSE 0 END CANDELETE FROM DBO.PRODUCTS P LEFT JOIN (SELECT DISTINCT PRODUCTID FROM DBO.ORDERSDETAILS) O ON P.ID = O.PRODUCTID {0} {1}";
            private const string CountProductQuery = @"SELECT COUNT(*) FROM DBO.PRODUCTS {0}";

            private readonly ICommand _command;

            public ProductItemCollectionRepository(ICommand command)
            {
                _command = command;
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

            public PagedCollectionResult<ProductItem> Get(Query query)
            {
                Debug.Assert(query.Skip != null, $"{nameof(query.Skip)} != null");
                Debug.Assert(query.PageSize != null, $"{nameof(query.PageSize)} != null");

                var whereFragment = GetWhereFragment(query.Code, query.Name);
                var pagedFragment = CommandHelper.GetPagedFragment(new Page(query.PageSize.Value, query.Skip.Value), GetTranslatedSort(query.SortExp));

                var countQuery = string.Format(CountProductQuery, whereFragment.Query);
                var selectQuery = string.Format(SelectProductQuery, whereFragment.Query, pagedFragment.Query);
                var count = _command.Query<int>(countQuery, whereFragment.Parameters).Single();
                whereFragment.Parameters.AddDynamicParams(pagedFragment.Parameters);
                var select = _command.Query<ProductItem>(selectQuery, whereFragment.Parameters);

                return new PagedCollectionResult<ProductItem>(new Paged<ProductItem>(count, select));
            }
        }


    }
}
