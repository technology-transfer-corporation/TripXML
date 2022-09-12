using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTracing;

namespace VirtualCreditCard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITracer _tracer;

        public BaseController(
            ILogger logger,
            ITracer tracer
            )
        {
            _logger = logger;
            _tracer = tracer;
        }
        protected void HandleTrace(object request)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log(JsonConvert.SerializeObject(request));

            _logger.LogInformation(JsonConvert.SerializeObject(request));
        }
    }
}
