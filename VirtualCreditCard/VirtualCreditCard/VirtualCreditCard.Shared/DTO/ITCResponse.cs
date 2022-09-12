using Newtonsoft.Json;

namespace VirtualCreditCard.Shared.DTO
{
    public class ITCResponse
    {
        [JsonProperty("gatewayGuid")]
        public Guid SaleGuid { get; set; }

        [JsonProperty("cardType")]
        public string CardType { get; set; } = string.Empty;
    }
}
