namespace Architecture2.Common.Handler.Interface
{
    public interface IHandler<out TResponse>
    {
        TResponse Handle();
    }
}