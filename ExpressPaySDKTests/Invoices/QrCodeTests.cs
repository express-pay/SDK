using System.Threading.Tasks;
using System;
using Xunit;

namespace ExpressPaySDKTests.Invoices
{

    public class QrCodeTests
    {

        [Fact(DisplayName = "Should_Get_Qr_Code_In_Base64_String")]
        public async Task Should_Get_Qr_Code_In_Base64_String()
        {
            var response = await Utils.ExpressPaySdk.GetQrCodeAsync(
                invoiceNo: Utils.InvoiceNo);

            var result = Convert.FromBase64String(response.QrCodeBody);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should_Get_Qr_Code_In_Text_String")]
        public async Task Should_Get_Qr_Code_In_Text_String()
        {
            var response = await Utils.ExpressPaySdk.GetQrCodeAsync(
                invoiceNo: Utils.InvoiceNo,
                viewType: ExpressPay.SDK.Enums.ViewType.Text);

            Assert.NotNull(response.QrCodeBody);
        }

        [Fact(DisplayName = "Should_Get_Qr_Code_By_Account_Number_In_Base64_String")]
        public async Task Should_Get_Qr_Code_By_Account_Number_In_Base64_String()
        {
            var response = await Utils.ExpressPaySdk.GetQrCodeByAccountNumberAsync(
                accountNumber: Utils.InvoiceNo.ToString());

            var result = Convert.FromBase64String(response.QrCodeBody);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should_Get_Qr_Code_By_Account_Number_In_Text_String")]
        public async Task Should_Get_Qr_Code_By_Account_Number_In_Text_String()
        {
            var response = await Utils.ExpressPaySdk.GetQrCodeByAccountNumberAsync(
                accountNumber: Utils.InvoiceNo.ToString(),
                viewType: ExpressPay.SDK.Enums.ViewType.Text);

            Assert.NotNull(response.QrCodeBody);
        }
    }
}
