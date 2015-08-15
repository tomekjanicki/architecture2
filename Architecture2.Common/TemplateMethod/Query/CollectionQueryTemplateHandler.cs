using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Query
{
    public abstract class CollectionQueryTemplateHandler<TQuery, TItem, TCollectionRepository> : IRequestHandler<TQuery, CollectionResult<TItem>>
        where TQuery : Sort<TItem>
        where TCollectionRepository : ICollectionRepository<TItem, TQuery>

    {
        protected readonly TCollectionRepository CollectionRepository;
        private readonly IValidator<TQuery> _validator;

        protected CollectionQueryTemplateHandler(IValidator<TQuery> validator, TCollectionRepository collectionRepository)
        {
            CollectionRepository = collectionRepository;
            _validator = validator;
        }

        public CollectionResult<TItem> Handle(TQuery message)
        {
            ExecuteValidate(message);

            ExecuteBeforeExecuteGet(message);

            return ExecuteGet(message);
        }

        protected virtual void ExecuteValidate(TQuery message)
        {
            _validator?.ValidateAndThrow(message);
        }

        protected virtual void ExecuteBeforeExecuteGet(TQuery message)
        {
        }

        protected virtual CollectionResult<TItem> ExecuteGet(TQuery message)
        {
            return CollectionRepository.Get(message);
        }
    }
}