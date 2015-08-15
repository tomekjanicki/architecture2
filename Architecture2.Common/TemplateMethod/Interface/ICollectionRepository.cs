using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface ICollectionRepository<TItem, in TParam> where TParam : Sort<TItem>
    {
        CollectionResult<TItem> Get(TParam query);
    }
}