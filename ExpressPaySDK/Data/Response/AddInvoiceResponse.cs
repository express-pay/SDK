using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Выходные параметры при выставлении счета
    /// </summary>
    public class AddInvoiceResponse : IResponseMessage
    {
        /// <summary>
        /// Номер счета в сервисе Экспресс Платежи
        /// </summary>
        public int InvoiceNo { get; set; }

        /// <summary>
        /// Публичная ссылка на счет
        /// </summary>
        public string InvoiceUrl { get; set; }
    }
}
