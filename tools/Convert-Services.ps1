<#
.SYNOPSIS
Mechanically converts ASMX service code-behinds to CoreWCF-compatible partial classes:
strips System.Web.Services attributes (the contract metadata now lives in generated
interfaces), replaces the designer region with a modMain DI constructor, swaps
Application state for TripXMLMain.AppState, and removes System.Web usings.

Idempotent: running twice produces no further changes.

.PARAMETER HostRoot
Path to the wsTripXML-converted repo.

.PARAMETER Files
Optional list of file names (e.g. wsPing.asmx.cs). When omitted, converts every
*.asmx.cs under Code\ and the repo root.
#>
param(
    [string]$HostRoot = (Join-Path $PSScriptRoot '..\src\wsTripXML'),
    [string[]]$Files = @()
)

$ErrorActionPreference = 'Stop'
$HostRoot = (Resolve-Path $HostRoot).Path

$targets = @(Get-ChildItem (Join-Path $HostRoot 'Code') -Filter *.asmx.cs -Recurse -File) +
           @(Get-ChildItem $HostRoot -Filter *.asmx.cs -File) +
           @(Get-ChildItem (Join-Path $HostRoot 'Code') -Filter 'wsStoredFare*.cs' -File)
if ($Files.Count -gt 0) { $targets = $targets | Where-Object { $Files -contains $_.Name } }

$attrPatterns = @(
    '^\s*\[CompressionExtension\.CompressionExtension\(\)\]\s*\r?\n',
    '^\s*\[(System\.Web\.Services\.Protocols\.)?SoapHeader\("\w+"[^\]]*\)\]\s*\r?\n',
    '^\s*\[WebMethod[^\]]*\]\s*\r?\n',
    '^\s*\[ScriptMethod[^\]]*\]\s*\r?\n',
    '^\s*\[ScriptService\(\)\]\s*\r?\n',
    '^\s*\[WebServiceBinding[^\]]*\]\s*\r?\n',
    '^\s*\[(System\.Web\.Services\.Protocols\.)?SoapDocumentService[^\]]*\]\s*\r?\n',
    '^\s*\[WebService\([^\]]*\]\s*\r?\n',
    '^\s*\[System\.ComponentModel\.ToolboxItem\(false\)\]\s*\r?\n'
)

$converted = 0; $warnings = @()

foreach ($f in $targets) {
    $raw = Get-Content $f.FullName -Raw
    $orig = $raw

    # 1) strip ASMX attributes (multiline regex)
    foreach ($p in $attrPatterns) {
        $raw = [regex]::Replace($raw, $p, '', 'Multiline')
    }

    # 2) drop the WebService base class, make the class partial
    $raw = [regex]::Replace($raw, 'public class (\w+) : WebService', 'public partial class $1')

    # 3) designer region -> DI constructor
    $classMatch = [regex]::Match($raw, 'public partial class (\w+)')
    if ($classMatch.Success) {
        $clsName = $classMatch.Groups[1].Value
        $ctor = @"
private readonly modMain _modMain;

        public $clsName(modMain modMain)
        {
            _modMain = modMain;
        }
"@
        $regionPattern = '(?s)#region\s+Web Services Designer Generated Code.*?#endregion'
        if ($raw -match $regionPattern) {
            $raw = [regex]::Replace($raw, $regionPattern, $ctor)
        } elseif ($raw -notmatch '_modMain') {
            $warnings += "$($f.Name): no designer region found - DI ctor not injected"
        }
    }

    # 4) Application state plumbing
    $raw = [regex]::Replace($raw, '^\s*var argoApp = Application;\s*\r?\n', '', 'Multiline')
    $raw = $raw -replace 'ref argoApp, ', ''
    $raw = $raw -replace 'Application\.Get\(', 'TripXMLMain.AppState.Get('
    $raw = $raw -replace 'Application\.Set\(', 'TripXMLMain.AppState.Set('
    $raw = $raw -replace 'Application\.Remove\(', 'TripXMLMain.AppState.Remove('
    $raw = $raw -replace 'Server\.MachineName', 'Environment.MachineName'

    # 5) modMain instance methods go through the injected instance (statics stay type-qualified)
    $raw = [regex]::Replace($raw, '\bmodMain\.(PreServiceRequestPool|PreServiceRequest|ndcPreServiceRequest|LogResponse|LogDeals|GetDeals)\(', '_modMain.$1(')

    # 6) remove System.Web usings (Microsoft.VisualBasic stays)
    $raw = [regex]::Replace($raw, '^using System\.Web(\.[\w.]+)?;\s*\r?\n', '', 'Multiline')

    if ($raw -ne $orig) {
        Set-Content -Path $f.FullName -Value $raw -Encoding UTF8 -NoNewline
        $converted++
    }
}

Write-Host "converted: $converted file(s)"
if ($warnings.Count) { $warnings | ForEach-Object { Write-Warning $_ } }
