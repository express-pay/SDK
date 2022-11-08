using ExpressPay.SDK.Data.Base;
using ExpressPay.SDK.Data.Extensions;
using ExpressPay.SDK.Data.Response;
using ExpressPay.SDK.Enums;
using ExpressPay.SDK.Response;
using ExpressPay.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpressPay.SDK
{
    /// <summary>
    /// SDK
    /// </summary>
    public class ExpressPaySdk : IExpressPaySdk
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://api.express-pay.by/")
        };

        private readonly string _token;
        private readonly bool _useSignature;
        private readonly string _secretWord;

        /// <summary>
        /// В конструктор передаем необходимы настройки
        /// </summary>
        /// <param name="isTest">Использовать тестовый сервер</param>
        /// <param name="token">API-ключ</param>
        /// <param name="useSignature">Использовать цифровую подпись</param>
        /// <param name="secretWord">Секретное слово</param>
        public ExpressPaySdk(bool isTest, string token, bool useSignature = false, string secretWord = null)
        {
            if (isTest)
            {
                _client.BaseAddress = new Uri("https://sandbox-api.express-pay.by/");
            }
            _token = token;
            _useSignature = useSignature;
            _secretWord = secretWord;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

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
        public async Task<IList<InvoiceItem>> GetInvoicesAsync(
            string accountNo = null,
            DateTime? from = null,
            DateTime? to = null,
            int? status = null,
            ParseDateFormat dateFormat = ParseDateFormat.Short)
        {

            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"AccountNo", accountNo},
                {"From", GetStringDateTime(from, dateFormat) },
                {"To",   GetStringDateTime(to, dateFormat) },
                {"Status", status.ToString()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-list-invoices"));
            }

            var queryString = GetQueryString(requestParams);
            var response = await _client.GetAsync($"v1/invoices?{queryString}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<InvoiceItem>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Items;
        }

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
        public async Task<AddInvoiceResponse> AddInvoiceAsync(
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
                bool returnInvoiceUrl = false)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"AccountNo", accountNo},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Expiration", expiration },
                {"Info", info },
                {"Surname", surname },
                {"FirstName", firstName },
                {"Patronymic", patronymic},
                {"City", city},
                {"Street", street},
                {"House", house},
                {"Building", building},
                {"Apartment", apartment},
                {"IsNameEditable", isNameEditable.Parse()},
                {"IsAddressEditable", isAddressEditable.Parse()},
                {"IsAmountEditable", isAmountEditable.Parse()},
                {"EmailNotification", emailNotification},
                {"SmsPhone", smsPhone},
                {"ReturnInvoiceUrl", returnInvoiceUrl.Parse()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "add-invoice"));
            }

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/invoices?token={_token}", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<AddInvoiceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Номер счета полученный при выставлении
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="returnInvoiceUrl"></param>
        /// <returns><see cref="InvoiceDetailsResponse"/></returns>
        public async Task<InvoiceDetailsResponse> GetInvoiceDetailsAsync(int invoiceNo, bool returnInvoiceUrl = false)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"InvoiceNo", invoiceNo.ToString()},
                {"ReturnInvoiceUrl", returnInvoiceUrl.Parse()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-details-invoice"));
            }

            var response = await _client.GetAsync($"v1/invoices/{invoiceNo}?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<InvoiceDetailsResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Статуса счета
        /// </summary>
        /// <param name="invoiceNo">Номер счета полученный при выставлении</param>
        /// <returns><see cref="StatusResponse"/></returns>
        public async Task<StatusResponse> GetInvoiceStatusAsync(int invoiceNo)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"InvoiceNo", invoiceNo.ToString()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "status-invoice"));
            }

            var response = await _client.GetAsync($"v1/invoices/{invoiceNo}/status?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<StatusResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Отмена счета
        /// </summary>
        /// <param name="invoiceNo">Номер счета полученный при выставлении</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        public async Task<HttpStatusCode> CancelInvoiceAsync(int invoiceNo)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"InvoiceNo", invoiceNo.ToString()},
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "cancel-invoice"));
            }

            var response = await _client.DeleteAsync($"v1/invoices/{invoiceNo}?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<StatusResponse>(responseString);

            CheckErrors(responsePacket);

            return response.StatusCode;
        }

        /// <summary>
        /// Получение Qr-кода
        /// </summary>
        /// <param name="invoiceNo">Номер счета</param>
        /// <param name="viewType">Тип возвращаемого значения</param>
        /// <param name="imageWidth">Ширина qr-кода</param>
        /// <param name="imageHeight">Высота qr-кода</param>
        /// <returns><see cref="QrCodeResponse"/></returns>
        public async Task<QrCodeResponse> GetQrCodeAsync(
            int invoiceNo,
            ViewType viewType = ViewType.Base64,
            int? imageWidth = null,
            int? imageHeight = null)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"InvoiceId", invoiceNo.ToString()},
                {"ViewType", viewType.ToString().ToLower()},
                {"ImageWidth", imageWidth.ToString()},
                {"ImageHeight", imageWidth.ToString()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-qr-code"));
            }

            var response = await _client.GetAsync($"v1/qrcode/getqrcode/?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<QrCodeResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Получение Qr-кода для лицевого счета
        /// </summary>
        /// <param name="accountNumber">Номер лицевого счета</param>
        /// <param name="viewType">Тип возвращаемого значения</param>
        /// <param name="imageWidth">Ширина qr-кода</param>
        /// <param name="imageHeight">Высота qr-кода</param>
        /// <param name="amount">Сумма для оплаты</param>
        /// <returns><see cref="QrCodeResponse"/></returns>
        public async Task<QrCodeResponse> GetQrCodeByAccountNumberAsync(
            string accountNumber,
            ViewType viewType = ViewType.Base64,
            int? imageWidth = null,
            int? imageHeight = null,
            decimal amount = 0)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"AccountNumber", accountNumber},
                {"ViewType", viewType.ToString().ToLower()},
                {"ImageWidth", imageWidth.ToString()},
                {"ImageHeight", imageWidth.ToString()},
                {"Amount", amount.ToString()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-qr-code-by-account-no"));
            }

            var response = await _client.GetAsync($"v1/qrcode/getqrcodebyaccountnumber/?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<QrCodeResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }


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
        public async Task<CardAddInvoiceResponse> AddCardInvoiceAsync(
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
                bool returnInvoiceUrl = false)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"AccountNo", accountNo},
                {"Expiration", expiration},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Info", info},
                {"ReturnUrl", returnUrl},
                {"FailUrl", failUrl},
                {"Language", language},
                {"SessionTimeoutSecs", sessionTimeoutSec.ToString()},
                {"ExpirationDate", expirationDate},
                {"ReturnInvoiceUrl", returnInvoiceUrl.Parse()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "add-card-invoice"));
            }

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/cardinvoices?token={_token}", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<CardAddInvoiceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Ссылку на форму оплаты для оплаты счета по карте
        /// </summary>
        /// <param name="cardInvoieNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <returns><see cref="PaymentFormResponse"/></returns>
        public async Task<PaymentFormResponse> GetPaymentFormAsync(int cardInvoieNo)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"CardInvoiceNo", cardInvoieNo.ToString()},
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "card-invoice-form"));
            }

            var response = await _client.GetAsync($"v1/cardinvoices/{cardInvoieNo}/payment?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<PaymentFormResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Статус счета по карте
        /// </summary>
        /// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <param name="language">Язык в кодировке ISO 639-1. Если не указан, считается, что язык – русский. 
        /// Сообщение об ошибке будет возвращено именно на этом языке.</param>
        /// <returns><see cref="CardStatusResponse"/></returns>
        public async Task<CardStatusResponse> GetCardInvoceStatusAsync(int cardInvoiceNo, string language = "ru")
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"CardInvoiceNo", cardInvoiceNo.ToString()},
                {"Language", language}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "status-card-invoice"));
            }

            var response = await _client.GetAsync($"v1/cardinvoices/{cardInvoiceNo}/status?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<CardStatusResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Отмена платежа по карте 
        /// </summary>
        /// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        public async Task<HttpStatusCode> ReverseCardPaymentAsync(int cardInvoiceNo)
        {

            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"CardInvoiceNo", cardInvoiceNo.ToString()},
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "reverse-card-invoice"));
            }

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/cardinvoices/{cardInvoiceNo}/reverse?Token={_token}", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<CardStatusResponse>(responseString);

            CheckErrors(responsePacket);

            var responseCode = response.StatusCode;

            return responseCode;
        }

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
        public async Task<WebAddInvoiceResponse> AddWebInvoiceAsync(
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
                bool returnInvoiceUrl = false)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"ServiceId", serviceId.ToString()},
                {"AccountNo", accountNo},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Expiration", expiration},
                {"Info", info},
                {"Surname", surname},
                {"FirstName", firstName},
                {"Patronymic", patronymic},
                {"City", city},
                {"Street", street},
                {"House", house},
                {"Building", building},
                {"Apartment", apartment},
                {"IsNameEditable", isNameEditable.Parse()},
                {"IsAddressEditable",isAddressEditable.Parse()},
                {"IsAmountEditable", isAmountEditable.Parse()},
                {"EmailNotification", emailNotification},
                {"SmsPhone", smsPhone},
                {"ReturnType", "json"},
                {"ReturnUrl", returnUrl},
                {"FailUrl", failUrl },
                {"ReturnInvoiceUrl", returnInvoiceUrl.Parse()}
            };

            requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "add-web-invoice"));

            // После формирования цифровой подписи удаляем API-ключ
            // т.к он не учавствует в выставлении счета 
            requestParams.Remove("Token");

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/web_invoices", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<WebAddInvoiceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

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
        public async Task<WebAddCardInvoiceResponse> AddWebCardInvoiceAsync(
                int serviceId,
                string accountNo,
                decimal amount,
                int currency,
                string returnUrl,
                string failUrl,
                string info,
                string expiration = null,
                bool returnInvoiceUrl = false,
                int? customerId = null)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"ServiceId", serviceId.ToString()},
                {"AccountNo", accountNo},
                {"Expiration", expiration},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Info", info},
                {"ReturnUrl", returnUrl },
                {"FailUrl", failUrl },
                {"ReturnType", "json" },
                {"ReturnInvoiceUrl", returnInvoiceUrl.Parse()},
                {"CustomerId", customerId.ToString()}
            };

            requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "add-webcard-invoice"));

            // После формирования цифровой подписи удаляем API-ключ
            // т.к он не учавствует в выставлении счета 
            requestParams.Remove("Token");

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/web_cardinvoices", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<WebAddCardInvoiceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;

        }

        /// <summary>
        /// Список платежей
        /// Если параметры не заданы возращаются платежи за полседние 30 дней
        /// </summary>
        /// <param name="from">Дата оплаты с – начало периода. Формат - yyyyMMdd</param>
        /// <param name="to">Дата оплаты по – конец периода. Формат - yyyyMMdd</param>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <param name="dateFormat">Формат парсинга даты</param>
        /// <returns>Список платежей <see cref="PaymentItem"/></returns>
        public async Task<IList<PaymentItem>> GetPaymentsAsync(
            string accountNo = null,
            DateTime? from = null,
            DateTime? to = null,
            ParseDateFormat dateFormat = ParseDateFormat.Short)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"From", GetStringDateTime(from, dateFormat)},
                {"To", GetStringDateTime(to, dateFormat)},
                {"AccountNo", accountNo}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-list-payments"));
            }

            var response = await _client.GetAsync($"v1/payments?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<PaymentItem>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Items;
        }

        /// <summary>
        /// Детали платежа
        /// </summary>
        /// <param name="paymentNo">Номер платежа</param>
        /// <returns><see cref="PaymentDeatailResponse"/></returns>
        public async Task<PaymentDeatailResponse> GetPaymentDetailAsync(int paymentNo)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"PaymentNo", paymentNo.ToString()}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "get-details-payment"));
            }

            var response = await _client.GetAsync($"v1/payments/{paymentNo}?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<PaymentDeatailResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Баланс по лицевому счету
        /// </summary>
        /// <param name="accountNo">Номер лицевого счета</param>
        /// <returns><see cref="BalanceResponse"/></returns>
        public async Task<BalanceResponse> GetBalanceAsync(string accountNo)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"AccountNo", accountNo}
            };

            if (_useSignature)
            {
                requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "balance"));
            }

            var response = await _client.GetAsync($"v1/balance?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<BalanceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

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
        public async Task<WebAddInvoiceResponse> AddInvoiceV2Async(
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
            int? customerId = null)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"ServiceId", serviceId.ToString()},
                {"AccountNo", accountNo},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Expiration", expiration},
                {"Info", info},
                {"Surname", surname},
                {"FirstName", firstName},
                {"Patronymic", patronymic},
                {"City", city},
                {"Street", street},
                {"House", house},
                {"Building", building},
                {"Apartment", apartment},
                {"IsNameEditable", isNameEditable.Parse()},
                {"IsAddressEditable",isAddressEditable.Parse()},
                {"IsAmountEditable", isAmountEditable.Parse()},
                {"EmailNotification", emailNotification},
                {"SmsPhone", smsPhone},
                {"ReturnType", "json"},
                {"ReturnUrl", returnUrl},
                {"FailUrl", failUrl },
                {"CustomerId", customerId.ToString() },
            };

            requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "add-web-invoice"));

            // После формирования цифровой подписи удаляем API-ключ
            // т.к он не учавствует в выставлении счета 
            requestParams.Remove("Token");

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v2/invoices", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<WebAddInvoiceResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

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
        public async Task<BindResponse> RecurringPaymentBindAsync(
            int serviceId,
            WriteOffPeriod writeOffPeriod,
            decimal amount,
            int currency,
            string info,
            string returnUrl,
            string failUrl,
            string language = "ru")
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"ServiceId", serviceId.ToString()},
                {"WriteOffPeriod", ((int)writeOffPeriod).ToString()},
                {"Amount", amount.ToString()},
                {"Currency", currency.ToString()},
                {"Info", info},
                {"ReturnType", "json"},
                {"ReturnUrl", returnUrl},
                {"FailUrl", failUrl },
                {"Language", language },
            };

            requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "recurring-payment-bind"));

            // После формирования цифровой подписи удаляем API-ключ
            // т.к он не учавствует в выставлении счета 
            requestParams.Remove("Token");

            var content = new FormUrlEncodedContent(requestParams);

            var response = await _client.PostAsync($"v1/recurringpayment/bind", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<BindResponse>(responseString);

            CheckErrors(responsePacket);

            return responsePacket.Value;
        }

        /// <summary>
        /// Метод выполняет отвязку карты.
        /// </summary>
        /// <param name="customerId">Id привязки</param>
        /// <param name="serviceId">Номер услуги</param>
        /// <returns><see cref="HttpStatusCode"/></returns>
        public async Task<HttpStatusCode> RecurringPaymentUnbindAsync(
            int customerId,
            int serviceId)
        {
            var requestParams = new Dictionary<string, string>
            {
                {"Token", _token},
                {"ServiceId", serviceId.ToString()},
                {"CustomerId", customerId.ToString()},
            };

            requestParams.Add("signature", SignatureHelper.Compute(requestParams, _secretWord, "recurring-payment-unbind"));

            // После формирования цифровой подписи удаляем API-ключ
            // т.к он не учавствует в выставлении счета 
            requestParams.Remove("Token");

            var response = await _client.DeleteAsync($"v1/recurringpayment/unbind/{customerId}?{GetQueryString(requestParams)}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responsePacket = ResponsePacketExtensions.Deserialize<BindResponse>(responseString);

            CheckErrors(responsePacket);

            return response.StatusCode;
        }

        /// <summary>
        /// Очищаем ресурсы
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }


        #region PrivateMethods
        private string GetQueryString(IDictionary<string, string> requestParams)
        {
            string queryString = string.Empty;

            using (var content = new FormUrlEncodedContent(requestParams))
            {
                queryString = content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            return queryString;
        }

        private string GetStringDateTime(DateTime? dateTime, ParseDateFormat parseDate)
        {
            if (!dateTime.HasValue)
                return null;

            switch (parseDate)
            {
                case ParseDateFormat.Short:
                    return dateTime?.ToString("yyyyMMdd");
                case ParseDateFormat.Normal:
                    return dateTime?.ToString("yyyyMMddHHmm");
                case ParseDateFormat.Full:
                    return dateTime?.ToString("yyyyMMddHHmmss");
            }

            return null;
        }

        private void CheckErrors(IResposnePacket responsePacket)
        {
            if (responsePacket.Errors != null)
                throw new ExpressPayException(responsePacket.Errors[0]);

            if (responsePacket.Error != null)
                throw new ExpressPayException(responsePacket.Error.Msg);

            if (responsePacket.ErrorMessage != null)
                throw new ExpressPayException(responsePacket.ErrorMessage);

            if (responsePacket.Message != null)
                throw new ExpressPayException(responsePacket.Message);
        }

        #endregion
    }
}
