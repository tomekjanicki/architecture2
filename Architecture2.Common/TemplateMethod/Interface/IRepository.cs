using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct;

namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IRepository<TItem, in TParam> where TParam : IRequest<Result<TItem>>
    {
        Result<TItem> Get(TParam query);
    }
}