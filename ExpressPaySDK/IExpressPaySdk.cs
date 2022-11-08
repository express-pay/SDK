using ExpressPay.SDK.Data.Response;
using ExpressPay.SDK.Enums;
using ExpressPay.SDK.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ExpressPay.SDK
{
    /// <summary>
    /// Клиентский интерфейс для использования API
    /// </summary>
    public interface IExpressPaySdk : IDisposable
    {
        /// <summary>
        /// Список счетов по парметрам
        /// Если параметры не заданы возращаются платежи за полседние 30 дней
        /// </summary>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="from">Дата выставления с - начало периода Формат - yyyyMMdd</param>
        /// <param name="to">Дата выставления по - конец периода Формат - yyyyMMdd</param>
        /// <param name="status">Статус счета на оплату Формат - yyyyMMdd</param>
        /// <param name="dateFormat">Формат парсинга даты</param>
        /// <returns>Список счетов <see cref="InvoiceItem"/></returns>
        Task<IList<InvoiceItem>> GetInvoicesAsync(
            string accountNo = null,
            DateTime? from = null,
            DateTime? to = null,
            int? status = null,
            ParseDateFormat dateFormat = ParseDateFormat.Short);

        /// <summary>
        /// Выставление счета
        /// </summary>
        /// <param name="accountNo">Номер лицевого счета. Разрешенные символы: цифры, буквы(латиница, кириллица), 
        /// тире(-), точка(.), правый слэш(/), левый слэш(\), двоеточие(:) и нижнее подчеркивание(_).</param>
        /// <param name="amount">Сумма счета на оплату.Разделителем дробной и целой части является символ запятой</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="expiration">Дата истечения срока действия выставления счета на оплату.
        /// Форматы – yyyyMMdd, yyyyMMddHHmm</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="city">Город</param>
        /// <param name="street">Улица</param>
        /// <param name="house">Дом</param>
        /// <param name="building">Корпус</param>
        /// <param name="apartment">Квартира</param>
        /// <param name="isNameEditable">При оплате разрешено изменять ФИО плательщика</param>
        /// <param name="isAddressEditable">При оплате разрешено изменять адрес плательщика</param>
        /// <param name="isAmountEditable">	При оплате разрешено изменять сумму оплаты</param>
        /// <param name="emailNotification"></param>
        /// <param name="smsPhone"></param>
        /// <param name="returnInvoiceUrl"></param>
        /// <returns><see cref="AddInvoiceResponse"/></returns>
        Task<AddInvoiceResponse> AddInvoiceAsync(
                string accountNo,
                decimal amount,
                int currency,
                string expiration = null,
                string info = null,
                string surname = null,
                string firstName = null,
                string patronymic = null,
                string city = null,
                string street = null,
                string house = null,
                string building = null,
                string apartment = null,
                bool isNameEditable = false,
                bool isAddressEditable = false,
                bool isAmountEditable = false,
                string emailNotification = null,
                string smsPhone = null,
                bool returnInvoiceUrl = false);

        /// <summary>
        /// Номер счета полученный при выставлении
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="returnInvoiceUrl"></param>
        /// <returns><see cref="InvoiceDetailsResponse"/></returns>
        Task<InvoiceDetailsResponse> GetInvoiceDetailsAsync(int invoiceNo, bool returnInvoiceUrl = false);

        /// <summary>
        /// Статуса счета
        /// </summary>
        /// <param name="invoiceNo">Номер счета полученный при выставлении</param>
        /// <returns><see cref="StatusResponse"/></returns>
        Task<StatusResponse> GetInvoiceStatusAsync(int invoiceNo);

        /// <summary>
        /// Отмена счета
        /// </summary>
        /// <param name="invoiceNo">Номер счета полученный при выставлении</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        Task<HttpStatusCode> CancelInvoiceAsync(int invoiceNo);

        /// <summary>
        /// Получение Qr-кода
        /// </summary>
        /// <param name="invoiceNo">Номер счета</param>
        /// <param name="viewType">Тип возвращаемого значения</param>
        /// <param name="imageWidth">Ширина qr-кода</param>
        /// <param name="imageHeight">Высота qr-кода</param>
        /// <returns><see cref="QrCodeResponse"/></returns>
        Task<QrCodeResponse> GetQrCodeAsync(
            int invoiceNo,
            ViewType viewType = ViewType.Base64,
            int? imageWidth = null,
            int? imageHeight = null);

        /// <summary>
        /// Получение Qr-кода для лицевого счета
        /// </summary>
        /// <param name="accountNumber">Номер лицевого счета</param>
        /// <param name="viewType">Тип возвращаемого значения</param>
        /// <param name="imageWidth">Ширина qr-кода</param>
        /// <param name="imageHeight">Высота qr-кода</param>
        /// <param name="amount">Сумма для оплаты</param>
        /// <returns><see cref="QrCodeResponse"/></returns>
        Task<QrCodeResponse> GetQrCodeByAccountNumberAsync(
            string accountNumber,
            ViewType viewType = ViewType.Base64,
            int? imageWidth = null,
            int? imageHeight = null,
            decimal amount = 0);

        /// <summary>
        /// Выставить счет для оплаты по карте
        /// </summary>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="expiration">Дата истечения срока действия выставленного счета на оплату. Формат - yyyyMMdd</param>
        /// <param name="amount">	Сумма счета на оплату. Сумма счета должна быть не менее 1,00 BYN.
        /// Разделителем дробной и целой части является символ запятой</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="returnUrl">Адрес для перенаправления пользователя в случае успешной оплаты</param>
        /// <param name="failUrl">Адрес для перенаправления пользователя в случае неуспешной оплаты</param>
        /// <param name="language">Язык в кодировке ISO 639-1. Если не указан, будет использован язык по умолчанию</param>
        /// <param name="sessionTimeoutSec">Продолжительность сессии в секундах.
        /// В случае если параметр не задан, будет использовано значение 1200 секунд(20 минут).
        /// Если в запросе присутствует параметр ExpirationDate, то значение параметра SessionTimeoutSecs не учитывается.
        /// Значение SessionTimeoutSecs должно находится в пределах от 60 до 1200 сек(1-20 мин).</param>
        /// <param name="expirationDate">Время жизни заказа. Формат yyyyMMddHHmmss. Если этот параметр не передаётся в 
        /// запросе, то для определения времени жизни сессии используется SessionTimeoutSecs.</param>
        /// <param name="returnInvoiceUrl">Вернуть в ответе публичную ссылку на счет</param>
        /// <returns><see cref="CardAddInvoiceResponse"/></returns>
        Task<CardAddInvoiceResponse> AddCardInvoiceAsync(
                string accountNo,
                decimal amount,
                int currency,
                string info,
                string returnUrl,
                string failUrl,
                string expiration = null,
                string language = "ru",
                int? sessionTimeoutSec = null,
                string expirationDate = null,
                bool returnInvoiceUrl = false);

        /// <summary>
        /// Ссылку на форму оплаты для оплаты счета по карте
        /// </summary>
        /// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <returns><see cref="PaymentFormResponse"/></returns>
        Task<PaymentFormResponse> GetPaymentFormAsync(int cardInvoiceNo);

        /// <summary>
        /// Статус счета по карте
        /// </summary>
        /// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <param name="language">Язык в кодировке ISO 639-1. Если не указан, считается, что язык – русский. 
        /// Сообщение об ошибке будет возвращено именно на этом языке.</param>
        /// <returns><see cref="CardStatusResponse"/></returns>
        Task<CardStatusResponse> GetCardInvoceStatusAsync(int cardInvoiceNo, string language = "ru");

        /// <summary>
        /// Отмена платежа по карте 
        /// </summary>
        /// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        Task<HttpStatusCode> ReverseCardPaymentAsync(int cardInvoiceNo);

        /// <summary>
        /// Выставить новый счет. В данном методе цифровая подпись является обязательным параметром.
        /// </summary>
        /// <param name="serviceId">Номер услуги</param>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="amount">Сумма счета на оплату. Разделителем дробной и целой части является символ запятой</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="returnUrl">Адрес, на который происходит перенаправление после успешного выставления счета</param>
        /// <param name="failUrl">Адрес, на который происходит перенаправление при ошибке выставления счета</param>
        /// <param name="expiration">Дата истечения срока действия выставлена счета на оплату. Формат - yyyyMMdd</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="city">Улица</param>
        /// <param name="street">Дом</param>
        /// <param name="house">Улица</param>
        /// <param name="building">Корпус</param>
        /// <param name="apartment">Квартира</param>
        /// <param name="isNameEditable">При оплате разрешено изменять ФИО плательщика 0 – нет, 1 – да</param>
        /// <param name="isAddressEditable">При оплате разрешено изменять адрес плательщика 0 – нет, 1 – да</param>
        /// <param name="isAmountEditable">При оплате разрешено изменять сумму оплаты 0 – нет, 1 – да</param>
        /// <param name="emailNotification">Адрес электронной почты, на который будет отправлено уведомление о выставлении счета</param>
        /// <param name="smsPhone">Номер мобильного телефона, на который будет отправлено SMS-сообщение о выставлении счета</param>
        /// <param name="returnInvoiceUrl">Вернуть в ответе публичную ссылку на счет</param>
        /// 
        /// <returns><see cref="AddInvoiceResponse"/></returns>
        Task<WebAddInvoiceResponse> AddWebInvoiceAsync(
                int serviceId,
                string accountNo,
                decimal amount,
                int currency,
                string returnUrl,
                string failUrl,
                string expiration = null,
                string info = null,
                string surname = null,
                string firstName = null,
                string patronymic = null,
                string city = null,
                string street = null,
                string house = null,
                string building = null,
                string apartment = null,
                bool isNameEditable = false,
                bool isAddressEditable = false,
                bool isAmountEditable = false,
                string emailNotification = null,
                string smsPhone = null,
                bool returnInvoiceUrl = false);

        /// <summary>
        /// Выставить счет для оплаты по карте. В данном методе цифровая подпись является обязательным параметром.
        /// </summary>
        /// <param name="serviceId">Номер услуги</param>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="amount">Сумма счета на оплату. Разделителем дробной и целой части является символ запятой</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="returnUrl">Адрес, на который происходит перенаправление после успешного выставления счета</param>
        /// <param name="failUrl">Адрес, на который происходит перенаправление при ошибке выставления счета</param>
        /// <param name="expiration">Дата истечения срока действия выставлена счета на оплату. Формат - yyyyMMdd</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="returnInvoiceUrl">Вернуть в ответе публичную ссылку на счет</param>
        /// <param name="customerId">Id клиента соверщающего платёж.
        /// Используется для использования функции "Упрощенный платеж"
        ///(Примечание: только для случая, использования WebPay)</param>
        /// <returns><see cref="WebAddCardInvoiceResponse"/></returns>
        Task<WebAddCardInvoiceResponse> AddWebCardInvoiceAsync(
                int serviceId,
                string accountNo,
                decimal amount,
                int currency,
                string returnUrl,
                string failUrl,
                string info,
                string expiration = null,
                bool returnInvoiceUrl = false,
                int? customerId = null);

        /// <summary>
        /// Список платежей
        /// Если параметры не заданы возращаются платежи за полседние 30 дней
        /// </summary>
        /// <param name="from">Дата оплаты с – начало периода. Формат - yyyyMMdd</param>
        /// <param name="to">Дата оплаты по – конец периода. Формат - yyyyMMdd</param>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="dateFormat">Формат парсинга даты</param>
        /// <returns>Список платежей <see cref="PaymentItem"/></returns>
        Task<IList<PaymentItem>> GetPaymentsAsync(
            string accountNo = null,
            DateTime? from = null,
            DateTime? to = null,
            ParseDateFormat dateFormat = ParseDateFormat.Short);

        /// <summary>
        /// Детали платежа
        /// </summary>
        /// <param name="paymentNo">Номер платежа</param>
        /// <returns><see cref="PaymentDeatailResponse"/></returns>
        Task<PaymentDeatailResponse> GetPaymentDetailAsync(int paymentNo);

        /// <summary>
        /// Баланс по лицевому счету
        /// </summary>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <returns><see cref="BalanceResponse"/></returns>
        Task<BalanceResponse> GetBalanceAsync(string accountNo);

        /// <summary>
        /// Выставить новый счет. В данном методе цифровая подпись является обязательным параметром.
        /// </summary>
        /// <param name="serviceId">Номер услуги</param>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="amount">Сумма счета на оплату. Разделителем дробной и целой части является символ запятой</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="returnUrl">Адрес, на который происходит перенаправление после успешного выставления счета</param>
        /// <param name="failUrl">Адрес, на который происходит перенаправление при ошибке выставления счета</param>
        /// <param name="expiration">Дата истечения срока действия выставлена счета на оплату. Формат - yyyyMMdd</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="city">Улица</param>
        /// <param name="street">Дом</param>
        /// <param name="house">Улица</param>
        /// <param name="building">Корпус</param>
        /// <param name="apartment">Квартира</param>
        /// <param name="isNameEditable">При оплате разрешено изменять ФИО плательщика 0 – нет, 1 – да</param>
        /// <param name="isAddressEditable">При оплате разрешено изменять адрес плательщика 0 – нет, 1 – да</param>
        /// <param name="isAmountEditable">При оплате разрешено изменять сумму оплаты 0 – нет, 1 – да</param>
        /// <param name="emailNotification">Адрес электронной почты, на который будет отправлено уведомление о выставлении счета</param>
        /// <param name="smsPhone">Номер мобильного телефона, на который будет отправлено SMS-сообщение о выставлении счета</param>
        /// <param name="customerId">Id клиента соверщающего платёж.
        /// Используется для использования функции "Упрощенный платеж"
        ///(Примечание: только для случая, использования WebPay)</param>
        /// 
        /// <returns><see cref="WebAddInvoiceResponse"/></returns>
        Task<WebAddInvoiceResponse> AddInvoiceV2Async(
            int serviceId,
            string accountNo,
            decimal amount,
            int currency,
            string returnUrl,
            string failUrl,
            string expiration = null,
            string info = null,
            string surname = null,
            string firstName = null,
            string patronymic = null,
            string city = null,
            string street = null,
            string house = null,
            string building = null,
            string apartment = null,
            bool isNameEditable = false,
            bool isAddressEditable = false,
            bool isAmountEditable = false,
            string emailNotification = null,
            string smsPhone = null,
            int? customerId = null);

        /// <summary>
        /// Метод выполняет инициирующий платеж для привязки карты.
        /// </summary>
        /// <param name="serviceId">Номер услуги</param>
        /// <param name="writeOffPeriod">Период через который необходимо произвести следующие списание</param>
        /// <param name="amount">Сумма счета на оплату</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="info">Назначение платежа</param>
        /// <param name="returnUrl">Адрес, на который происходит перенаправление после успешного выставления счета</param>
        /// <param name="failUrl">Адрес, на который происходит перенаправление при ошибке выставления счета</param>
        /// <param name="language">Язык в кодировке ISO 639-1</param>
        /// <returns><see cref="BindResponse"/></returns>
        Task<BindResponse> RecurringPaymentBindAsync(
            int serviceId,
            WriteOffPeriod writeOffPeriod,
            decimal amount,
            int currency,
            string info,
            string returnUrl,
            string failUrl,
            string language = "ru");

        /// <summary>
        /// Метод выполняет отвязку карты.
        /// </summary>
        /// <param name="customerId">Id привязки</param>
        /// <param name="serviceId">Номер услуги</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        Task<HttpStatusCode> RecurringPaymentUnbindAsync(
            int customerId,
            int serviceId);
    }
}
