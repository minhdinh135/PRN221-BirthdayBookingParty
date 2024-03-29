using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Services.MomoService
{
    public class MomoOneTimePaymentRequest
    {
        public string requestId { get; set; } = string.Empty;

        public string partnerCode { get; set; } = string.Empty;

        public long amount { get; set; }

        public string orderId { get; set; } = string.Empty;

        public string orderInfo { get; set; } = string.Empty;

        public string redirectUrl { get; set; } = string.Empty;

        public string ipnUrl { get; set; } = string.Empty;

        public string requestType { get; set; } = string.Empty;

        public string extraData { get; set; } = string.Empty;

        public string lang { get; set; } = string.Empty;

        public string signature { get; set; } = string.Empty;

        public (bool, string?) GetLink(string paymentUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var createPaymentLinkRes = client.PostAsync(paymentUrl, requestContent).Result;

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = createPaymentLinkRes.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<MomoOneTimePaymentCreateLinkResponse>(responseContent);
                if (responseData.resultCode == 0)
                {
                    return (true, responseData.payUrl);
                }
                else
                {
                    return (false, responseData.message);
                }
            }
            else
            {
                return (false, createPaymentLinkRes.ReasonPhrase);
            }
        }
    }
}
