namespace ExpressPay.SDK.Data.Base
{
    internal interface IResposnePacket
    {
        /// <summary>
        /// Ошибка валидации модели
        /// </summary>
        string[] Errors { get; set; }

        /// <summary>
        /// Номер ошибки. Отсутствует, если ошибки нет
        /// </summary>
        int? ErrorCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке. Отсутствует, если ошибки нет
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Ошибка при проведении запроса 
        /// </summary>
        Error Error { get; set; }

        /// <summary>
        /// Сообщение об ошибке авторизации
        /// </summary>
        string Message { get; set; }
    }
}
