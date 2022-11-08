using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Data.Response
{
    /// <summary>
    /// Выходные параметры при выставлении счета
    /// </summary>
    public class WebAddInvoiceResponse : IResponseMessage
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public string ExpressPayAccountNumber { get; set; }

        /// <summary>
        /// Номер счета в сервисе Экспресс Платежи
        /// </summary>
        public int ExpressPayInvoiceNo { get; set; }

        /// <summary>
        /// Цифровая подпись
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Публичная ссылка на счет
        /// </summary>
        public string InvoiceUrl { get; set; }
    }
}
