namespace Architecture2.Common.TemplateMethod.Interface.Command
{
    public interface IInsertRepository<in T>
    {
        int Execute(T entity);
    }
}