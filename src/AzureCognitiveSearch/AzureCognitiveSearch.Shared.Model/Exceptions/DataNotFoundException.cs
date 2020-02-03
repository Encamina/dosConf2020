using System;
using System.Runtime.Serialization;

namespace AzureCognitiveSearch.Shared.Model.Exceptions
{
    [Serializable]
    public class DataNotFoundException : BaseException
    {
        public DataNotFoundException()
        {
        }

        public DataNotFoundException(string message) : base(message)
        {
        }

        public DataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataNotFoundException(string documentId, string message, Exception innerException) : base(message, AddInfo(message, documentId), innerException)
        {
        }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string AddInfo(string message, string documentId)
        {
            return $"{message}. Document Id: {documentId}";
        }
    }
}
