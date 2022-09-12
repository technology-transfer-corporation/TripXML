using Microsoft.Extensions.Configuration;
using VirtualCreditCard.ConnexPayIntegration.Api;
using VirtualCreditCard.Core.Service;
using VirtualCreditCard.Core.Service.Interfaces;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class ConnexPaySalesAuthService : BaseConnexPayAuth
    {
        public ConnexPaySalesAuthService(IStorageService storageService, IConfiguration configuration,
            IUrlService urlService): base(storageService, configuration)
        {
            TokenKey = "SALES_TOKEN";
            GetTokenUrl = urlService.GetSalesTokenUrl();
        }
    }
}
