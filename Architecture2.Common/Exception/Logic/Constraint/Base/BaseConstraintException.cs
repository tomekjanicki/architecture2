using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Logic.Base;

namespace Architecture2.Common.Exception.Logic.Constraint.Base
{
    [Serializable]
    public abstract class BaseConstraintException<T> : BaseLogicException<T>
    {
        public string Name { get; set; }

        protected BaseConstraintException(string key) : base(key)
        {
        }

        protected BaseConstraintException(string key, System.Exception innerException) : base(key, innerException)
        {
        }

        protected BaseConstraintException(SerializationInfo info, StreamingContext context) : base(info, context)
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
    }
}
