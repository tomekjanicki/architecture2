using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Base;

namespace Architecture2.Common.Database.Exception
{
    [Serializable]
    public class DbException : BaseException
    {

        public DbException(System.Exception innerException)
            : base("Error during handling repository operation. See inner exception for details", innerException)
        {
        }

        public DbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}