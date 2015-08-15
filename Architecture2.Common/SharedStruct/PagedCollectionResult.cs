namespace Architecture2.Common.SharedStruct
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