using System.Diagnostics;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.Exception.Logic.Constraint;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.TemplateMethod.Command;
using Architecture2.Common.TemplateMethod.Interface.Command;
using FluentValidation;

namespace Architecture2.Logic.Product
{
    public static class Delete
    {
        public class Command : IdWithRowVersion
        {
        }

        [RegisterType]
        public class CommandValidator : IdWithRowVersionValidator<Command>
        {
        }

        [RegisterType]
        public class CommandHandler : DeleteCommandTemplateHandler<Command, IDeleteProductRepository>
        {
            public CommandHandler(IValidator<Command> validator, IDeleteProductRepository deleteProductRepository) : base(validator, deleteProductRepository)
            {
            }

            protected override void ExcecuteBeforeExecute(Command message)
            {
                Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

                var idString = message.Id.Value.ToString();

                var can = DeleteRepository.Can(message.Id.Value);

                if (!can)
                    throw new ForeignKeyException<Command>(idString) { Name = DeleteRepository.ConstraintName };
            }
        }

        public interface IDeleteProductRepository : IDeleteRepository
        {
            bool Can(int id);

            string ConstraintName { get; }
        }

        [RegisterType]
        public class DeleteRepository : IDeleteProductRepository
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
