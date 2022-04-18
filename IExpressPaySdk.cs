using System.Net;
using System.Threading.Tasks;

namespace ExpressPay.SDK
{
    interface IExpressPaySdk
    {
        Task<string> GetInvoices(string accountNo, string from, string to, int? status);
        Task<string> AddInvoice(
                string accountNo,
                decimal amount,
                int currency,
                string expiration,
                string info,
                string surname,
                string firstName,
                string patronymic,
                string city,
                string street,
                string house,
                string building,
                string apartment,
                int isNameEditable,
                int isAddressEditable,
                int isAmountEditable,
                string emailNotification,
                string smsPhone,
                int returnInvoiceUrl
        );
        Task<string> GetInvoiceDetails(int invoiceNo, int returnInvoiceUrl);

        Task<string> GetInvoiceStatus(int invoiceNo);

        Task<HttpStatusCode> CancelInvoice(int invoiceNo);

        Task<string> GetQrCode(int invoiceNo, string viewType, int? imageWidth, int? imageHeight);

        Task<string> AddCardInvoice
            (
                string accountNo,
                decimal amount,
                int currency,
                string info,
                string returnUrl,
                string failUrl,
                string expiration,
                string language,
                int? sessionTimeoutSec,
                string expirationDate,
                int returnInvoiceUrl
            );

        Task<string> GetPaymentForm(int cardInvoieNo);

        Task<string> GetCardInvoceStatus(int cardInvoiceNo, string language);

        Task<HttpStatusCode> ReverseCardPayment(int cardInvoiceNo);

        Task<string> AddWebInvoice
            (
                int serviceId,
                string accountNo,
                decimal amount,
                int currency,
                string returnUrl,
                string failUrl,
                string expiration,
                string info,
                string surname,
                string firstName,
                string patronymic,
                string city,
                string street,
                string house,
                string building,
                string apartment,
                int isNameEditable,
                int isAddressEditable,
                int isAmountEditable,
                string emailNotification,
                string smsPhone,
                int returnInvoiceUrl
            );

        Task<string> AddWebCardInvoice
            (
                int serviceId,
                string accountNo,
                decimal amount,
                int currency,
                string returnUrl,
                string failUrl,
                string expiration,
                string info,
                int returnInvoiceUrl
            );

        Task<string> GetPayments(string from, string to, string accountNo);

        Task<string> GetPaymentDetail(int paymentNo);

        Task<string> GetBalance(string accountNo);

        Task<string> AddInvoiceV2
        (
            int serviceId,
            string accountNo,
            decimal amount,
            int currency,
            string returnUrl,
            string failUrl,
            string expiration,
            string info,
            string surname,
            string firstName,
            string patronymic,
            string city,
            string street,
            string house,
            string building,
            string apartment,
            int isNameEditable,
            int isAddressEditable,
            int isAmountEditable,
            string emailNotification,
            string smsPhone
        );
    }
}
