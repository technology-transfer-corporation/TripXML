using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wsTripXML.Hosting;
using wsTripXML.wsTravelTalk;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<modMain>();

// Behaviors registered in DI are applied by CoreWCF to every service host:
// SOAP request/fault logging (ported ASMX SoapExtension) + ASMX RequestElement routing parity.
builder.Services.AddSingleton<IServiceBehavior, SoapRequestInspectorBehavior>();
builder.Services.AddSingleton<IServiceBehavior, RequestElementRoutingBehavior>();

foreach (var s in ServiceRoutes.All)
{
    builder.Services.AddTransient(s.Implementation);
}

var app = builder.Build();

TripXMLRuntime.Initialize(app);

// An https-bound endpoint can only be registered when the server actually listens on https.
string serverUrls = app.Configuration["urls"]
    ?? System.Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
    ?? string.Empty;
bool enableHttps = serverUrls.Contains("https", System.StringComparison.OrdinalIgnoreCase);

bool includeFaultDetail = app.Configuration.GetValue("IncludeExceptionDetailInFaults", false);

((IApplicationBuilder)app).UseServiceModel(sb =>
{
    foreach (var s in ServiceRoutes.All)
    {
        // ASMX served SOAP 1.1 over HTTP(S); BasicHttpBinding matches the wire format.
        // Per-service binding instances so the WSDL binding lands in the service namespace
        // (ASMX kept everything in one namespace; WCF would default to tempuri.org).
        var contractNs = ((CoreWCF.ServiceContractAttribute)System.Attribute
            .GetCustomAttribute(s.Contract, typeof(CoreWCF.ServiceContractAttribute)))?.Namespace;
        var http = new BasicHttpBinding
        {
            MaxReceivedMessageSize = 64_000_000,
            MaxBufferSize = 64_000_000,
            Namespace = contractNs
        };

        var route = new System.Uri(s.Route, System.UriKind.Relative);
        sb.AddService(s.Implementation, o => o.DebugBehavior.IncludeExceptionDetailInFaults = includeFaultDetail);
        sb.AddServiceEndpoint(s.Implementation, s.Contract, http, route, null);
        if (enableHttps)
        {
            var https = new BasicHttpBinding(CoreWCF.Channels.BasicHttpSecurityMode.Transport)
            {
                MaxReceivedMessageSize = 64_000_000,
                MaxBufferSize = 64_000_000,
                Namespace = contractNs
            };
            sb.AddServiceEndpoint(s.Implementation, s.Contract, https, route, null);
        }
    }
});

var metadata = app.Services.GetRequiredService<ServiceMetadataBehavior>();
metadata.HttpGetEnabled = true;
metadata.HttpsGetEnabled = true;

app.Run();
