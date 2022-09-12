namespace VirtualCreditCard.Shared.DTO
{
    public class CheckCardRqDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Cvv2 { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
