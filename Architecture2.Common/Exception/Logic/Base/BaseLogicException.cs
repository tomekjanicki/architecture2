using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Base;

namespace Architecture2.Common.Exception.Logic.Base
{
    [Serializable]
    public abstract class BaseLogicException<T> : BaseException
    {
        public string Key { get; }
        protected abstract string Text { get; }

        protected BaseLogicException(string key) : base(string.Empty)
        {
            Key = key;
        }

        protected BaseLogicException(string key, System.Exception innerException)
            : base(string.Empty, innerException)
        {
            Key = key;
        }

        protected BaseLogicException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Key = info.GetString(nameof(Key));
        }

        public override string Message => $"{Text}\r\nObjectType: {typeof(T).FullName}\r\nKey: {Key}";

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Key), Key);
        }

    }
}