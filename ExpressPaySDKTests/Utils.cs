using ExpressPay.SDK;

namespace ExpressPaySDKTests
{

    public static class Utils
    {
        public static readonly IExpressPaySdk ExpressPaySdk;
        public static readonly int InvoiceNo = 100;
        public static readonly int ServiceId = 4;

        static Utils()
        {
            ExpressPaySdk = new ExpressPaySdk(
                isTest: true,
                token: "a75b74cbcfe446509e8ee874f421bd66",
                useSignature: true,
                secretWord: "sandbox.expresspay.by");
        }       
    }
}
