using System;
using System.Runtime.Serialization;

namespace Architecture2.Common.Exception
{
    [Serializable]
    public abstract class BaseException : System.Exception
    {
        protected BaseException(string message) : base(message)
        {

        }

        protected BaseException(string messsage, System.Exception innerException) : base(messsage, innerException)
        {

        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

    }
}
