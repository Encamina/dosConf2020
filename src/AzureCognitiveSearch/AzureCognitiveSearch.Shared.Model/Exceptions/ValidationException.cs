using AzureCognitiveSearch.Shared.Model.Validation;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace AzureCognitiveSearch.Shared.Model.Exceptions
{
    [Serializable]
    public class ValidationException : BaseException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }
        public ValidationException(Notification notification) : base("There were validation errors", GetMessage(notification))
        {
        }


        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string GetMessage(Notification notification)
        {
            var stringBuilder = new StringBuilder();

            foreach (var error in notification.GetErrors())
            {
                stringBuilder.AppendLine(error);
            }

            return stringBuilder.ToString();
        }
    }
}
