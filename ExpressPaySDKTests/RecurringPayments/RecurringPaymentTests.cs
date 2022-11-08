using System.Threading.Tasks;
using Xunit;

namespace ExpressPaySDKTests.RecurringPayments
{
    public class RecurringPaymentTests
    {
        [Fact(DisplayName = "Should_Get_Url_For_Initiating_Payment")]
        public async Task Should_Get_Url_For_Initiating_Payment()
        {
            var bindResponse = await Utils.ExpressPaySdk.RecurringPaymentBindAsync(
                serviceId: Utils.ServiceId,
                writeOffPeriod: ExpressPay.SDK.Enums.WriteOffPeriod.Day,
                amount: 25,
                currency: 933,
                info: "test",
                returnUrl: "https://express-pay.by/",
                failUrl: "https://express-pay.by/");

            Assert.NotNull(bindResponse.CustomerId);
            Assert.NotEmpty(bindResponse.FormUrl);
        }
    }
}
