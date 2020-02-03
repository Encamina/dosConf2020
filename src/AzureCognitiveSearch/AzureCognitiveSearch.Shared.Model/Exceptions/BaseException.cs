using System;
using System.Runtime.Serialization;

namespace AzureCognitiveSearch.Shared.Model.Exceptions
{
    [Serializable]
    public class BaseException : Exception
    {
        public string ErrorDetails { get; set; }

        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BaseException(string message, string errorDetails, Exception innerException) : base(message, innerException)
        {
            this.ErrorDetails = errorDetails;
        }

        public BaseException(string message, string errorDetails) : base(message)
        {
            this.ErrorDetails = errorDetails;
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
