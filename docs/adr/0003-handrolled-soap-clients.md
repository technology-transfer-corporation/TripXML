# ADR 0003: Hand-rolled SOAP/HTTP clients on shared HttpClient instead of generated proxies

Status: Accepted (2026-06-11)

## Context

Outbound GDS calls used generated `SoapHttpClientProtocol` proxies (Visual Studio "Web References") and `HttpWebRequest` transports. `System.Web.Services` does not exist on net10.0. The GDS endpoints are sensitive to envelope details that proxy generators do not guarantee: namespace declaration order, BOM-less UTF-8, per-system SOAPAction prefixes, and quoted SOAPAction headers. The legacy transports also depended on a private-reflection hack — reading `HttpWebRequest._HttpResponse` via `FieldInfo` — to recover SOAP fault bodies from non-2xx responses.

## Decision

Replace generated proxies with hand-rolled clients that own every envelope byte, on `static` shared `HttpClient`/`SocketsHttpHandler` instances (`PooledConnectionLifetime = TimeSpan.FromMinutes(5)` keeps DNS fresh):

- `GalileoSoapClient` (src/Galileo/Classes/GalileoSoapClient.cs, commit 21a86e9: 11 files, +451/−1,006) replaces the four Galileo proxies. Request envelopes are byte-identical to the .NET Framework 4.8 proxy output, verified against a loopback listener: UTF-8 without BOM, no indentation, namespaces in soap/xsi/xsd order, parameters in declared order. It preserves the legacy `MS Web Services Client Protocol` user agent, the per-system SOAPAction prefix (`http://webservices.galileo.com/` production vs `https://...` copy), and SOAP faults throw `GalileoSoapFaultException` with `SoapException`-parity messages.
- `BlackListClient` (src/Amadeus/BlackListClient.cs, part of commit b569756: 7 files, +469/−589) replaces the `wsBlackList` Web Reference: `wmFlightAdd` with quoted SOAPAction (`"http://admintalk/wsFlightBlackList/wmFlightAdd"`), `xs:dateTime` in `XmlDateTimeSerializationMode.RoundtripKind`, fault → exception mapping.
- The `ttHttpWebClient` transports were rewritten on `HttpClient` with unchanged public signatures in Sabre (commit 487d7a1: 3 files, +126/−314), Worldspan (9e9c590: 3 files, +132/−317), TravelPort (85eafde: 3 files, +52/−177), and Amadeus (b569756). Non-2xx bodies (e.g. Sabre SOAP faults on HTTP 500) are now read on the normal path — see the comment in src/Sabre/ttHttpWebClient.cs — which removes the `_HttpResponse` reflection hack in those transports.
- Known residual: src/Galileo/ttHttpWebClient.cs still runs on `HttpWebRequest` and still carries the `_HttpResponse` reflection fallback (line 81). Galileo XML Select traffic goes through `GalileoSoapClient`; this class is the remaining legacy path and a candidate for the same rewrite.

## Alternatives considered

- **svcutil / dotnet-svcutil / Connected Services (WCF client proxies)** — rejected. Generated WCF clients control neither namespace-prefix ordering nor SOAPAction quoting quirks, and parity with the captured legacy envelopes was the acceptance test. They also pull the WCF client stack into every GDS assembly for what is, on the wire, a string template plus an HTTP POST.
- **Keep `HttpWebRequest`** — rejected for new code. It is obsolete on modern .NET, and its WebException path is what forced the reflection hack.
- **`IHttpClientFactory` typed clients** — rejected for this pass. The GDS assemblies are class libraries consumed by legacy call sites without DI; a static pooled client with `PooledConnectionLifetime` gives the same socket/DNS hygiene without threading a container through them (same reasoning as [ADR 0004](0004-appstate-facade.md)).

## Consequences

- Positive: envelope bytes are under version control and reviewable; fault bodies are readable without reflection; one pooled connection set per process per GDS.
- Negative: WSDL changes on the GDS side must be hand-applied; nothing regenerates these clients.
- **Do not undo casually.** Do not regenerate proxies with svcutil "to clean this up" — byte-level parity was verified against captured legacy traffic and a generated client silently changes it. Do not replace the `static readonly HttpClient` instances with per-call `new HttpClient()` (socket exhaustion) and do not move them behind DI without preserving the envelope bytes, timeout semantics, and the cert-validation gate (`AmadeusSkipCertValidation`, see [ADR 0005](0005-modcore-config-bridge.md)).
- Revisit only if: a GDS publishes a breaking WSDL change large enough that hand-maintenance loses to regeneration plus a fresh byte-parity capture.

Related: [architecture](../architecture.md), [migration report](../migration-report.md).
