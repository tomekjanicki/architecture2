namespace Architecture2.Common.SharedStruct.ResponseParam
{
    public class Result<TItem>
    {
        public Result(TItem results)
        {
            Results = results;
        }

        public TItem Results { get; }
    }
}