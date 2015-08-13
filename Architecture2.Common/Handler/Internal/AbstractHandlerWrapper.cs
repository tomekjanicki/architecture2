namespace Architecture2.Common.Handler.Internal
{
    internal abstract class AbstractHandlerWrapper<TResult>
    {
        public abstract TResult Handle();
    }
}
