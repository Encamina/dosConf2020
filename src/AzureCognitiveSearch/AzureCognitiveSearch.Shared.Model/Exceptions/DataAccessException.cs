using System;
using System.Runtime.Serialization;

namespace AzureCognitiveSearch.Shared.Model.Exceptions
{
    [Serializable]
    public class DataAccessException : BaseException
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }

        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataAccessException(string documentId, string message, Exception innerException) : base(message, AddInfo(message, documentId), innerException)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string AddInfo(string message, string documentId)
        {
            return $"{message}. Document Id: {documentId}";
        }
    }
}
