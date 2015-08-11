namespace Architecture2.Common.TemplateMethod.Interface
{
    public interface IDeleteRepository
    {
        void Delete(int id);

        byte[] GetRowVersion(int id);

        bool CanDelete(int id);

        string ConstraintName { get; }
    }
}
