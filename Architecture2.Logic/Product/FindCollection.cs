using System;
using System.Collections.Generic;
using Architecture2.Common.Database;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using Architecture2.Common.TemplateMethod.Query;
using Dapper;

namespace Architecture2.Logic.Product
{
    public static class FindCollection
    {
        public class Query : Sort<ProductItem>
        {
            public string Name { get; set; }
            public string Code { get; set; }
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
        public class QueryHandler : CollectionQueryTemplateHandler<Query, ProductItem, ICollectionRepository<ProductItem, Query>>
        {

            public QueryHandler(ICollectionRepository<ProductItem, Query> collectionRepository) : base(null, collectionRepository)
            {
            }
        }

        [RegisterType]
        public class ProductItemCollectionRepository : ICollectionRepository<ProductItem, Query>
        {
            private const string SelectProductQuery = @"SELECT ID, CODE, NAME, PRICE, VERSION, CASE WHEN ID < 20 THEN GETDATE() ELSE NULL END DATE, CASE WHEN O.PRODUCTID IS NULL THEN 1 ELSE 0 END CANDELETE FROM DBO.PRODUCTS P LEFT JOIN (SELECT DISTINCT PRODUCTID FROM DBO.ORDERSDETAILS) O ON P.ID = O.PRODUCTID {0} {1}";

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

            public CollectionResult<ProductItem> Fetch(Query query)
            {
                var whereFragment = GetWhereFragment(query.Code, query.Name);
                var selectQuery = string.Format(SelectProductQuery, whereFragment.Query, GetTranslatedSort(query.SortExp));
                var select = _command.Query<ProductItem>(selectQuery, whereFragment.Parameters);
                return new CollectionResult<ProductItem>(select);
            }
        }


    }
}
