using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Logic.Base;

namespace Architecture2.Common.Exception.Logic
{
    [Serializable]
    public class NotFoundException<T> : BaseLogicException<T>
    {
        public NotFoundException(string key) : base(key)
        {
        }

        public NotFoundException(string key, System.Exception innerException) : base(key, innerException)
        {
        }

        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override string Text => "Not found";
    }
}
