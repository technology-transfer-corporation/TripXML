using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using TripXML.Core.Models.ConnexPay;
using VirtualCreditCard.Infrastructure.Service;
using VirtualCreditCard.Service;
using VirtualCreditCard.Shared.DTO;

namespace VirtualCreditCard.Controllers
{
    [Authorize]
    public class VirtualCreditCardController : BaseController
    {
        private readonly IConnexPayService _connexPayService;
        private readonly IConnexPaySalesService _connexPaySalesService;

        public VirtualCreditCardController(
            ITracer tracer,
            IConnexPayService connexPayService, 
            ILogger<VirtualCreditCardController> logger, 
            IConnexPaySalesService connexPaySalesService
            ) : base(logger, tracer)
        {
            _connexPayService = connexPayService;
            _connexPaySalesService = connexPaySalesService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> IssueLite([FromBody] CreateIssueLiteRequest request)
        {
            HandleTrace(request);
            return Ok(await _connexPayService.IssueLite(request));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> IssueCard([FromBody] CreateIssueCardRequest request)
        {
            HandleTrace(request);
            return Ok(await _connexPayService.IssueCard(request));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ListIssuedCard([FromBody] ListVirtualCardsRequest request, 
            [FromQuery] PaginationParameter paginationParams)
        {
            HandleTrace(request);
            return Ok(await _connexPayService.ListIssuedCard(request, paginationParams));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetCardDetails(string cardGuid, bool showFullPan = true)
        {
            HandleTrace(cardGuid);
            return Ok(await _connexPayService.GetCardDetails(cardGuid, showFullPan));

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> CancelCard(string cardGuid)
        {            
            if (string.IsNullOrEmpty(cardGuid)) return NotFound();
            HandleTrace(cardGuid);
            return Ok(await _connexPayService.CancelCard(cardGuid));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateSale([FromBody] CreateSaleRequest request)
        {
            HandleTrace(request);
            var result = await _connexPaySalesService.CreateSale(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CancelSale([FromBody] CancelSaleRequest request)
        {
            HandleTrace(request);
            var result = await _connexPaySalesService.CancelSale(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("ITC/{itc}")]
        public async Task<IActionResult> GetITCInfo(string itc)
        {
            HandleTrace(itc);
            return Ok(await _connexPayService.ITCInfo(itc));
        }
    }
}