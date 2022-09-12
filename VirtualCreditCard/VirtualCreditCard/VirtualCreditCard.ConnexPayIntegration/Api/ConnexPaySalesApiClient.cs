using RestSharp;
using TripXML.Core.Models.ConnexPay;
using TripXML.Core.Results;
using VirtualCreditCard.Core.Constants;
using VirtualCreditCard.Core.Service;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class ConnexPaySalesApiClient : BaseApiClient, IConnexPaySalesApiClient
    {
        public ConnexPaySalesApiClient(
            ConnexPaySalesAuthService connexPaySalesAuthService, 
            IUrlService urlService
            )
        {
            ConnexPayAuthService = connexPaySalesAuthService;
            Client = new RestClient(urlService.BaseConnexPaySalesUrl);
        }
        public async Task<IResult<CreateSaleResponse>> CreateSale(CreateSaleRequest createSaleRQ)
        {
            var request = await CreatePostRequest(UrlConstants.CreateSale, createSaleRQ);
            return await ExecuteRequest<CreateSaleResponse>(request);
        }

        public async Task<IResult<CancelSaleResponse>> CancelSale(CancelSaleRequest cancelSaleRQ)
        {
            var request = await CreatePostRequest(UrlConstants.CancelSale, cancelSaleRQ);
            return await ExecuteRequest<CancelSaleResponse>(request);
        }
    }

    public interface IConnexPaySalesApiClient
    {
        Task<IResult<CancelSaleResponse>> CancelSale(CancelSaleRequest cancelSaleRQ);
        Task<IResult<CreateSaleResponse>> CreateSale(CreateSaleRequest createSaleRQ);
    }
}
