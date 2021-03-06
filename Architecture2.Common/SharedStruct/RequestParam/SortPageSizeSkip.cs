﻿using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;

namespace Architecture2.Common.SharedStruct.RequestParam
{
    public class SortPageSizeSkip<TItem> : IRequest<PagedCollectionResult<TItem>>
    {
        public string SortExp { get; set; }
        public int? PageSize { get; set; }
        public int? Skip { get; set; }
    }
}