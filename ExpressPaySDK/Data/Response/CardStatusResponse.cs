using ExpressPay.SDK.Data.Base;
using ExpressPay.SDK.Data.Converters;
using Newtonsoft.Json;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Ответ на получение статуса счёта по карте
    /// </summary>
    public class CardStatusResponse : IResponseMessage
    {
        /// <summary>
        /// Сумма оплаты
        /// </summary>
        [JsonConverter(typeof(StringToDecimalJsonConverter))]
        public decimal Amount { get; set; }

        /// <summary>
        /// Состояние оплаты
        /// </summary>
        public int CardInvoiceStatus { get; set; }
    }
}
