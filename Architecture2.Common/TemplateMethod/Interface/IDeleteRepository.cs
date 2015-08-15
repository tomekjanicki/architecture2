namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IDeleteRepository
    {
        void Execute(int id);

        byte[] GetRowVersion(int id);
    }
}
