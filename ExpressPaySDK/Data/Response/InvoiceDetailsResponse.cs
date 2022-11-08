using ExpressPay.SDK.Data.Base;
using ExpressPay.SDK.Data.Converters;
using Newtonsoft.Json;
using System;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Детальная информация по счёту
    /// </summary>
    public class InvoiceDetailsResponse : IResponseMessage
    {
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// Статус счета на оплату
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Дата/Время выставления счета
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
        /// При оплате разрешено изменять ФИО плательщика
        /// </summary>
        public bool IsNameEditable { get; set; }
        /// <summary>
        /// При оплате разрешено изменять адрес плательщика
        /// </summary>
        public bool IsAddressEditable { get; set; }
        /// <summary>
        /// При оплате разрешено изменять сумму оплаты
        /// </summary>
        public bool IsAmountEditable { get; set; }
        /// <summary>
        /// Публичная ссылка на счет.
        /// </summary>
        public string InvoiceUrl { get; set; }
    }
}
