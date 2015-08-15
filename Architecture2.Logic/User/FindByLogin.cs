using System.Collections.Generic;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.FluentValidation;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.IoC;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using Architecture2.Common.TemplateMethod.Query;
using FluentValidation;

namespace Architecture2.Logic.User
{
    public static class FindByLogin
    {
        public class Query : IRequest<Result<LoginItem>>
        {
            public string Login { get; set; }
        }

        [RegisterType]
        public class QueryValidator : AbstractClassValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(query => query.Login).NotNull().NotEmpty();
            }
        }

        [RegisterType]
        public class QueryHandler : QueryTemplateHandler<Query, LoginItem, IRepository<LoginItem, Query>>
        {

            public QueryHandler(IRepository<LoginItem, Query> repository, IValidator<Query> validator) : base(validator, repository)
            {
            }
        }

        public class LoginItem
        {
            public LoginItem(string firstName, string lastName, IReadOnlyCollection<string> roles)
            {
                FirstName = firstName;
                LastName = lastName;
                Roles = roles;
            }

            public string FirstName { get; }
            public string LastName { get; }
            public IReadOnlyCollection<string> Roles { get;  }
        }

        [RegisterType]
        public class LoginItemRepository : IRepository<LoginItem, Query>
        {

            private readonly ICommand _command;

            public LoginItemRepository(ICommand command)
            {
                _command = command;
            }

            public Result<LoginItem> Fetch(Query query)
            {
                var user = _command.FirstOrDefault<LoginItem>("SELECT FIRSTNAME, LASTNAME FROM DBO.USERS WHERE LOWER(LOGIN) = @LOGIN", new { LOGIN = query.Login.ToLower() });
                if (user == null)
                    return null;
                var roles = _command.Query<string>("SELECT NAME FROM DBO.ROLES WHERE ID IN (SELECT ROLEID FROM DBO.USERSROLES WHERE USERID IN (SELECT ID FROM DBO.USERS WHERE LOWER(LOGIN) = @LOGIN))", new { LOGIN = query.Login.ToLower() });
                return new Result<LoginItem>(new LoginItem(user.FirstName, user.LastName, roles));
            }
        }

    }
}
