using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.TemplateMethod.Interface.Query
{
    public interface IPagedCollectionRepository<TItem, in TParam> where TParam : SortPageSizeSkip<TItem>
    {
        PagedCollectionResult<TItem> Fetch(TParam query);
    }
}