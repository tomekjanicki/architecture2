using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IPagedCollectionRepository<TItem, in TParam> where TParam : SortPageSizeSkip<TItem>
    {
        PagedCollectionResult<TItem> Get(TParam query);
    }
}