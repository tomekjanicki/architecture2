using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IPagedRepository<TItem, in TParam> where TParam : SortPageSizeSkip<TItem>
    {
        Result<TItem> Get(TParam query);
    }
}