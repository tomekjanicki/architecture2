using System.Collections.Generic;
using Architecture2.Common.Tool;

namespace Architecture2.Common.SharedStruct
{
    public class Paged<T>
    {
        public Paged(int count, IReadOnlyCollection<T> items)
        {
            Guard.InRange(nameof(count), count < 0, $"{nameof(count)} has to be equal or greater than 0, but it is {count}");
            Guard.NotNull(items, nameof(items));
            Count = count;
            Items = items;
        }

        public int Count { get; private set; }

        public IReadOnlyCollection<T> Items { get; private set; }
    }
}