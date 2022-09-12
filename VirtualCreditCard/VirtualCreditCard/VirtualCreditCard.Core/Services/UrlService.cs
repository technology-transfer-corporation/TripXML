using Microsoft.Extensions.Configuration;
using VirtualCreditCard.Core.Constants;

namespace VirtualCreditCard.Core.Service
{
    public class UrlService : IUrlService
    {
        private readonly IConfiguration Configuration;
        public string BaseConnexPayPurchasesUrl { get; }
        public string BaseConnexPaySalesUrl { get; }

        public UrlService(IConfiguration configuration)
        {
            Configuration = configuration;
            BaseConnexPayPurchasesUrl = Configuration[ConfigurationConstants.ConnexPayPerchasesBaseUrl];
            BaseConnexPaySalesUrl = Configuration[ConfigurationConstants.ConnexPaySalesBaseUrl];
        }

        public string GetPurchasesTokenUrl() => BaseConnexPayPurchasesUrl + UrlConstants.Token;

        public string GetSalesTokenUrl() => BaseConnexPaySalesUrl + UrlConstants.Token;
    }

    public interface IUrlService
    {
        string BaseConnexPayPurchasesUrl { get; }
        string BaseConnexPaySalesUrl { get; }
        string GetPurchasesTokenUrl();
        string GetSalesTokenUrl();
    }
}


