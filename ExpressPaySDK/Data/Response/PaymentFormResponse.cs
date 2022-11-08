using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Выходные параметры при получении формы оплаты
    /// </summary>
    public class PaymentFormResponse : IResponseMessage
    {
        /// <summary>
        /// Адрес страницы ввода данных банковской карты
        /// </summary>
        public string FormUrl { get; set; }
    }
}
