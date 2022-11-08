using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Data.Response
{
    /// <summary>
    /// Выходные параметры при привязке карты 
    /// </summary>
    public class BindResponse : IResponseMessage
    {
        /// <summary>
        /// Адрес страницы ввода данных банковской карты
        /// </summary>
        public string FormUrl { get; set; }

        /// <summary>
        /// Номер привзяки (используется при отвязке карты)
        /// </summary>
        public int CustomerId { get; set; }
    }
}
