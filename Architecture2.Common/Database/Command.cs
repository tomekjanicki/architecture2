using System;
using System.Collections.Generic;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.IoC;

namespace Architecture2.Common.Database
{
    [RegisterType]
    public class Command : ICommand
    {
        public void Execute(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> Query<T>(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
