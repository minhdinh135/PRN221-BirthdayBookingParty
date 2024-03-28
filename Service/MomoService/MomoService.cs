using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.MomoService
{
    public class MomoService
    {
        public MomoOneTimePaymentRequest momoRequest { get; set; }
        private readonly MomoConfig _config;

        public MomoService(IOptions<MomoConfig> config)
        {
            _config = config.Value;
        }

        public MomoOneTimePaymentRequest CreateRequestModel(long amount, string description, ExtraDataDTO extraData)
        {
            momoRequest = new MomoOneTimePaymentRequest
            {
                requestId = CreateRequestId(),
                orderId = CreateOrderId(),
                partnerCode = _config.PartnerCode,
                amount = amount,
                orderInfo = description,
                extraData = Base64Encode(JsonConvert.SerializeObject(extraData)),
                redirectUrl = _config.ReturnUrl,
                ipnUrl = _config.IpnUrl,
                requestType = "captureWallet",
                lang = "vi",
            };
            momoRequest.signature = MakeSignature(_config.AccessKey, _config.SecretKey);
            return momoRequest;
        }

        public string MakeSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                "&amount=" + momoRequest.amount +
                "&extraData=" + momoRequest.extraData +
                "&ipnUrl=" + momoRequest.ipnUrl +
                "&orderId=" + momoRequest.orderId +
                "&orderInfo=" + momoRequest.orderInfo +
                "&partnerCode=" + momoRequest.partnerCode +
                "&redirectUrl=" + momoRequest.redirectUrl +
                "&requestId=" + momoRequest.requestId +
                "&requestType=" + momoRequest.requestType;
            return HmacSHA256(secretKey, rawHash);
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public string HmacSHA256(string key, string inputData)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;
            }
        }

        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private string CreateRequestId()
        {
            DateTime currentTime = DateTime.UtcNow;
            long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

            return timestamp + "id";
        }

        private string CreateOrderId()
        {
            DateTime currentTime = DateTime.UtcNow;
            long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

            return timestamp + ":0123456778";
        }

    }

    public class ExtraDataDTO
    {
        public int BookingId { get; set; }
        public string PayType { get; set; }
    }
}
