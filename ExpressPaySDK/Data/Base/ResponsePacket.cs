
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpressPay.SDK.Data.Base
{
    internal class ResponsePacket<T> : IResposnePacket
       where T : IResponseMessage, new()
    {
        public string[] Errors { get; set; }
        public Error Error { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Тело ответа при получении списка.
        /// </summary>
        [JsonProperty("Items")]
        public IList<T> Items { get; set; }

        /// <summary>
        /// Тело ответа при получении деталей.
        /// </summary>
        public T Value { get; set; }

        public ResponsePacket()
        {
            Items = new List<T>();
            Value = new T();
        }
    }
}
