namespace Architecture2.Common.Handler.Interface
{
    public interface IMediator
    {
        TResponse Send<TResponse>(IRequest<TResponse> request);

        void Send(IRequest request);

        TResponse Send<TResponse>();
    }
}