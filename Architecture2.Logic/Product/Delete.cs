using System;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.TemplateMethod;
using Architecture2.Common.TemplateMethod.Interface;
using FluentValidation;
using MediatR;

namespace Architecture2.Logic.Product
{
    public class Delete
    {
        public class Command : IdWithRowVersion, INotification
        {
        }

        [RegisterType]
        public class CommandValidator : IdWithRowVersionValidator<Command>
        {
        }

        [RegisterType]
        public class CommandHandler : DeleteTemplateHandler<Command>
        {
            public CommandHandler(IValidator<Command> validator, IDeleteRepository deleteRepository) : base(validator, deleteRepository)
            {
            }
        }

        [RegisterType]
        public class DeleteRepository : IDeleteRepository
        {
            public void Delete(int id)
            {
                throw new NotImplementedException();
            }

            public byte[] GetRowVersion(int id)
            {
                throw new NotImplementedException();
            }

            public bool CanDelete(int id)
            {
                throw new NotImplementedException();
            }

            public string ConstraintName => "product_order";
        }


    }
}
