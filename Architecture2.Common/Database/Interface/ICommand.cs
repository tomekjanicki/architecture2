using System;
using System.Collections.Generic;

namespace Architecture2.Common.Database.Interface
{
    public interface ICommand : IDisposable
    {
        void Execute(string sql, object param = null);

        IReadOnlyCollection<T> Query<T>(string sql, object param = null);
    }
}
