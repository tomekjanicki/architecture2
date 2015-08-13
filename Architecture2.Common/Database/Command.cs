using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture2.Common.Database.Exception;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.Exception;
using Architecture2.Common.IoC;
using Dapper;

namespace Architecture2.Common.Database
{
    [RegisterType]
    public class Command : Disposable, ICommand
    {
        private readonly ExceptionConverter _exceptionConverter;

        private IDbConnection _connection;

        private bool _disposed;

        public Command()
        {
            var types = new[] { typeof(SqlException) };
            _exceptionConverter = new ExceptionConverter(types, exception => new DbException(exception));
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
            _exceptionConverter.HandleAction(() =>
            {
                OpenConnection();
                _connection.Execute(sql, param);
            });
        }

        public IReadOnlyCollection<T> Query<T>(string sql, object param = null)
        {
            return _exceptionConverter.HandleFunction(() =>
            {
                OpenConnection();
                return _connection.Query<T>(sql, param).ToList();
            });
        }

        public T SingleOrDefault<T>(string sql, object param = null)
        {
            return Query<T>(sql, param).SingleOrDefault();
        }

        public T FirstOrDefault<T>(string sql, object param = null)
        {
            return Query<T>(sql, param).FirstOrDefault();
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
