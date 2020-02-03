using System;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToISOString(this DateTime date)
        {
            return date.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
