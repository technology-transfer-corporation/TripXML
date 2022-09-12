namespace VirtualCreditCard.Shared.DTO
{
    public class ConnexPayError
    {
        public string Message { get; set; } = string.Empty;
        public long ErrorId { get; set; }
        public Dictionary<string, string[]>? ModelState { get; set; }
    }
}
