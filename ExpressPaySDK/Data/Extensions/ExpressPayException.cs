using System;

namespace ExpressPay.SDK.Data.Extensions
{
    /// <summary>
    /// При возникновении ошибки прокидываем исключение
    /// </summary>
    public class ExpressPayException : Exception
    {
        /// <summary>
        /// При возникновении ошибки прокидываем исключение
        /// </summary>
        /// <param name="message">Сообщение от API</param>
        public ExpressPayException(string message)
            : base(message)
        {
        }
    }
}
