﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Architecture2.Common.Tool
{
    public static class Extension
    {
        public static bool AreEqual<T>(IEnumerable<T> ar1, IEnumerable<T> ar2)
        {
            if (ar1 == null || ar2 == null)
                return false;
            return ar1.SequenceEqual(ar2);
        }

        public static string GetConvertedValue(byte[] bytes)
        {
            return bytes == null ? string.Empty : Convert.ToBase64String(bytes);
        }

        public static bool IsType(object obj, Type type)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(type, nameof(type));
            var objectType = obj.GetType();
            return 
                objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == type || IsSubclassOfRawGeneric(type, objectType)) 
                || 
                !objectType.IsGenericType && (objectType == type || objectType.IsSubclassOf(type)) ;
        }

        public static bool IsType(object obj, IReadOnlyCollection<Type> types)
        {
            Guard.NotNull(obj, nameof(obj));
            Guard.NotNull(types, nameof(types));
            return types.Any(type => IsType(obj, type));
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
