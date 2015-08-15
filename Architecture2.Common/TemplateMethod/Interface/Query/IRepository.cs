using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.TemplateMethod.Interface.Query
{
    
    public interface IRepository<TItem, in TParam> where TParam : IRequest<Result<TItem>>
    {
        Result<TItem> Fetch(TParam query);
    }
}
