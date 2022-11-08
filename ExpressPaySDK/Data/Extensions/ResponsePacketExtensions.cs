using ExpressPay.SDK.Data.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpressPay.SDK.Data.Extensions
{
    internal static class ResponsePacketExtensions
    {
        internal static string Serialize<T>(this ResponsePacket<T> packet)
             where T : IResponseMessage, new()
        {
            return JsonConvert.SerializeObject(packet);
        }

        internal static ResponsePacket<T> Deserialize<T>(string data)
            where T : IResponseMessage, new()
        {
            if (!data.Contains("Items") && 
                !data.Contains("Error") &&
                !data.Contains("Message"))
            {
                JObject keyValuePairs = JObject.Parse(data);
                data = JsonConvert.SerializeObject(new { Value = keyValuePairs });
            }

            return JsonConvert.DeserializeObject<ResponsePacket<T>>(data);
        }
    }
}
