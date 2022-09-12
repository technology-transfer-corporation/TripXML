using RestSharp;
using TripXML.Core.Models.ConnexPay;
using TripXML.Core.Results;
using VirtualCreditCard.Core.Constants;
using VirtualCreditCard.Core.Service;
using VirtualCreditCard.Shared.DTO;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public interface IConnexPayPurchasesApiClient
    {
        Task<IResult<CancelCardResponse>> CancelCard(string cardGuid);
        Task<IResult<CreateVirtualCardResponse>> CreateIssueCard(CreateIssueCardRequest issueCardRQ);
        Task<IResult<CreateVirtualCardResponse>> CreateIssueLite(CreateIssueLiteRequest issueLiteRQ);
        Task<IResult<CardDetailResponse>> GetCardDetail(string cardGuid, bool showFullPan);
        Task<IResult<ListVirtualCardsResponse>> GetListIssueCard(ListVirtualCardsRequest listVirtualCardsRequest, PaginationParameter paginationParams);
        Task<IResult<ITCResponse>> ITCInfo(string itc);
    }

    public class ConnexPayPurchasesApiClient : BaseApiClient, IConnexPayPurchasesApiClient
    {
        private readonly IUrlService _urlService;

        public ConnexPayPurchasesApiClient(
            IUrlService urlService,
            ConnexPayPurchaseAuthService cpAuthService
            )
        {
            _urlService = urlService;
            ConnexPayAuthService = cpAuthService;
            Client = new RestClient(_urlService.BaseConnexPayPurchasesUrl);
        }

        public async Task<IResult<CreateVirtualCardResponse>> CreateIssueLite(CreateIssueLiteRequest issueLiteRQ)
        {
            var request = await CreatePostRequest(UrlConstants.IssueLite, issueLiteRQ);

            return await ExecuteRequest<CreateVirtualCardResponse>(request);
        }

        public async Task<IResult<CreateVirtualCardResponse>> CreateIssueCard(CreateIssueCardRequest issueCardRQ)
        {
            var request = await CreatePostRequest(UrlConstants.IssueCard, issueCardRQ);

            return await ExecuteRequest<CreateVirtualCardResponse>(request);
        }

        public async Task<IResult<ListVirtualCardsResponse>> GetListIssueCard(
            ListVirtualCardsRequest listVirtualCardsRequest,
            PaginationParameter paginationParams)
        {
            var request = await CreatePostRequest(UrlConstants.ListIssueCard, listVirtualCardsRequest);

            request.AddUrlSegment(nameof(paginationParams.PageNumber),
                paginationParams.PageNumber.ToString());

            request.AddUrlSegment(nameof(paginationParams.PageSize),
                paginationParams.PageSize.ToString());

            return await ExecuteRequest<ListVirtualCardsResponse>(request);
        }

        public async Task<IResult<CancelCardResponse>> CancelCard(string cardGuid)
        {
            var request = new RestRequest(UrlConstants.CancelCard, Method.PUT)
                .AddUrlSegment("CardGuid", cardGuid);
            await FillRequestCommonHeaders(request);

            return await ExecuteRequest<CancelCardResponse>(request);
        }

        public async Task<IResult<CardDetailResponse>> GetCardDetail(string cardGuid, bool showFullPan)
        {
            var request = new RestRequest(UrlConstants.DetailCard, Method.GET)
                .AddUrlSegment("CardGuid", cardGuid)
                .AddUrlSegment("ShowFullPan", showFullPan);
            await FillRequestCommonHeaders(request);

            return await ExecuteRequest<CardDetailResponse>(request);
        }

        public async Task<IResult<ITCResponse>> ITCInfo(string itc)
        {
            var request = new RestRequest(UrlConstants.IncomingTransactionCode, Method.GET)
                .AddUrlSegment("ITC", itc);
            return await ExecuteRequest<ITCResponse>(request);
        }
    }
}
