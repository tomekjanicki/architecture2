using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.TemplateMethod.Interface.Query
{
    public interface ICollectionRepository<TItem, in TParam> where TParam : Sort<TItem>
    {
        CollectionResult<TItem> Get(TParam query);
    }
}