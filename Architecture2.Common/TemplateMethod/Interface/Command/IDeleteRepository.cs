namespace Architecture2.Common.TemplateMethod.Interface.Command
{
    public interface IDeleteRepository
    {
        void Execute(int id);

        byte[] GetRowVersion(int id);
    }
}
