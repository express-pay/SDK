
using System.Threading.Tasks;
using Xunit;

namespace ExpressPaySDKTests.Payments
{
    public class GetPaymentTests
    {
        [Fact(DisplayName = "Should_Get_Payment_List")]
        public async Task Should_Get_Payment_List()
        {
            var payments = await Utils.ExpressPaySdk.GetPaymentsAsync();

            Assert.NotEmpty(payments);
        }

        [Fact(DisplayName = "Should_Get_Payment_Deatails")]
        public async Task Should_Get_Payment_Deatails()
        {
            var response = await Utils.ExpressPaySdk.GetPaymentDetailAsync(
                paymentNo: 1);

            Assert.NotNull(response.AccountNo);
        }

        [Fact(DisplayName = "Should_Get_Balance")]
        public async Task Should_Get_Balance()
        {
            var response = await Utils.ExpressPaySdk.GetBalanceAsync(
                accountNo: Utils.InvoiceNo.ToString());

            Assert.NotNull(response.Balance);
        }
    }
}
