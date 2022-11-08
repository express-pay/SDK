namespace ExpressPay.SDK.Enums
{
    /// <summary>
    /// Период через который будет произведено следующие списание
    /// </summary>
    public enum WriteOffPeriod
    {
        /// <summary>
        /// День
        /// </summary>
        Day = 1,
        /// <summary>
        /// Неделя
        /// </summary>
        Week = 2,
        /// <summary>
        /// Месяц
        /// </summary>
        Month = 3,
        /// <summary>
        /// 3 месяца
        /// </summary>
        ThreeMonths = 4,
        /// <summary>
        /// 6 месяцев
        /// </summary>
        HalfYear = 5,
        /// <summary>
        /// Год
        /// </summary>
        Year = 6
    }
}
