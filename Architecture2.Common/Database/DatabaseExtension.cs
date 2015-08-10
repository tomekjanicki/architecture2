using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Architecture2.Common.Database
{
    public static class DatabaseExtension
    {
        private const string SqlClient = "System.Data.SqlClient";
        public static IDbConnection GetConnection(string key, bool switchToMaster)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            var connection = factory.CreateConnection();
            Debug.Assert(connection != null, $"{nameof(connection)} != null");
            connection.ConnectionString = switchToMaster ? GetConnectionStringWithMasterDb(connectionString) : connectionString.ConnectionString;
            return connection;
        }

        private static string GetConnectionStringWithMasterDb(ConnectionStringSettings css)
        {
            if (css.ProviderName == SqlClient)
                return new SqlConnectionStringBuilder(css.ConnectionString) { InitialCatalog = "master" }.ToString();
            throw new NotImplementedException();
        }

        public static string ToLikeString(this string input, string escapeChar)
        {
            return input == null ? null : $"%{input.ToLikeStringInternal(escapeChar)}%";
        }

        public static string ToLikeLeftString(this string input, string escapeChar)
        {
            return input == null ? null : $"%{input.ToLikeStringInternal(escapeChar)}";
        }

        public static string ToLikeRightString(this string input, string escapeChar)
        {
            return input == null ? null : $"{input.ToLikeStringInternal(escapeChar)}%";
        }

        private static string ToLikeStringInternal(this string input, string escapeChar)
        {
            input = input.Replace(escapeChar, string.Format("{0}{0}", escapeChar));
            input = input.Replace("%", $"{escapeChar}%");
            input = input.Replace("_", $"{escapeChar}_");
            input = input.Replace("[", $"{escapeChar}[");
            return input;
        }


    }
}
