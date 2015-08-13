using Architecture2.Common.Handler.Interface;

namespace Architecture2.Common.Handler.Internal
{
    internal abstract class VoidRequestHandlerWrapper
    {
        public abstract void Handle(IRequest message);
    }

    internal class VoidRequestHandlerWrapper<TCommand> : VoidRequestHandlerWrapper
        where TCommand : IRequest
    {
        private readonly IRequestHandler<TCommand> _inner;

        public VoidRequestHandlerWrapper(IRequestHandler<TCommand> inner)
        {
            _inner = inner;
        }

        public override void Handle(IRequest message)
        {
            _inner.Handle((TCommand)message);
        }
    }
}