using ExpressPay.SDK.Data.Base;
using ExpressPay.SDK.Data.Converters;
using Newtonsoft.Json;
using System;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Счет на оплату
    /// </summary>
    public class InvoiceItem : IResponseMessage
    {
        /// <summary>
        /// Номер счета на оплату
        /// </summary>
        public int InvoiceNo { get; set; }
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// Статус счета на оплату
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Дата выставления счета.
        /// </summary>
        [JsonConverter(typeof(StringToDateTimeJsonConverter))]
        public DateTime Created { get; set; }
        /// <summary>
        /// Дата истечения срока действия выставления счета на оплату.
        /// </summary>
        [JsonConverter(typeof(StringToDateTimeJsonConverter))]
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// Сумма счета на оплату
        /// </summary>
        [JsonConverter(typeof(StringToDecimalJsonConverter))]
        public decimal Amount { get; set; }
        /// <summary>
        /// Код валюты
        /// </summary>
        public int Currency { get; set; }
        /// <summary>
        /// Номер счета по карте.
        /// </summary>
        public int? CardInvoiceNo { get; set; }
    }
}
