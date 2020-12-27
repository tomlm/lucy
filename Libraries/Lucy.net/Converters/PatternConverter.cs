using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lucy
{
    /// <summary>
    /// JsonConverter which allows a pattern to be a string or array of strings.
    /// </summary>
    public class PatternConverter : JsonConverter<Pattern>
    {
        public override bool CanRead => true;

        public override Pattern ReadJson(JsonReader reader, Type objectType, Pattern existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                return new Pattern((string)reader.Value);
            }
            else
            {
                return new Pattern(JArray.Load(reader).ToObject<string[]>());
            }
        }

        public override void WriteJson(JsonWriter writer, Pattern value, JsonSerializer serializer)
        {
            if (value.Count() > 1)
            {
                serializer.Serialize(writer, value.ToArray());
            }
            else
            {
                serializer.Serialize(writer, value.First());
            }
        }
    }
}
