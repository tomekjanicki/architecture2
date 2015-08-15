using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.TemplateMethod.Interface.Query
{
    public interface IRepository<TItem, out TResult, in TParam>
        where TParam : IRequest<TResult>
        where TResult : Result<TItem>
    {
        TResult Get(TParam query);
    }
}
