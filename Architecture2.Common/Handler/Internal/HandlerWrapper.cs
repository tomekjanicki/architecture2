using Architecture2.Common.Handler.Interface;

namespace Architecture2.Common.Handler.Internal
{
    internal class HandlerWrapper<TResult> : AbstractHandlerWrapper<TResult>
    {
        private readonly IHandler<TResult> _inner;

        public HandlerWrapper(IHandler<TResult> inner)
        {
            _inner = inner;
        }

        public override TResult Handle()
        {
            return _inner.Handle();
        }
    }
}