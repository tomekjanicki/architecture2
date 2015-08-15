using System.Collections.Generic;

namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class CollectionResult<TItem> : Result<TItem> 
        where TItem : IReadOnlyCollection<TItem>
    {
        public CollectionResult(TItem results): base(results)
        {
        }

    }
}