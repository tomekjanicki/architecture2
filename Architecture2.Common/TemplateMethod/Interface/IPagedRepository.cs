using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IPagedRepository<TItem, in TParam> where TParam : SortPageSizeSkipParam<TItem>
    {
        Result<TItem> GetData(TParam param);
    }
}