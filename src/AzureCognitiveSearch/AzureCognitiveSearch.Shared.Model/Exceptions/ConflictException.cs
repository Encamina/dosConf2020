using System;
using System.Runtime.Serialization;

namespace AzureCognitiveSearch.Shared.Model.Exceptions
{
    [Serializable]
    public class ConflictException : BaseException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ConflictException(string message, string errorDetails) : base(message, errorDetails)
        {
        }

        public ConflictException(string message, string errorDetails, Exception innerException) : base(message, errorDetails, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
