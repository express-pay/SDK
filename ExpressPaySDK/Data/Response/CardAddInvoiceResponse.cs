using ExpressPay.SDK.Data.Base;

namespace ExpressPay.SDK.Response
{
    /// <summary>
    /// Выходные параметры при выставлении счета для оплаты по карте 
    /// </summary>
    public class CardAddInvoiceResponse : IResponseMessage
    {
        /// <summary>
        /// Номер счета по карте
        /// </summary>
        public int CardInvoiceNo { get; set; }

        /// <summary>
        /// Публичная ссылка на счет. 
        /// </summary>
        public string InvoiceUrl { get; set; }


    }
}
