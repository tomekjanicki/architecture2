using System.Collections.Generic;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.SharedStruct.RequestParam
{
    public class Sort<TItem> : IRequest<CollectionResult<TItem>>
    {
        public string SortExp { get; set; }
    }
}