using System.Collections.Generic;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.TemplateMethod.Interface.Query
{
    public interface ICollectionRepository<TItem, out TResult, in TParam> : IRepository<TItem,  TResult, TParam>
        where TParam : IRequest<TResult>
        where TResult : CollectionResult<TItem>
        where TItem : IReadOnlyCollection<TItem>
    {
        
    }
}