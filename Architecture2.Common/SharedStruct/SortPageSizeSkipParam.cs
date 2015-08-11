using MediatR;

namespace Architecture2.Common.SharedStruct
{
    public class SortPageSizeSkipParam<TItem, TParam> : IRequest<Result<TItem>>
    {
        public string Sort { get; set; }
        public int? PageSize { get; set; }
        public int? Skip { get; set; }

        public TParam Param { get; set; }
    }
}