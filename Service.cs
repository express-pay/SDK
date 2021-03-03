using ExpressPay.SDK.Utils;
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
		private static readonly HttpClient client = new HttpClient();

		private string host = "https://api.express-pay.by/v1/";
		private readonly string token;
		private readonly bool useSignature;
		private readonly string secretWord;

		/// <summary>
		/// В конструктор передаем необходимы настройки
		/// </summary>
		/// <param name="isTest">Использовать тестовый сервер</param>
		/// <param name="token">API-ключ</param>
		/// <param name="useSignature">Использовать цифровую подпись</param>
		/// <param name="secretWord">Секретное слово</param>
		public ExpressPaySdk(bool isTest, string token, bool useSignature = false, string secretWord = "")
		{
			if (isTest)
			{
				host = "https://sandbox-api.express-pay.by/v1/";
			}
			this.token = token;
			this.useSignature = useSignature;
			this.secretWord = secretWord;
		}

		/// <summary>
		/// Список счетов по парметрам
		/// Если параметры не заданы возращаются платежи за полседние 30 дней
		/// </summary>
		/// <param name="accountNo">Номер счета</param>
		/// <param name="from">Дата выставления с - начало периода Формат - yyyyMMdd</param>
		/// <param name="to">Дата выставления по - конец периода Формат - yyyyMMdd</param>
		/// <param name="status">Статус счета на оплату Формат - yyyyMMdd</param>
		/// <returns>json</returns>
		public async Task<string> GetInvoices(string accountNo = "", string from = "", string to = "", int? status = null)
		{
			host = string.Format("{0}invoices?", host);
			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"AccountNo", accountNo},
				{"From", from },
				{"To", to },
				{"Status", status.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "get-list-invoices"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
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
		/// <param name="isNameEditable">При оплате разрешено изменять ФИО плательщика
		/// 0 – нет, 1 – да</param>
		/// <param name="isAddressEditable">При оплате разрешено изменять адрес плательщика
		/// 0 – нет, 1 – да</param>
		/// <param name="isAmountEditable">	При оплате разрешено изменять сумму оплаты
		/// 0 – нет, 1 – да</param>
		/// <param name="emailNotification"></param>
		/// <param name="smsPhone"></param>
		/// <param name="returnInvoiceUrl"></param>
		/// <returns>json</returns>
		public async Task<string> AddInvoice
			(
				string accountNo,
				decimal amount,
				int currency,
				string expiration = "",
				string info = "",
				string surname = "",
				string firstName = "",
				string patronymic = "",
				string city = "",
				string street = "",
				string house = "",
				string building = "",
				string apartment = "",
				int isNameEditable = 0,
				int isAddressEditable = 0,
				int isAmountEditable = 0,
				string emailNotification = "",
				string smsPhone = "",
				int returnInvoiceUrl = 0
			)
		{
			host = string.Format("{0}invoices?token={1}", host, token);
			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
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
				{"IsNameEditable", isNameEditable.ToString()},
				{"IsAddressEditable", isAddressEditable.ToString()},
				{"IsAmountEditable", isAmountEditable.ToString()},
				{"EmailNotification", emailNotification},
				{"SmsPhone", smsPhone},
				{"ReturnInvoiceUrl", returnInvoiceUrl.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "add-invoice"));
			}

			var content = new FormUrlEncodedContent(requestParams);

			var response = await client.PostAsync(host, content);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Детальная информации по счету
		/// </summary>
		/// <param name="invoiceNo">Номер счета полученный при выставлении</param>
		/// <param name="returnInvoiceUrl">Вернуть в ответе публичную ссылку на счет 
		/// 0 – нет, 1 – да (0 - по умолчанию)</param>
		/// <returns>json</returns>
		public async Task<string> GetInvoiceDetails(int invoiceNo, int returnInvoiceUrl = 0)
		{
			host = string.Format("{0}invoices/{1}?", host, invoiceNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"InvoiceNo", invoiceNo.ToString()},
				{"ReturnInvoiceUrl", returnInvoiceUrl.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "get-details-invoice"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Статуса счета
		/// </summary>
		/// <param name="invoiceNo">Номер счета полученный при выставлении</param>
		/// <returns>json</returns>
		public async Task<string> GetInvoiceStatus(int invoiceNo)
		{
			host = string.Format("{0}invoices/{1}/status?", host, invoiceNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"InvoiceNo", invoiceNo.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "status-invoice"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Отмена счета
		/// </summary>
		/// <param name="invoiceNo">Номер счета полученный при выставлении</param>
		/// <returns>HTTP код</returns>
		public async Task<HttpStatusCode> CancelInvoice(int invoiceNo)
		{
			host = string.Format("{0}invoices/{1}?", host, invoiceNo);
			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"InvoiceNo", invoiceNo.ToString()},
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "cancel-invoice"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.DeleteAsync(host);

			var responseString = response.StatusCode;

			return responseString;
		}

		/// <summary>
		/// Получение Qr-кода
		/// </summary>
		/// <param name="invoiceNo">Номер счета</param>
		/// <param name="viewType">Тип возвращаемого значения. Принимает два параметра:
		/// base64 - возвращает изображение в формате base64;
		/// text - возвращает ссылку;</param>
		/// <param name="imageWidth">Ширина qr-кода</param>
		/// <param name="imageHeight">Высота qr-кода</param>
		/// <returns>json</returns>
		public async Task<string> GetQrCode(int invoiceNo, string viewType = "base64", int? imageWidth = null, int? imageHeight = null)
		{
			host = string.Format("{0}qrcode/getqrcode/?", host);
			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"InvoiceId", invoiceNo.ToString()},
				{"ViewType", viewType},
				{"ImageWidth", imageWidth.ToString()},
				{"ImageHeight", imageWidth.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "get-qr-code"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
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
		/// <param name="returnInvoiceUrl">Вернуть в ответе публичную ссылку на счет
		/// 0 – нет, 1 – да(0 - по умолчанию)</param>
		/// <returns>json</returns>
		public async Task<string> AddCardInvoice
			(
				string accountNo,
				decimal amount,
				int currency,
				string info,
				string returnUrl,
				string failUrl,
				string expiration = "",
				string language = "ru",
				int? sessionTimeoutSec = 1200,
				string expirationDate = "",
				int returnInvoiceUrl = 0
			)
		{
			host = string.Format("{0}cardinvoices?token={1}", host, token);
			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
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
				{"ReturnInvoiceUrl", returnInvoiceUrl.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "add-card-invoice"));
			}


			var content = new FormUrlEncodedContent(requestParams);

			var response = await client.PostAsync(host, content);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Ссылку на форму оплаты для оплаты счета по карте
		/// </summary>
		/// <param name="cardInvoieNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
		/// <returns>json</returns>
		public async Task<string> GetPaymentForm(int cardInvoieNo)
		{
			host = string.Format("{0}cardinvoices/{1}/payment?", host, cardInvoieNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"CardInvoiceNo", cardInvoieNo.ToString()},
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "card-invoice-form"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Статус счета по карте
		/// </summary>
		/// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
		/// <param name="language">Язык в кодировке ISO 639-1. Если не указан, считается, что язык – русский. 
		/// Сообщение об ошибке будет возвращено именно на этом языке.</param>
		/// <returns></returns>
		public async Task<string> GetCardInvoceStatus(int cardInvoiceNo, string language = "ru")
		{
			host = string.Format("{0}cardinvoices/{1}/status?", host, cardInvoiceNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"CardInvoiceNo", cardInvoiceNo.ToString()},
				{"Language", language}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "status-card-invoice"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Отмена платежа по карте 
		/// </summary>
		/// <param name="cardInvoiceNo">Номер счета по карте, полученный с помощью метода “Выставление счета по карте”</param>
		/// <returns>json</returns>
		public async Task<HttpStatusCode> ReverseCardPayment(int cardInvoiceNo)
		{
			host = string.Format("{0}cardinvoices/{1}/reverse?", host, cardInvoiceNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"CardInvoiceNo", cardInvoiceNo.ToString()},
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "reverse-card-invoice"));
			}

			var content = new FormUrlEncodedContent(requestParams);

			var response = await client.PostAsync(host, content);

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
		/// <returns>json</returns>
		public async Task<string> AddWebInvoice
			(
				int serviceId,
				string accountNo,
				decimal amount,
				int currency,
				string expiration = "",
				string info = "",
				string surname = "",
				string firstName = "",
				string patronymic = "",
				string city = "",
				string street = "",
				string house = "",
				string building = "",
				string apartment = "",
				int isNameEditable = 0,
				int isAddressEditable = 0,
				int isAmountEditable = 0,
				string emailNotification = "",
				string smsPhone = ""
			)
		{
			host = string.Format("{0}web_invoices", host);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
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
				{"IsNameEditable", isNameEditable.ToString()},
				{"IsAddressEditable",isAddressEditable.ToString()},
				{"IsAmountEditable", isAmountEditable.ToString()},
				{"EmailNotification", emailNotification},
				{"SmsPhone", smsPhone.Trim('+')},
				{"ReturnType", "json"},
				{"ReturnUrl", "-"},
				{"FailUrl", "-" },
				{"ReturnInvoiceUrl", "1"}
			};

			requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "add-web-invoice"));

			// После формирования цифровой подписи удаляем API-ключ
			// т.к он не учавствует в выставлении счета 
			requestParams.Remove("Token");

			var content = new FormUrlEncodedContent(requestParams);

			var response = await client.PostAsync(host, content);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Выставить счет для оплаты по карте. В данном методе цифровая подпись является обязательным параметром.
		/// </summary>
		/// <param name="serviceId">Номер услуги</param>
		/// <param name="accountNo">Номер лицевого счета</param>
		/// <param name="amount">Сумма счета на оплату. Разделителем дробной и целой части является символ запятой</param>
		/// <param name="currency">Код валюты</param>
		/// <param name="expiration">Дата истечения срока действия выставлена счета на оплату. Формат - yyyyMMdd</param>
		/// <param name="info">Назначение платежа</param>
		/// <returns>json</returns>
		public async Task<string> AddWebCardInvoice
			(
				int serviceId, 
				string accountNo, 
				decimal amount, 
				int currency,
				string info,
				string expiration = ""
			)
		{
			host = string.Format("{0}web_cardinvoices", host);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"ServiceId", serviceId.ToString()},
				{"AccountNo", accountNo},
				{"Expiration", expiration},
				{"Amount", amount.ToString()},
				{"Currency", currency.ToString()},
				{"Info", info},
				{"ReturnUrl", "-" },
				{"FailUrl", "-" },
				{"ReturnType", "json" },
				{"ReturnInvoiceUrl", "1"}
			};

			requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "add-webcard-invoice"));

			// После формирования цифровой подписи удаляем API-ключ
			// т.к он не учавствует в выставлении счета 
			requestParams.Remove("Token");

			var content = new FormUrlEncodedContent(requestParams);

			var response = await client.PostAsync(host, content);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;

		}

		/// <summary>
		/// Список платежей
		/// Если параметры не заданы возращаются платежи за полседние 30 дней
		/// </summary>
		/// <param name="from">Дата оплаты с – начало периода. Формат - yyyyMMdd</param>
		/// <param name="to">Дата оплаты по – конец периода. Формат - yyyyMMdd</param>
		/// <param name="accountNo">Номер лицевого счета</param>
		/// <returns>json</returns>
		public async Task<string> GetPayments(string from = "", string to = "", string accountNo = "")
		{

			host = string.Format("{0}payments?", host);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"From", from},
				{"To", to},
				{"AccountNo", accountNo}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "get-list-payments"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}

		/// <summary>
		/// Детали платежа
		/// </summary>
		/// <param name="paymentNo">Номер платежа</param>
		/// <returns>json</returns>
		public async Task<string> GetPaymentDetail(int paymentNo)
		{
			host = string.Format("{0}payments/{1}?", host, paymentNo);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"PaymentNo", paymentNo.ToString()}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "get-details-payment"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}
		
		/// <summary>
		/// Баланс по лицевому счету
		/// </summary>
		/// <param name="accountNo">Номер лицевого счета</param>
		/// <returns>json</returns>
		public async Task<string> GetBalance(string accountNo)
		{

			host = string.Format("{0}balance?", host);

			var requestParams = new Dictionary<string, string>
			{
				{"Token", token},
				{"AccountNo", accountNo}
			};

			if (useSignature)
			{
				requestParams.Add("signature", SignatureHelper.Compute(requestParams, secretWord, "balance"));
			}

			foreach (var rp in requestParams)
			{
				host += string.Format("&{0}={1}", rp.Key, rp.Value);
			}

			var response = await client.GetAsync(host);

			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}
	}
}
