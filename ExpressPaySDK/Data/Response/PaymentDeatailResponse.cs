using ExpressPay.SDK.Data.Base;
using ExpressPay.SDK.Data.Converters;
using Newtonsoft.Json;
using System;

namespace ExpressPay.SDK.Data.Response
{
    /// <summary>
    /// Детальная информация по платежу
    /// </summary>
    public class PaymentDeatailResponse : IResponseMessage
    {
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// Дата/Время платежа
        /// </summary>
        [JsonConverter(typeof(StringToDateTimeJsonConverter))]
        public DateTime Created { get; set; }
        /// <summary>
        /// Сумма платежа
        /// </summary>
        [JsonConverter(typeof(StringToDecimalJsonConverter))]
        public decimal Amount { get; set; }
        /// <summary>
        /// Код валюты
        /// </summary>
        public int Currency { get; set; }
        /// <summary>
        /// Назначение платежа
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; set; }
        /// <summary>
        /// Корпус
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// Квартира
        /// </summary>
        public string Apartment { get; set; }
        /// <summary>
        /// Номер платежного документа
        /// </summary>
        public string DocumentNumber { get; set; }
        /// <summary>
        /// Дата платежного документа
        /// </summary>
        [JsonConverter(typeof(StringToDateTimeJsonConverter))]
        public DateTime? DocumentDate { get; set; }
        /// <summary>
        /// Сумма перечисленная
        /// </summary>      
        [JsonConverter(typeof(StringToDecimalJsonConverter))]
        public decimal? TransferredAmount { get; set; }
        /// <summary>
        /// Сумма комсии (для платежей через ЕРИП).
        /// </summary>
        [JsonConverter(typeof(StringToDecimalJsonConverter))]
        public decimal? EripComission { get; set; }
    }
}
