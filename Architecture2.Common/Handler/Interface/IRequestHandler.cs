namespace Architecture2.Common.Handler.Interface
{
    public interface IRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        void Handle(TRequest message);
    }

    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest message);
    }
}