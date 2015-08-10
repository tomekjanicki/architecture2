using System;

namespace Architecture2.Common.Tool
{
    public static class Guard
    {
        public static void InRange(string paramName, bool notInRange, string errorMessage)
        {
            if (notInRange)
                throw new ArgumentOutOfRangeException(paramName, errorMessage);
        }

        public static void NotNull<T>(T obj, string paramName) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }

        public static void NotNullOrEmpty(string obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            if (obj == string.Empty)
                throw new ArgumentException(paramName);
        }
    }
}