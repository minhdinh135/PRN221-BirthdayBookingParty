using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MomoService
{
    public class MomoOneTimePaymentCreateLinkResponse
    {
        public string partnerCode { get; set; } = string.Empty;

        public string requestId { get; set; } = string.Empty;

        public string orderId { get; set; } = string.Empty;

        public long amount { get; set; }

        public long responseTime { get; set; }

        public string message { get; set; } = string.Empty;

        public long resultCode { get; set; }

        public string payUrl { get; set; } = string.Empty;

        public string deepLink { get; set; } = string.Empty;

        public string qrCodeUrl { get; set; } = string.Empty;
    }
}
