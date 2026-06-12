# ADR 0001: Adopt CoreWCF to preserve ASMX SOAP wire contracts

Status: Accepted (2026-06-11)

## Context

The legacy host was an ASP.NET (.NET Framework) ASMX application exposing 104 SOAP 1.1 document/literal services. External B2B clients integrate against the exact wire shape: wrapper element names equal to the web-method name, `{method}Result` response members, a `TripXML` SOAP header, the `http://tripxml.downtowntravel.com/tripxml/{service}` namespace per service, and `SoapServiceRoutingStyle.RequestElement` routing (many clients send an empty or non-standard SOAPAction). ASMX does not exist on .NET 10, and any change to the wire shape forces every client to re-integrate. The repo carries 103 `.asmx.cs` code-behind files plus three plain-`.cs` services (src/wsTripXML/Code/wsStoredFareBuild.cs, src/wsTripXML/Code/wsStoredFareBuild_v03.cs, src/wsTripXML/Code/wsStoredFareUpdate.cs); `wsCruiseCabinUnUnhold` is declared inside src/wsTripXML/Code/wsCruiseCabinUnhold.asmx.cs.

## Decision

Host the existing service classes on CoreWCF with generated contracts that replicate the ASMX wire format:

- tools/Generate-Contracts.ps1 emits src/wsTripXML/Code/Generated/ — 105 `.g.cs` files: 104 service contract files plus ServiceRoutes.g.cs, whose `ServiceRoutes.All` array has exactly 104 entries mapping implementation, contract interface, and `.asmx` route (e.g. `wsCruiseCabinUnUnhold` → `/wsCruiseCabinUnhold.asmx`).
- Each contract uses `[XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]`, `[MessageContract]` request/response wrappers, `[MessageHeader] TripXML`, and `{method}Result` body members — see src/wsTripXML/Code/Generated/wsPing.g.cs (`wsPing_wmPingResponse.wmPingResult`).
- `RequestElementRoutingBehavior` and `RequestElementOperationSelector` (src/wsTripXML/Code/Classes/RequestElementRouting.cs) reproduce RequestElement routing: the endpoint `ContractFilter` is replaced with `MatchAllMessageFilter` and the operation is selected from the first SOAP body element, so SOAPAction is ignored exactly as ASMX did.
- src/wsTripXML/Program.cs registers every `ServiceRoutes.All` entry on a per-service `BasicHttpBinding` (SOAP 1.1 wire format) with the binding `Namespace` set to the contract namespace and 64 MB message limits; WSDL metadata stays enabled via `ServiceMetadataBehavior`.

Landed in commit bf710ba (283 files, +6,533/−8,055), which also fixed 10 endpoints that return HTTP 500 on legacy production.

## Alternatives considered

- **REST/JSON rewrite** — rejected. The SOAP contracts are the product; every B2B client would be forced to migrate on our schedule. Nothing in the backlog funds that.
- **gRPC** — rejected. Same forced-client-migration problem, plus partners do not speak a binary protocol.
- **Stay on .NET Framework 4.8** — rejected. Blocks the entire modernization (SDK-style projects, current runtime, current SqlClient) and keeps the codebase on an EOL trajectory.
- **SoapCore** — rejected. Middleware-level SOAP shim without the WCF dispatch pipeline; the RequestElement parity above needs `IServiceBehavior`/`IDispatchOperationSelector` extensibility and first-class WSDL generation, which CoreWCF (the maintained WCF lineage) provides.

## Consequences

- Positive: existing clients keep working against unchanged URLs (`/{service}.asmx`) and unchanged envelopes. Measured on the new stack (localhost loopback, Release, no legacy-side baseline exists): host cold start to first HTTP 200 from `/wsPing.asmx?wsdl` with all 104 endpoints registered was 1106/1113/1159 ms over 3 runs; wsPing SOAP round-trip steady state median 2.02 ms over 20 calls (first call 158.43 ms including XmlSerializer first-use codegen).
- Negative: we own a CoreWCF dependency and a code generator. The `.g.cs` files are marked "do not edit by hand" — fixes belong in tools/Generate-Contracts.ps1, then regenerate.
- **Do not undo casually.** Do not convert these endpoints to REST/minimal APIs, do not swap `XmlSerializerFormat` for `DataContractSerializer`, and do not remove `RequestElementRoutingBehavior` — clients sending empty SOAPAction would start failing with dispatch faults while tests against well-behaved clients still pass.
- Revisit only if: every consuming client is confirmed migrated off SOAP, or CoreWCF is abandoned upstream with no successor.

Related: [migration report](../migration-report.md), [architecture](../architecture.md), [performance](../performance.md), [ADR 0003 (outbound SOAP clients)](0003-handrolled-soap-clients.md).
