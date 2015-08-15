namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class PagedCollectionResult<TItem> : Result<Paged<TItem>>
    {
        public PagedCollectionResult(Paged<TItem> results) : base(results)
        {
        }
    }
}