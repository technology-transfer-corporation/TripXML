namespace VirtualCreditCard.Core.Constants
{
    public static class ConfigurationConstants
    {
        public const string ConnexPay = "ConnexPay";
        public const string ConnexPayPerchasesBaseUrl = $"{ConnexPay}:BasePurchasesUrl";
        public const string ConnexPaySalesBaseUrl = $"{ConnexPay}:BaseSalesUrl";
        public const string ConnexPayMerchantGuid = $"{ConnexPay}:MerchantGuid";
        public const string ConnexPayDeviceGuid = $"{ConnexPay}:DeviceGuid";
        public const string ConnexPayUsername = $"{ConnexPay}:Username";
        public const string ConnexPayPassword = $"{ConnexPay}:Password";
    }
}
