using System.Collections.Generic;
using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Query
{
    public abstract class CollectionQueryTemplateHandler<TQuery, TItem, TCollectionRepository> : QueryTemplateHandler<TQuery, TItem, TCollectionRepository>
        where TQuery : Sort<TItem> 
        where TCollectionRepository : ICollectionRepository<TItem, CollectionResult<TItem>, TQuery>
        where TItem : IReadOnlyCollection<TItem>
    {

        protected CollectionQueryTemplateHandler(IValidator<TQuery> validator, TCollectionRepository collectionRepository) : base(validator, collectionRepository)
        {
        }

    }
}