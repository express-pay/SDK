using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Data.Response
{
    /// <summary>
    /// Ответ на получение баланса
    /// </summary>
    public class BalanceResponse : IResponseMessage
    {
        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; }
    }
}
