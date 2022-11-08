using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Выходные параметры при получении статуса счета
    /// </summary>
    public class StatusResponse : IResponseMessage
    {
        /// <summary>
        /// Статус счета на оплату
        /// </summary>
        public int Status { get; set; }
    }
}
