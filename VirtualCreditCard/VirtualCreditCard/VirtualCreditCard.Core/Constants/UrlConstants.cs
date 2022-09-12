namespace VirtualCreditCard.Core.Constants
{
    public static class UrlConstants
    {
        public const string Token = "token";
        public const string IssueLite = "IssueCard/IssueLite";
        public const string IssueCard = "IssueCard";
        public const string ListIssueCard = "Search/IssuedCards/{PageNumber}/{PageSize}";
        public const string CancelCard = "IssueCard/Cancel/{CardGuid}";
        public const string DetailCard = "Cards/{CardGuid}/{ShowFullPan}";
        public const string IncomingTransactionCode = "IncomingTransactionCodes/{ITC}";
        public const string CreateSale = "Sales";
        public const string CancelSale = "Cancel";


        public const string BinListBaseUrl = "https://lookup.binlist.net/";
    }
}
