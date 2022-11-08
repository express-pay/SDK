namespace ExpressPay.SDK.Data.Extensions
{
    internal static class BoolExtension
    {
        internal static string Parse(this bool Val)
        {
            return Val ? "1" : "0";
        }
    }
}
