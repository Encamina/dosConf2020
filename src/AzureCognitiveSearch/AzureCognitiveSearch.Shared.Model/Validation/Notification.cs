using AzureCognitiveSearch.Shared.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveSearch.Shared.Model.Validation
{
    public class Notification
    {
        private readonly List<string> errors;

        public Notification() => errors = new List<string>();

        public Notification(Notification baseNotification) => errors = new List<string>(baseNotification.GetErrors());

        public void AddError(string message) => errors.Add(message);

        public void Join(IList<string> newErrors)
        {
            errors.AddRange(newErrors);
        }

        public IList<string> GetErrors() => errors;

        public bool HasErrors => errors.Any();

        public void ThrowIfErrorsDetected()
        {
            if (HasErrors)
            {
                throw new ValidationException(this);
            }
        }
    }
}
