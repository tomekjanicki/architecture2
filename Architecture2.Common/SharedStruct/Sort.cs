using Architecture2.Common.Handler.Interface;

namespace Architecture2.Common.SharedStruct
{
    public class Sort<TItem> : IRequest<CollectionResult<TItem>>
    {
        public string SortExp { get; set; }
    }
}