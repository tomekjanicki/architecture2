using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Logic.Base;

namespace Architecture2.Common.Exception.Logic
{
    [Serializable]
    public class UniqueConstraintException<T> : BaseLogicException<T>
    {
        public string Name { get; set; }
        public UniqueConstraintException(string key) : base(key)
        {
        }

        public UniqueConstraintException(string key, System.Exception innerException) : base(key, innerException)
        {
        }

        public UniqueConstraintException(SerializationInfo info, StreamingContext context) : base(info, context)
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

        protected override string Text => "Unique Constraint";
    }
}