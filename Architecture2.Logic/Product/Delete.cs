using Architecture2.Common.Database.Interface;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.TemplateMethod;
using Architecture2.Common.TemplateMethod.Interface;
using FluentValidation;

namespace Architecture2.Logic.Product
{
    public class Delete
    {
        public class Command : IdWithRowVersion
        {
        }

        [RegisterType]
        public class CommandValidator : IdWithRowVersionValidator<Command>
        {
        }

        [RegisterType]
        public class CommandHandler : DeleteCommandTemplateHandler<Command>
        {
            public CommandHandler(IValidator<Command> validator, IDeleteRepository deleteRepository) : base(validator, deleteRepository)
            {
            }
        }

        [RegisterType]
        public class DeleteRepository : IDeleteRepository
        {
            private readonly ICommand _command;

            public DeleteRepository(ICommand command)
            {
                _command = command;
            }

            public void Execute(int id)
            {
                _command.Execute(@"DELETE FROM DBO.PRODUCTS WHERE ID = @ID", new { ID = id });
            }

            public byte[] GetRowVersion(int id)
            {
                return _command.SingleOrDefault<byte[]>(@"SELECT VERSION FROM DBO.PRODUCTS WHERE ID = @ID", new {ID = id});
            }

            public bool Can(int id)
            {
                return _command.SingleOrDefault<int>("SELECT COUNT(*) FROM DBO.ORDERSDETAILS WHERE PRODUCTID = @PRODUCTID", new { PRODUCTID = id }) == 0;
            }

            public string ConstraintName => "product_order";
        }


    }
}
