namespace ExpressPay.SDK.Enums
{
    /// <summary>
    /// Используемый формат даты для запроса
    /// </summary>
    public enum ParseDateFormat
    {
        /// <summary>
        /// Короткий формат
        /// yyyyMMdd
        /// </summary>
        Short = 0,
        /// <summary>
        /// Номарльный формат
        /// yyyyMMddHHmm 
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Полный формат 
        /// yyyyMMddHHmmss 
        /// </summary>
        Full = 2
    }
}
