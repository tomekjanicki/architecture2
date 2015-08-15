using System.Collections.Generic;

namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class CollectionResult<TItem>
    {
        public CollectionResult(IReadOnlyCollection<TItem> results)
        {
            Results = results;
        }

        public IReadOnlyCollection<TItem> Results { get; }

    }
}