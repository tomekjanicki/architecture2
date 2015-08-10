using System;
using Architecture2.Common.Tool;

namespace Architecture2.Common.SharedStruct
{
    public class Page : IEquatable<Page>
    {
        public readonly int PageSize;
        public readonly int Skip;

        public Page(int pageSize, int skip)
        {
            Guard.InRange(nameof(pageSize), pageSize > Const.MaxPageSize || pageSize < Const.MinPageSize, $"{nameof(pageSize)} has to be between {Const.MinPageSize} and {Const.MaxPageSize}. The value passed is {pageSize}.");
            Guard.InRange(nameof(skip), skip < 0, $"{nameof(skip)} has to be equal or greater than 0, but it is {skip}");
            PageSize = pageSize;
            Skip = skip;
        }

        public static Page FromPageAndSortCriteria(PageAndSortCriteria pageAndSortCriteria)
        {
            return new Page(pageAndSortCriteria.PageSize, pageAndSortCriteria.Skip);
        }

        public bool Equals(Page other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return PageSize == other.PageSize && Skip == other.Skip;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Page)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + PageSize.GetHashCode();
                hash = hash * 23 + Skip.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Page left, Page right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Page left, Page right)
        {
            return !Equals(left, right);
        }


    }
}