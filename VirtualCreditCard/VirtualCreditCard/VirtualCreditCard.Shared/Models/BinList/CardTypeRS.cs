namespace VirtualCreditCard.Shared.Models
{
    public class CardTypeRS
    {
        public Number Number { get; set; } = null!;
        public string Scheme { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public bool Prepaid { get; set; }
        public Country Country { get; set; } = null!;
        public Bank Bank { get; set; } = null!;
    }
    
    public class Number
    {
        public int Length { get; set; }
        public bool Luhn { get; set; }
    }

    public class Country
    {
        public string Numeric { get; set; } = string.Empty;
        public string Alpha2 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }

    public class Bank
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
