using System.Threading.Tasks;
using Xunit;

namespace ExpressPaySDKTests.Invoices
{
    public class AddInvoiceTest
    {
        [Fact(DisplayName = "Should_Add_Invoice")]
        public async Task Should_Add_Invoice()
        {
            var response = await Utils.ExpressPaySdk.AddInvoiceAsync(
                accountNo: "100",
                amount: 25,
                currency: 933);

            Assert.NotEqual(response.InvoiceNo, 0);
        }

        [Fact(DisplayName = "Should_Add_Invoice_With_Public_Url")]
        public async Task Should_Add_Invoice_With_Public_Url()
        {
            var response = await Utils.ExpressPaySdk.AddInvoiceAsync(
                accountNo: "100",
                amount: 25,
                currency: 933,
                returnInvoiceUrl: true);

            Assert.NotEqual(response.InvoiceNo, 0);
            Assert.NotNull(response.InvoiceUrl);
        }

        [Fact(DisplayName = "Should_Add_Card_Invoice")]
        public async Task Should_Add_Card_Invoice()
        {
            var response = await Utils.ExpressPaySdk.AddCardInvoiceAsync(
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/");

            Assert.NotEqual(response.CardInvoiceNo, 0);
        }

        [Fact(DisplayName = "Should_Add_Card_Invoice_With_Public_Url")]
        public async Task Should_Add_Card_Invoice_With_Public_Url()
        {
            var response = await Utils.ExpressPaySdk.AddCardInvoiceAsync(
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/",
                returnInvoiceUrl: true);

            Assert.NotEqual(response.CardInvoiceNo, 0);
            Assert.NotNull(response.InvoiceUrl);
        }

        [Fact(DisplayName = "Should_Add_Invoice_By_Version_2")]
        public async Task Should_Add_Invoice_By_Version_2()
        {
            var response = await Utils.ExpressPaySdk.AddInvoiceV2Async(
                serviceId: Utils.ServiceId,
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/");

            Assert.NotEqual(response.ExpressPayInvoiceNo, 0);
            Assert.NotNull(response.InvoiceUrl);
        }

        [Fact(DisplayName = "Should_Add_Web_Invoice")]
        public async Task Should_Add_Web_Invoice()
        {
            var response = await Utils.ExpressPaySdk.AddWebInvoiceAsync(
                serviceId: Utils.ServiceId,
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/");

            Assert.NotEqual(response.ExpressPayInvoiceNo, 0);
        }

        [Fact(DisplayName = "Should_Add_Web_Invoice_With_Public_Url")]
        public async Task Should_Add_Web_Invoice_With_Public_Url()
        {
            var response = await Utils.ExpressPaySdk.AddWebInvoiceAsync(
                serviceId: Utils.ServiceId,
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/",
                returnInvoiceUrl: true);

            Assert.NotEqual(response.ExpressPayInvoiceNo, 0);
            Assert.NotNull(response.InvoiceUrl);
        }

        [Fact(DisplayName = "Should_Add_Web_Card_Invoice")]
        public async Task Should_Add_Web_Card_Invoice()
        {
            var response = await Utils.ExpressPaySdk.AddWebCardInvoiceAsync(
                serviceId: Utils.ServiceId,
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/");

            Assert.NotNull(response.FormUrl);
        }

        [Fact(DisplayName = "Should_Add_Web_Card_Invoice_With_Public_Url")]
        public async Task Should_Add_Web_Card_Invoice_With_Public_Url()
        {
            var response = await Utils.ExpressPaySdk.AddWebCardInvoiceAsync(
                serviceId: Utils.ServiceId,
                accountNo: "100",
                amount: 25,
                currency: 933,
                info: "Test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/",
                returnInvoiceUrl: true);

            Assert.NotNull(response.FormUrl);
            Assert.NotNull(response.InvoiceUrl);
        }
    }
}
