using System;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Check if the date is the min value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        /// Null if the date specified is the minimum value of DateTimeOffset.
        /// The DateTimeObject if the value is not the minimum.
        /// </returns>
        public static DateTimeOffset? GetValueOrNull(this DateTimeOffset? date)
        {
            if (date.HasValue)
            {
                var compareValue = DateTimeOffset.Compare(date.Value, DateTimeOffset.MinValue);
                var compareValueWithDateTime = DateTime.Compare(date.Value.DateTime, DateTime.MinValue);
                if (compareValue == 0 || compareValueWithDateTime == 0)
                {
                    return null;
                }
                else
                {
                    return date;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
