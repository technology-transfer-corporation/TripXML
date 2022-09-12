using Microsoft.Extensions.Configuration;
using VirtualCreditCard.Core.Service;
using VirtualCreditCard.Core.Service.Interfaces;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class ConnexPayPurchaseAuthService : BaseConnexPayAuth
    {
        public ConnexPayPurchaseAuthService(IStorageService storageService, IUrlService urlService,
            IConfiguration configuration) : base(storageService, configuration)
        {
            TokenKey = "PURCHASE_TOKEN";
            GetTokenUrl = urlService.GetPurchasesTokenUrl();
        }
    }
}
