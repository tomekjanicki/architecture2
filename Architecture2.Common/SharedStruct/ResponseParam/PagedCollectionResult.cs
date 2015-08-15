namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class PagedCollectionResult<TItem>
    {
        public PagedCollectionResult(Paged<TItem> results)
        {
            Results = results;
        }

        public Paged<TItem> Results { get; }
    }
}