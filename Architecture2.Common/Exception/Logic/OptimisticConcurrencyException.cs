using System;
using System.Runtime.Serialization;
using Architecture2.Common.Exception.Logic.Base;
using Architecture2.Common.Tool;

namespace Architecture2.Common.Exception.Logic
{
    [Serializable]
    public class OptimisticConcurrencyException<T> : BaseLogicException<T>
    {
        public byte[] ExpectedVersion { get; }

        public byte[] ActualVersion { get; }

        public OptimisticConcurrencyException(string key, byte[] expectedVersion, byte[] actualVersion) : base(key)
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
        }

        public OptimisticConcurrencyException(string key, byte[] expectedVersion, byte[] actualVersion, System.Exception innerException) : base(key, innerException)
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
        }

        public OptimisticConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ExpectedVersion = (byte[])info.GetValue(nameof(ExpectedVersion), typeof(byte[]));
            ActualVersion = (byte[])info.GetValue(nameof(ActualVersion), typeof(byte[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ExpectedVersion), ExpectedVersion);
            info.AddValue(nameof(ActualVersion), ActualVersion);
        }

        public override string Message
        {
            get
            {
                var message = base.Message;
                return $"{message}\r\nExpectedVersion: {Extension.GetConvertedValue(ExpectedVersion)}\r\nActualVersion: {Extension.GetConvertedValue(ActualVersion)}";
            }
        }

        protected override string Text => "Optimistic Concurrency";

    }
}