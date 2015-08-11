using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Base;

namespace Architecture2.Common.Exception
{
    [Serializable]
    public class DbException : BaseException
    {
        public DbException(string message) : base(message)
        {
        }

        public DbException(string messsage, System.Exception innerException)
            : base(messsage, innerException)
        {
        }

        public DbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}