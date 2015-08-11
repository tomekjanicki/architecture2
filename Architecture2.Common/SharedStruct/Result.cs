namespace Architecture2.Common.SharedStruct
{
    public class Result<TItem>
    {
        public Result(Paged<TItem> results)
        {
            Results = results;
        }

        public Paged<TItem> Results { get; }
    }
}