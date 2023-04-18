using System.Runtime.Serialization;

namespace QueueStorageSubscriber.Exceptions
{
    [Serializable]
    internal sealed class DequeueMessageException : Exception
    {
        public DequeueMessageException()
        { }

        public DequeueMessageException(string message)
            : base(message)
        { }

        public DequeueMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public DequeueMessageException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}