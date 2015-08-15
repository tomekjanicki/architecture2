using System.Collections.Generic;

namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class CollectionResult<TItem> : Result<IReadOnlyCollection<TItem>> 
    {
        public CollectionResult(IReadOnlyCollection<TItem> results): base(results)
        {
        }

    }
}