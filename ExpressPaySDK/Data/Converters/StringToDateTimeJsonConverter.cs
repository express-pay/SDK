using Newtonsoft.Json;
using System;
using System.Globalization;

namespace ExpressPay.SDK.Data.Converters
{
    /// <summary>
    /// Ensures that boolean parameters can be read from Json
    /// </summary>
    internal class StringToDateTimeJsonConverter : JsonConverter
    {
        private const string DateFormat = "yyyyMMdd";
        private const string DateTimeFormat = "yyyyMMddHHmm";
        private const string FullDateTimeFormat = "yyyyMMddHHmmss";

        /// <summary>
        /// Ensures that this is decorating a boolean type property
        /// </summary>
        /// <param name="objectType">The type of the decorated property</param>
        /// <returns>True if the property can be converted</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime) == objectType;
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

            if (reader.Value is null) return null;

            var str = reader.Value.ToString();

            if (ParseDateTime(str, out DateTime dateTime))
                return dateTime;

            return null;
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

        private static bool ParseDateTime(string dateTime, out DateTime date)
        {
            var formats = new[] { FullDateTimeFormat, DateTimeFormat, DateFormat };

            return DateTime.TryParseExact(dateTime, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
