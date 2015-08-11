using System;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.TemplateMethod;
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
            private readonly IRepository _repository;

            public CommandHandler(IValidator<Command> validator, IRepository repository) : base(validator)
            {
                _repository = repository;
            }

            protected override byte[] GetRowVersion(int id)
            {
                return _repository.GetVersion(id);
            }

            protected override bool CanDelete(int id)
            {
                return _repository.CanDelete(id);
            }

            protected override string ConstraintName => "order_product";

            protected override void Delete(int id)
            {
                _repository.Delete(id);
            }
        }

        public interface IRepository
        {
            void Delete(int id);

            byte[] GetVersion(int id);

            bool CanDelete(int id);
        }

        [RegisterType]
        public class Repository : IRepository
        {
            public void Delete(int id)
            {
                throw new NotImplementedException();
            }

            public byte[] GetVersion(int id)
            {
                throw new NotImplementedException();
            }

            public bool CanDelete(int id)
            {
                throw new NotImplementedException();
            }
        }


    }
}
