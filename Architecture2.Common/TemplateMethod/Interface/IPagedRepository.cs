using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IPagedRepository<TItem, TParam>
    {
        Result<TItem> GetData(SortPageSizeSkipParam<TItem, TParam> sortPageSizeSkipParam);
    }
}