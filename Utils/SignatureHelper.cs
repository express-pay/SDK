
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ExpressPay.SDK.Utils
{
    /// <summary>
    /// Статический класс получение цифровой подписи
    /// </summary>
	public static class SignatureHelper
    {
        private static readonly Encoding HashEncoding = Encoding.UTF8;
        // Порядок следования полей при вычислении цифровой подписи.
        // Внимание! Для корректной работы порядок изменять нельзя
        private static readonly Dictionary<string, dynamic> Mapping = new Dictionary<string, dynamic>
        {
            {
                "add-invoice", new[]
                {
                    "token",
                    "accountno",
                    "amount",
                    "currency",
                    "expiration",
                    "info",
                    "surname",
                    "firstname",
                    "patronymic",
                    "city",
                    "street",
                    "house",
                    "building",
                    "apartment",
                    "isnameeditable",
                    "isaddresseditable",
                    "isamounteditable",
                    "emailnotification",
                    "returninvoiceurl"
                }
            },
            {
                "get-details-invoice", new[]
                {
                    "token",
                    "invoiceno",
                    "returninvoiceurl"
                }
            },
            {
                "cancel-invoice", new[]
                {
                    "token",
                    "invoiceno"
                }
            },
            {
                "status-invoice", new[]
                {
                    "token",
                    "invoiceno"
                }
            },
            {
                "get-list-invoices", new[]
                {
                    "token",
                    "from",
                    "to",
                    "accountno",
                    "status"
                }
            },
            {
                "get-list-payments", new[]
                {
                    "token",
                    "from",
                    "to",
                    "accountno"
                }
            },
            {
                "get-details-payment", new[]
                {
                    "token",
                    "paymentno"
                }
            },
            {
                "balance", new[]
                {
                    "token",
                    "accountno"
                }
            },
            {
                "add-card-invoice", new[]
                {
                    "token",
                    "accountno",
                    "expiration",
                    "amount",
                    "currency",
                    "info",
                    "returnurl",
                    "failurl",
                    "language",
                    "pageview",
                    "sessiontimeoutsecs",
                    "expirationdate",
                    "returninvoiceurl"
                }
            },
            {
                "card-invoice-form", new[]
                {
                    "token",
                    "cardinvoiceno"
                }
            },
            {
                "status-card-invoice", new[]
                {
                    "token",
                    "cardinvoiceno",
                    "language"
                }
            },
            {
                "reverse-card-invoice", new[]
                {
                    "token",
                    "cardinvoiceno"
                }
            },
            {
                "get-qr-code", new[]
                {
                    "token",
                    "invoiceid",
                    "viewtype",
                    "imagewidth",
                    "imageheight"
                }
            },
            {
                "add-web-invoice",new[]
                {
                    "token",
                    "serviceid",
                    "accountno",
                    "amount",
                    "currency",
                    "expiration",
                    "info",
                    "surname",
                    "firstname",
                    "patronymic",
                    "city",
                    "street",
                    "house",
                    "building",
                    "apartment",
                    "isnameeditable",
                    "isaddresseditable",
                    "isamounteditable",
                    "emailnotification",
                    "smsphone",
                    "returntype",
                    "returnurl",
                    "failurl",
                    "returninvoiceurl"
                }
            },
            {
                "add-webcard-invoice",new[]
                {
                    "token",
                    "serviceid",
                    "accountno",
                    "expiration",
                    "amount",
                    "currency",
                    "info",
                    "returnurl",
                    "failurl",
                    "language",
                    "sessiontimeoutsecs",
                    "expirationdate",
                    "returntype",
                    "returninvoiceurl"
                }
            },
            {
               "invoicesv2",new[]
                {
                     "token",
                     "serviceid",
                     "accountno",
                     "amount",
                     "currency",
                     "expiration",
                     "info",
                     "surname",
                     "firstname",
                     "patronymic",
                     "city",
                     "street",
                     "house",
                     "building",
                     "apartment",
                     "isnameeditable",
                     "isaddresseditable",
                     "isamounteditable",
                     "emailnotification",
                     "smsphone",
                     "returntype",
                     "returnurl",
                     "failurl",
                }
            }

        };

        /// <summary>
        /// Вычисление цифровой подписи
        /// </summary>
        /// <param name="requestParams">Входные параметры</param>
        /// <param name="secretWord">Секретное слово</param>
        /// <param name="action">Используемый метод</param>
        /// <returns></returns>
        public static string Compute(Dictionary<string, string> requestParams, string secretWord, string action)
        {
            var normalizedParams = requestParams
                .ToDictionary(k => k.Key.ToLower(), v => v.Value);

            var cmdFields = Mapping[action];

            var builder = new StringBuilder();
            foreach (var cmdField in cmdFields)
            {
                if (normalizedParams.ContainsKey(cmdField))
                    builder.Append(normalizedParams[cmdField] as string);
            }

            HMACSHA1 hmac;

            if (string.IsNullOrWhiteSpace(secretWord))
            {
                // В алгоритме всегда должно быть задан ключ шифрования. Если используется конструктор по умолчанию, то ключ генерируется автоматически,
                // что нам не подходит
                hmac = new HMACSHA1(HashEncoding.GetBytes(string.Empty));
            }
            else
            {
                hmac = new HMACSHA1(HashEncoding.GetBytes(secretWord));
            }

            var hash = hmac.ComputeHash(
                HashEncoding.GetBytes(builder.ToString()));

            var result = new StringBuilder();

            foreach (var item in hash)
            {
                result.Append(item.ToString("X2"));
            }

            return result.ToString();
        }
    }
}
