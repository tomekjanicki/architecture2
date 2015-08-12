
using Architecture2.Common.Handler.Interface;

namespace Architecture2.Common.SharedStruct
{
    public class SortPageSizeSkip<TItem> : IRequest<Result<TItem>>
    {
        public string Sort { get; set; }
        public int? PageSize { get; set; }
        public int? Skip { get; set; }
    }
}