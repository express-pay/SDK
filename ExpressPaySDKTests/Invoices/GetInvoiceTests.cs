using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ExpressPaySDKTests.Invoices
{
    public class GetInvoiceTests
    {

        [Fact(DisplayName = "Should_Get_Invoice_List")]
        public async Task Should_Get_Invoice_List()
        {
            var invoices = await Utils.ExpressPaySdk.GetInvoicesAsync();

            Assert.NotEmpty(invoices);
        }

        [Fact(DisplayName = "Should_Get_Invoice_Deatails")]
        public async Task Should_Get_Invoice_Deatails()
        {
            var response = await Utils.ExpressPaySdk.GetInvoiceDetailsAsync(
                invoiceNo: Utils.InvoiceNo);

            Assert.NotNull(response.AccountNo);
        }

        [Fact(DisplayName = "Should_Get_Invoice_Deatails_With_Pusblic_Url")]
        public async Task Get_Invoice_Details_With_Url_Test()
        {
            var response = await Utils.ExpressPaySdk.GetInvoiceDetailsAsync(
                invoiceNo: Utils.InvoiceNo,
                returnInvoiceUrl: true);

            Assert.NotNull(response.AccountNo);
            Assert.NotNull(response.InvoiceUrl);
        }

        [Fact(DisplayName = "Should_Get_Invoice_Status")]
        public async Task Should_Get_Invoice_Status()
        {
            var response = await Utils.ExpressPaySdk.GetInvoiceStatusAsync(
                invoiceNo: Utils.InvoiceNo);

            Assert.NotNull(response.Status);
        }

        [Fact(DisplayName = "Should_Get_Payment_From_For_Card_Inovice")]
        public async Task Should_Get_Payment_From_For_Card_Inovice()
        {
            var response = await Utils.ExpressPaySdk.GetPaymentFormAsync(
                cardInvoiceNo: Utils.InvoiceNo);

            Assert.NotNull(response.FormUrl);
        }

        [Fact(DisplayName = "Should_Get_Card_Invoice_Status")]
        public async Task Should_Get_Card_Invoice_Status()
        {
            var response = await Utils.ExpressPaySdk.GetCardInvoceStatusAsync(
                cardInvoiceNo: Utils.InvoiceNo);

            Assert.NotNull(response.CardInvoiceStatus);
        }

        [Fact(DisplayName = "Should_Reverse_Card_Payment")]
        public async Task Should_Reverse_Card_Payment()
        {
            var response = await Utils.ExpressPaySdk.ReverseCardPaymentAsync(
                cardInvoiceNo: 104);

            Assert.Equal(response, HttpStatusCode.OK);
        }
    }
}
