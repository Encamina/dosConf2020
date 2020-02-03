using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;

namespace AzureCognitiveSearch.Shared.Utils.Json
{
    public class JsonValidEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type enumType = (Nullable.GetUnderlyingType(objectType) ?? objectType);
            var value = reader.Value;
            if (value == null)
            {
                throw new ArgumentException("Invalid enum value", new Exception("Invalid enum value"));
            }

            if (Enum.GetNames(enumType).Any(x => x.ToLowerInvariant() == value.ToString().ToLowerInvariant()))
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            else
            {
                throw new ArgumentException("Invalid enum value", new Exception("Invalid enum value"));
            }
        }
    }
}
