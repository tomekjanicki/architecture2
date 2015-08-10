using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Logic.Base;

namespace Architecture2.Common.Exception.Logic
{
    [Serializable]
    public class ForeignKeyException<T> : BaseLogicException<T>
    {
        public string Name { get; set; }
        public ForeignKeyException(string key) : base(key)
        {
        }

        public ForeignKeyException(string key, System.Exception innerException) : base(key, innerException)
        {
        }

        public ForeignKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Name = info.GetString(nameof(Name));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Name), Name);
        }

        public override string Message
        {
            get
            {
                var message = base.Message;
                return $"{message}\r\nName: {Name}";
            }
        }

        protected override string Text => "Foreign Key";
    }
}