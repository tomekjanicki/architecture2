namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IUpdateRepository<in T>
    {
        void Execute(T entity);
        byte[] GetRowVersion(int id);
    }
}