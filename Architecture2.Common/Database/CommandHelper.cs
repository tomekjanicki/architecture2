using System.Collections.Generic;
using System.Linq;
using Architecture2.Common.SharedStruct;
using Dapper;

namespace Architecture2.Common.Database
{
    public static class CommandHelper
    {
        public class Result
        {
            public Result(string query, DynamicParameters parameters)
            {
                Query = query;
                Parameters = parameters;
            }

            public string Query { get; }

            public DynamicParameters Parameters { get; }
        }

        public static Result GetPagedFragment(Page page, string sort)
        {
            var dp = new DynamicParameters();
            dp.Add("SKIP", page.Skip);
            dp.Add("PAGESIZE", page.PageSize);
            return new Result($@"{GetSort(sort)} OFFSET @SKIP ROWS FETCH NEXT @PAGESIZE ROWS ONLY", dp);
        }

        public static string GetSort(string sort)
        {
            return $@"ORDER BY {sort}";
        }

        public static void SetValues(ICollection<string> criteria, DynamicParameters dp, Result like)
        {
            criteria.Add(like.Query);
            dp.AddDynamicParams(like.Parameters);
        }

        public static Result GetLikeCaluse(string fieldName, string paramName, string value)
        {
            return GetLikeCaluseInternal(fieldName, paramName, value, LikeType.Full);
        }

        public static Result GetLikeLeftCaluse(string fieldName, string paramName, string value)
        {
            return GetLikeCaluseInternal(fieldName, paramName, value, LikeType.Left);
        }

        public static Result GetLikeRightCaluse(string fieldName, string paramName, string value)
        {
            return GetLikeCaluseInternal(fieldName, paramName, value, LikeType.Right);
        }

        private enum LikeType
        {
            Full,
            Left,
            Right
        }

        private static Result GetLikeCaluseInternal(string fieldName, string paramName, string value, LikeType likeType)
        {
            const string escapeChar = @"\";
            var dp = new DynamicParameters();
            dp.Add(paramName, ToLikeString(value, likeType, escapeChar));
            return new Result($@"{fieldName} LIKE @{paramName} ESCAPE '{escapeChar}'", dp);
        }

        private static string ToLikeString(string input, LikeType likeType, string escapeChar)
        {
            return
                likeType == LikeType.Right
                ?
                input.ToLikeRightString(escapeChar)
                :
                    likeType == LikeType.Left
                    ?
                    input.ToLikeLeftString(escapeChar)
                    :
                    input.ToLikeString(escapeChar);
        }

        public static Result GetWhereStringWithParams(IReadOnlyCollection<string> criteria, DynamicParameters dp)
        {
            var where = criteria.Count == 0 ? string.Empty : $" WHERE {string.Join(" AND ", criteria)} ";
            return new Result(@where, dp);
        }

        public static string GetTranslatedSort(string modelColumn, string defaultSort, IReadOnlyCollection<string> allowedColumns)
        {
            if (string.IsNullOrEmpty(modelColumn))
                return defaultSort.ToUpperInvariant();
            var arguments = modelColumn.Split(' ');
            if (arguments.Length != 2)
                return defaultSort.ToUpperInvariant();
            var ascending = arguments[1].ToUpperInvariant() == "ASC";
            var column = arguments[0].ToUpperInvariant();
            if (!allowedColumns.Select(c => c.ToUpperInvariant()).Contains(column))
                return defaultSort.ToUpperInvariant();
            return $"{column} {(@ascending ? "ASC" : "DESC")}";
        }


    }
}
