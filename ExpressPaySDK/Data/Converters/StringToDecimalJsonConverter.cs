using Newtonsoft.Json;
using System;

namespace ExpressPay.SDK.Data.Converters
{
    internal class StringToDecimalJsonConverter : JsonConverter
    {
        /// <summary>
        /// Ensures that this is decorating a boolean type property
        /// </summary>
        /// <param name="objectType">The type of the decorated property</param>
        /// <returns>True if the property can be converted</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(decimal) == objectType;
        }

        /// <summary>
        /// Reads a JSON field into a boolean property
        /// </summary>
        /// <param name="reader">Json reader</param>
        /// <param name="objectType">Type of the object we are reading</param>
        /// <param name="existingValue">Existing value of object being read</param>
        /// <param name="serializer">The serialiser calling this method</param>
        /// <returns>Boolean representation of the Json as an object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = reader.Value.ToString();

            if (decimal.TryParse(str, out var result))
                return result;

            return decimal.Parse(str.Replace(".", ","));
        }

        /// <summary>
        /// Writes the decorated boolean property as a 1 or a 0, instead of True or False respectively
        /// </summary>
        /// <param name="writer">Writer used to output value</param>
        /// <param name="value">The value of the property</param>
        /// <param name="serializer">The serialiser calling this method</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
