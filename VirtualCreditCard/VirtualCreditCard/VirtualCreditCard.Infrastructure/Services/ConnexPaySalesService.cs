using Microsoft.Extensions.Configuration;
using TripXML.Core.Models.ConnexPay;
using TripXML.Core.Results;
using VirtualCreditCard.ConnexPayIntegration.Api;
using VirtualCreditCard.Core.Constants;

namespace VirtualCreditCard.Service
{
    public class ConnexPaySalesService : IConnexPaySalesService
    {
        private readonly IConnexPaySalesApiClient _connexPaySalesApiClient;
        private readonly IConfiguration _configuration;

        public ConnexPaySalesService(IConnexPaySalesApiClient connexPaySalesApiClient,
            IConfiguration configuration)
        {
            _connexPaySalesApiClient = connexPaySalesApiClient;
            _configuration = configuration;
        }
        public async Task<IResult<CreateSaleResponse>> CreateSale(CreateSaleRequest request)
        {
            request.DeviceGuid = _configuration[ConfigurationConstants.ConnexPayDeviceGuid];

            return await _connexPaySalesApiClient.CreateSale(request);
        }

        public async Task<IResult<CancelSaleResponse>> CancelSale(CancelSaleRequest request)
        {
            request.DeviceGuid = _configuration[ConfigurationConstants.ConnexPayDeviceGuid];

            return await _connexPaySalesApiClient.CancelSale(request);
        }
    }

    public interface IConnexPaySalesService
    {
        Task<IResult<CancelSaleResponse>> CancelSale(CancelSaleRequest request);
        Task<IResult<CreateSaleResponse>> CreateSale(CreateSaleRequest request);
    }
}
