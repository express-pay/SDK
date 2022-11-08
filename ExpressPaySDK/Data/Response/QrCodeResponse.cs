using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Выходные параметры при генерации QR-кода для счета
    /// </summary>
    public class QrCodeResponse : IResponseMessage
    {
        /// <summary>
        /// QR-код счета
        /// </summary>
        public string QrCodeBody { get; set; }
    }
}
