using Microsoft.Extensions.Configuration;
using TripXML.Core.Models.ConnexPay;
using TripXML.Core.Results;
using VirtualCreditCard.ConnexPayIntegration.Api;
using VirtualCreditCard.Core.Constants;
using VirtualCreditCard.Shared.DTO;

namespace VirtualCreditCard.Infrastructure.Service
{
    public interface IConnexPayService
    {
        IConfiguration Configuration { get; set; }

        Task<IResult<CancelCardResponse>> CancelCard(string cardGuid);
        Task<IResult<CardDetailResponse>> GetCardDetails(string cardGuid, bool showFullPan);
        Task<IResult<CreateVirtualCardResponse>> IssueCard(CreateIssueCardRequest request);
        Task<IResult<CreateVirtualCardResponse>> IssueLite(CreateIssueLiteRequest request);
        Task<IResult<ITCResponse>> ITCInfo(string itc);
        Task<IResult<ListVirtualCardsResponse>> ListIssuedCard(ListVirtualCardsRequest request, PaginationParameter paginationParams);
    }

    public class ConnexPayPurchasesService : IConnexPayService
    {

        private readonly IConnexPayPurchasesApiClient _connexPayApiClient;

        public IConfiguration Configuration { get; set; }

        public ConnexPayPurchasesService(IConnexPayPurchasesApiClient connexPayApiClient,
            IConfiguration configuration)
        {
            _connexPayApiClient = connexPayApiClient;
            Configuration = configuration;
        }

        public async Task<IResult<CreateVirtualCardResponse>> IssueLite(CreateIssueLiteRequest request)
        {
            SetMerchantGuid(request);
            return await _connexPayApiClient.CreateIssueLite(request);
        }
        public async Task<IResult<CreateVirtualCardResponse>> IssueCard(CreateIssueCardRequest request)
        {
            SetMerchantGuid(request);
            return await _connexPayApiClient.CreateIssueCard(request);
        }

        public async Task<IResult<ListVirtualCardsResponse>> ListIssuedCard(ListVirtualCardsRequest request,
            PaginationParameter paginationParams)
        {
            SetMerchantGuid(request);
            return await _connexPayApiClient.GetListIssueCard(request, paginationParams);
        }

        public async Task<IResult<CardDetailResponse>> GetCardDetails(string cardGuid, bool showFullPan)
        {
            return await _connexPayApiClient.GetCardDetail(cardGuid, showFullPan);
        }

        public async Task<IResult<CancelCardResponse>> CancelCard(string cardGuid)
        {
            return await _connexPayApiClient.CancelCard(cardGuid);
        }

        public async Task<IResult<ITCResponse>> ITCInfo(string itc)
        {
            return await _connexPayApiClient.ITCInfo(itc);
        }

        private void SetMerchantGuid(IHasMerchantGuid request)
        {
            request.MerchantGuid = Configuration[ConfigurationConstants.ConnexPayMerchantGuid];
        }
    }
}
