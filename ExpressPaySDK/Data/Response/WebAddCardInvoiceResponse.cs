using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Data.Response
{
    /// <summary>
    /// Выходные параметры при выставлении счета для оплаты картой
    /// </summary>
    public class WebAddCardInvoiceResponse : IResponseMessage
    {
        /// <summary>
        /// Адрес страницы ввода данных банковской карты
        /// </summary>
        public string FormUrl { get; set; }

        /// <summary>
        /// Публичная ссылка на счет
        /// </summary>
        public string InvoiceUrl { get; set; }
    }
}
