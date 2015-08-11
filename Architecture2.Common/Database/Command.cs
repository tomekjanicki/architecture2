using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.IoC;
using Dapper;

namespace Architecture2.Common.Database
{
    [RegisterType]
    public class Command : Disposable, ICommand
    {
        private IDbConnection _connection;

        private bool _disposed;

        public Command()
        {
            _connection = DatabaseExtension.GetConnection("Main", false);
        }

        private void OpenConnection()
        {
            EnsureNotDisposed();
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void Execute(string sql, object param = null)
        {
            OpenConnection();
            _connection.Execute(sql, param);
        }

        public IReadOnlyCollection<T> Query<T>(string sql, object param = null)
        {
            OpenConnection();
            return _connection.Query<T>(sql, param).ToList();
        }

        public T SingleOrDefault<T>(string sql, object param = null)
        {
            OpenConnection();
            return _connection.Query<T>(sql, param).SingleOrDefault();
        }

        public T FirstOrDefault<T>(string sql, object param = null)
        {
            OpenConnection();
            return _connection.Query<T>(sql, param).FirstOrDefault();
        }

        protected override void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, () =>
            {
                StandardDisposeWithAction(ref _connection, () =>
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                });
            });
            base.Dispose(disposing);
        }

        private void EnsureNotDisposed()
        {
            EnsureNotDisposed(_disposed);
        }

    }
}
