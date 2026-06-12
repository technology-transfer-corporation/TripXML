using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WebSocketSharp;

namespace AmadeusWS
{

    // ══════════════════════════════════════════════════════════════════════════════
    //  Shared value types
    // ══════════════════════════════════════════════════════════════════════════════

public class MoneyAmount
{
    public string  Currency { get; set; } = string.Empty;
    public decimal Amount   { get; set; }

    public override string ToString()
    {
        // Integer currencies (JPY) display without decimals; all others use 2dp.
        return Amount == Math.Floor(Amount) && Amount >= 1m
            ? string.Format("{0} {1:0}", Currency, Amount)
            : string.Format("{0} {1:F2}", Currency, Amount);
    }
}

public class TaxItem
{
    public string      Code             { get; set; } = string.Empty;
    public MoneyAmount Amount           { get; set; } = new MoneyAmount();
    /// <summary>Airport fee breakdown when code is XF, e.g. "JFK4.50".</summary>
    public string      AirportBreakdown { get; set; } = string.Empty;

    public override string ToString()
    {
        return string.IsNullOrEmpty(AirportBreakdown)
            ? string.Format("{0} -{1}", Amount, Code)
            : string.Format("{0} -{1} {2}", Amount, Code, AirportBreakdown);
    }
}

public class FlightSegment
{
    public string Origin               { get; set; } = string.Empty;
    public string Destination          { get; set; } = string.Empty;
    public bool   IsTransitDestination { get; set; }
    public string Airline              { get; set; } = string.Empty;
    public int    FlightNumber         { get; set; }
    public string BookingClass         { get; set; } = string.Empty;
    public string Date                 { get; set; } = string.Empty;
    public string DepartureTime        { get; set; } = string.Empty;
    public string FareBasis            { get; set; } = string.Empty;
    /// <summary>Not-valid-before restriction date, e.g. "13SEP". Empty when not restricted.</summary>
    public string NotValidBefore       { get; set; } = string.Empty;
    /// <summary>
    /// Not-valid-after restriction date, e.g. "10DEC". Empty when not restricted.
    /// NOTE: When only one date appears in the NVB/NVA slot and it cannot be
    /// distinguished by column position (post-trim lines), it is placed in
    /// NotValidBefore.  Consumers requiring strict NVA-only detection should
    /// use raw-line column-position parsing.
    /// </summary>
    public string NotValidAfter        { get; set; } = string.Empty;
    public string BaggageAllowance     { get; set; } = string.Empty;
}

// ══════════════════════════════════════════════════════════════════════════════
//  Format discriminator
// ══════════════════════════════════════════════════════════════════════════════

public enum FareQuoteFormat { Detail, Summary }

// ══════════════════════════════════════════════════════════════════════════════
//  Detail format model
// ══════════════════════════════════════════════════════════════════════════════

public class FareQuoteData
{
    public string              PassengerName        { get; set; } = string.Empty;
    public string              FrequentFlyerProfile { get; set; } = string.Empty;
    /// <summary>Last ticket date as printed, e.g. "19MAR26/23:59" or "10SEP26".</summary>
    public string              LastTicketDate       { get; set; } = string.Empty;
    public string              LastTicketDateSuffix { get; set; } = string.Empty;
    public string              FareType             { get; set; } = string.Empty;
    public List<string>        AdvisoryMessages     { get; private set; } = new List<string>();
    public List<FlightSegment> Segments             { get; private set; } = new List<FlightSegment>();
    /// <summary>Raw fare-calculation text (between the base-fare line and the END marker).</summary>
    public string              FareCalculation      { get; set; } = string.Empty;
    /// <summary>
    /// First left-column non-tax amount after segments.
    /// May be in a non-USD currency (e.g. JPY 48800).
    /// </summary>
    public MoneyAmount         BaseFare             { get; set; } = new MoneyAmount();
    public List<TaxItem>       Taxes                { get; private set; } = new List<TaxItem>();
    /// <summary>Last left-column non-tax amount after segments (grand total line).</summary>
    public MoneyAmount         TotalAmount          { get; set; } = new MoneyAmount();
    /// <summary>
    /// Exchange-rate string from the "RATE USED" line, e.g. "1JPY=0.0062981USD".
    /// Empty when not present.
    /// </summary>
    public string              ExchangeRate         { get; set; } = string.Empty;
    public string              BaggageInfo          { get; set; } = string.Empty;
    public string              PageIndicator        { get; set; } = string.Empty;
    public List<string>        UnparsedLines        { get; private set; } = new List<string>();
}

// ══════════════════════════════════════════════════════════════════════════════
//  Summary format models
// ══════════════════════════════════════════════════════════════════════════════

    public class PassengerFareSummary
    {
        public int SequenceNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PassengerTypeCode { get; set; } = string.Empty;
        public int NumberOfPassengers { get; set; }
        public decimal BaseFare { get; set; }
        public decimal TaxFeeAmount { get; set; }
        public decimal Total { get; set; }
    }

public class FareSummaryTotals
{
    public int     TotalPassengers { get; set; }
    public decimal TotalFare       { get; set; }
    public decimal TotalTaxFee     { get; set; }
    public decimal GrandTotal      { get; set; }

    public override string ToString()
    {
        return string.Format("Pax:{0}  Fare:{1:F2}  Tax:{2:F2}  Total:{3:F2}",
            TotalPassengers, TotalFare, TotalTaxFee, GrandTotal);
    }
}

public class UpsellOffer
{
    public string      CurrentProduct { get; set; } = string.Empty;
    public string      UpsellProduct  { get; set; } = string.Empty;
    public MoneyAmount Amount         { get; set; } = new MoneyAmount();
    public string      FareFamilies   { get; set; } = string.Empty;
}

public class FareNotice
{
    public string PassengerRange { get; set; } = string.Empty;
    public string Message        { get; set; } = string.Empty;
    /// <summary>Null when this notice is not a LAST TKT DTE line.</summary>
    public string LastTicketDate { get; set; }
}

public class FareQuoteSummaryData
{
    public string                     FrequentFlyerProfile { get; set; } = string.Empty;
    public string                     FareCurrency         { get; set; } = string.Empty;
    public List<PassengerFareSummary> Passengers           { get; private set; } = new List<PassengerFareSummary>();
    public FareSummaryTotals          Totals               { get; set; }
    public UpsellOffer                Upsell               { get; set; }
    public List<FareNotice>           Notices              { get; private set; } = new List<FareNotice>();
    public string                     PageIndicator        { get; set; } = string.Empty;
    public List<string>               UnparsedLines        { get; private set; } = new List<string>();

    public string LastTicketDate
    {
        get
        {
            var notice = Notices.FirstOrDefault(n => n.LastTicketDate != null);
            return notice != null ? notice.LastTicketDate : string.Empty;
        }
    }
}

// ══════════════════════════════════════════════════════════════════════════════
//  Unified result wrapper
// ══════════════════════════════════════════════════════════════════════════════

public class FareQuoteResult
{
    public FareQuoteFormat      Format  { get; set; }
    /// <summary>Populated when Format is Detail; null otherwise.</summary>
    public FareQuoteData        Detail  { get; set; }
    /// <summary>Populated when Format is Summary; null otherwise.</summary>
    public FareQuoteSummaryData Summary { get; set; }
}

// ══════════════════════════════════════════════════════════════════════════════
//  Parser
// ══════════════════════════════════════════════════════════════════════════════

public class FareQuoteParser
{
    // ── Shared ────────────────────────────────────────────────────────────────

    // Time part is optional: "19MAR26/23:59"  or  "10SEP26"
    private static readonly Regex RxLastTktDate = new Regex(
        @"LAST\s+TKT\s+DTE\s+(?<date>\d{1,2}[A-Z]{3}\d{2}(?:/\d{2}:\d{2})?)(?:\s*-\s*(?<suffix>[A-Z ]+))?",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // ── Detail: segment regexes ───────────────────────────────────────────────
    //
    // NVB/NVA date capture rules:
    //   Two consecutive dates (e.g. "13SEP10DEC") → nvb=first, nva=second
    //   One date only                              → nvb=date, nva=empty
    //     (When NVB is blank and only NVA is present, the date lands in nvb.
    //      Use raw-line column-position parsing when strict NVA detection matters.)

    // Segment with explicit dest (FXP inline, FXP/FXX two-column).
    private static readonly Regex RxSegmentWithDest = new Regex(
        @"^(?:(?<orig>[A-Z]{3})\s+)?(?<xflag>X)?(?<dest>[A-Z]{3})\s+" +
        @"(?<al>[A-Z]{2})\s+(?<flt>\d{1,4})\s+" +
        @"(?<cls>[A-Z])\s+(?<date>\d{1,2}[A-Z]{3})\s+(?<time>\d{4})\s+" +
        @"(?<fb>[A-Z0-9]+(?:/[A-Z0-9]+)?)\s+" +
        @"(?:(?<nvb>\d{1,2}[A-Z]{3})(?<nva>\d{1,2}[A-Z]{3})?\s+)?" +
        @"(?<bg>\d+[KPF]|0P)\s*(?<fin>X?[A-Z]{3})?$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Airline-first segment (FXP inline continuation: "AA 823 Q 27JUL 0930 … 0P XMIA").
    private static readonly Regex RxSegmentAirlineFirst = new Regex(
        @"^(?<al>[A-Z]{2})\s+(?<flt>\d{1,4})\s+" +
        @"(?<cls>[A-Z])\s+(?<date>\d{1,2}[A-Z]{3})\s+(?<time>\d{4})\s+" +
        @"(?<fb>[A-Z0-9]+(?:/[A-Z0-9]+)?)\s+" +
        @"(?:(?<nvb>\d{1,2}[A-Z]{3})(?<nva>\d{1,2}[A-Z]{3})?\s+)?" +
        @"(?<bg>\d+[KPF]|0P)\s*(?<xfin>X)?(?<fin>[A-Z]{3})?$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Standalone 3-letter city code on its own line (FXP 2-col initial-origin).
    private static readonly Regex RxStandaloneCity = new Regex(
        @"^([A-Z]{3})$",
        RegexOptions.Compiled);

    // ── Detail: monetary / tax regexes ────────────────────────────────────────

    // Left-column monetary anchor.
    // Amount decimal is optional to support integer currencies (JPY):
    //   "JPY    48800      27JUL26TYO PR ..."
    //   "USD   813.00      27JUL26WAS AA ..."
    //   "USD    11.20-AY   USD813.00END"
    //   "USD  1000.93      4.80-IJ ..."
    private static readonly Regex RxLeftMoney = new Regex(
        @"^\s*(?<cur>[A-Z]{3})\s+(?<amt>\d+(?:\.\d+)?)(?:\s*-(?<code>[A-Z0-9]{2,3}))?(?:\s+(?<rest>.+))?$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Inline tax anywhere in a string: "USD 11.20-AY" or "USD11.20-AY"
    private static readonly Regex RxInlineTax = new Regex(
        @"(?<cur>[A-Z]{3})\s*(?<amt>\d+(?:\.\d+)?)-(?<code>[A-Z0-9]{2,3})",
        RegexOptions.Compiled);

    // No-currency tax: "4.80-IJ" — inherits currency from last seen CUR.
    private static readonly Regex RxNoCurTax = new Regex(
        @"(?<![.\d/])(?<amt>\d+\.\d+)-(?<code>[A-Z]{2,3})\b",
        RegexOptions.Compiled);

    // Base-fare confirmation token: "USD813.00END" or "JPY48800END"
    private static readonly Regex RxBaseFareConfirm = new Regex(
        @"(?<cur>[A-Z]{3})\s*\d+(?:\.\d+)?END",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Airport XF breakdown: "JFK4.50" or "DCA4.50CLT4.50MIA4.50"
    private static readonly Regex RxXfBreakdown = new Regex(
        @"[A-Z]{3}\d+\.\d+",
        RegexOptions.Compiled);

    // Exchange-rate line: "RATE USED 1JPY=0.0062981USD"
    private static readonly Regex RxExchangeRate = new Regex(
        @"RATE\s+USED\s+(?<rate>1[A-Z]{3}=[\d.]+[A-Z]{3})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // ── Summary-format regexes ────────────────────────────────────────────────

    private static readonly Regex RxSummaryColHeader = new Regex(
        @"PASSENGER\s+PTC\s+NP\s+FARE",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxSummaryHeader = new Regex(
        @"^FX[XP]/R(?:[^/]*/|/)FF-(?<ff>[A-Z0-9 ]+?)(?:\s{2,}|$)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxPassengerRow = new Regex(
        @"^(?<seq>\d{2})\s+(?<n>[A-Z/]+)\*?\s+" +
        @"(?<ptc>[A-Z]{3})\s+(?<np>\d+)\s+" +
        @"(?<fare>\d+\.\d+)\s+(?<tax>\d+\.\d+)\s+(?<total>\d+\.\d+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxTotalsRow = new Regex(
        @"TOTALS\s+(?<np>\d+)\s+(?<fare>\d+\.\d+)\s+(?<tax>\d+\.\d+)\s+(?<total>\d+\.\d+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxUpsell = new Regex(
        @"FXU/TS\s+TO\s+UPSELL\s+(?<from>[A-Z0-9]+)-(?<to>[A-Z0-9]+)\s+FOR\s+(?<amt>\d+\.\d+)(?<cur>[A-Z]{3})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxFareFamilies = new Regex(
        @"^(?<range>\d+(?:-\d+)?)\s+FARE\s+FAMILIES:(?<families>.+)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxRangedNotice = new Regex(
        @"^(?<range>\d+(?:-\d+)?)\s+(?<msg>.+)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxPage = new Regex(
        @"PAGE\s+(?<page>\d+/\s*\d+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RxFareCurrency = new Regex(
        @"FARE\s+(?<cur>[A-Z]{3})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // ═════════════════════════════════════════════════════════════════════════
    //  Public entry point
    // ═════════════════════════════════════════════════════════════════════════

    public FareQuoteResult Parse(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            throw new ArgumentNullException("raw");

        var lines = NormaliseLines(raw);

        if (IsSummaryFormat(lines))
            return new FareQuoteResult { Format = FareQuoteFormat.Summary, Summary = ParseSummary(lines) };
        else
            return new FareQuoteResult { Format = FareQuoteFormat.Detail,  Detail  = ParseDetail(lines)  };
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Format detection
    // ─────────────────────────────────────────────────────────────────────────

    private static bool IsSummaryFormat(List<string> lines)
    {
        return lines.Any(l => RxSummaryColHeader.IsMatch(l));
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Detail parser
    // ─────────────────────────────────────────────────────────────────────────
    //
    //  Layout variants handled
    //  ────────────────────────
    //  FXP inline (USD):
    //    USD 813.00 27JUL26WAS AA …     ← base fare (with optional time in TKT date)
    //    USD 1000.93                     ← grand total on its own line
    //
    //  FXP/FXX two-column (multi-currency, e.g. JPY base + USD taxes):
    //    JPY    48800      10SEP26TYO …  ← integer base fare (JPY, no decimal)
    //    USD   307.00      PR X/MNL …   ← second non-tax amount in fare calc
    //    USD   401.80-YQ   END ROE…     ← primary tax | END marker in right col
    //    USD   868.23      7.39-YC …    ← grand total | extra taxes in right col
    //    RATE USED 1JPY=0.0062981USD    ← exchange rate line
    //
    //  Segment NVB/NVA dates:
    //    "13SEP10DEC" → nvb=13SEP  nva=10DEC  (two dates concatenated)
    //    "10DEC"      → nvb=10DEC  nva=empty  (single date; may be NVA in context)
    // ─────────────────────────────────────────────────────────────────────────

    private enum DetailPhase { Header, Segments, Pricing }

    private FareQuoteData ParseDetail(List<string> lines)
    {
        var result      = new FareQuoteData();
        var phase       = DetailPhase.Header;
        var lastDest    = string.Empty;
        var lastCur     = "USD";
        var baseFareSet = false;

        var fareCalcParts = new List<string>();
        var fareCalcDone  = false;

        foreach (var rawLine in lines)
        {
            var trimmed = rawLine.Trim();
            if (trimmed.Length == 0) continue;

            // ── Always-active parsers ─────────────────────────────────────────

            // FXP/R or FXX/R header
            if (trimmed.StartsWith("FXP/R", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("FXX/R", StringComparison.OrdinalIgnoreCase))
            {
                ParseDetailHeader(trimmed, result);
                continue;
            }

            // Passenger line: "01 GOR/ANNA"
            var mPaxSeq = Regex.Match(trimmed,
                @"^\d{2}\s+(?<n>[A-Z]+/[A-Z]+)\s*$", RegexOptions.IgnoreCase);
            if (mPaxSeq.Success)
            {
                result.PassengerName = mPaxSeq.Groups["n"].Value.ToUpper();
                continue;
            }

            // Last ticket date — time optional, suffix optional ("- DATE OF ORIGIN")
            var mTkt = RxLastTktDate.Match(trimmed);
            if (mTkt.Success)
            {
                result.LastTicketDate = mTkt.Groups["date"].Value;
                if (mTkt.Groups["suffix"].Success)
                    result.LastTicketDateSuffix = mTkt.Groups["suffix"].Value.Trim();
                var after = trimmed.Substring(mTkt.Index + mTkt.Length).Trim();
                // Strip leading dash that is part of the suffix token
                after = after.TrimStart('-').Trim();
                if (!string.IsNullOrEmpty(after) &&
                    !after.Equals(result.LastTicketDateSuffix, StringComparison.OrdinalIgnoreCase))
                    result.AdvisoryMessages.Add(after);
                continue;
            }

            // Section markers / column header
            if (Regex.IsMatch(trimmed, @"^-{5,}$"))
                continue;

            if (string.Equals(trimmed, "PRIVATE", StringComparison.OrdinalIgnoreCase))
            {
                result.FareType = "PRIVATE";
                continue;
            }

            if (trimmed.StartsWith("AL FLGT", StringComparison.OrdinalIgnoreCase))
            {
                phase = DetailPhase.Segments;
                continue;
            }

            // Page indicator
            var mPg = RxPage.Match(trimmed);
            if (mPg.Success)
            {
                result.PageIndicator = mPg.Groups["page"].Value.Trim();
                continue;
            }

            // Exchange-rate line: "RATE USED 1JPY=0.0062981USD"
            var mRate = RxExchangeRate.Match(trimmed);
            if (mRate.Success)
            {
                result.ExchangeRate = mRate.Groups["rate"].Value;
                continue;
            }

            // Baggage advisory
            if (trimmed.StartsWith("NO BAG", StringComparison.OrdinalIgnoreCase) ||
                (trimmed.StartsWith("BAG", StringComparison.OrdinalIgnoreCase) &&
                 phase == DetailPhase.Pricing))
            {
                result.BaggageInfo = trimmed;
                continue;
            }

            // ── Monetary line: CUR amount [-CODE] [rest] ─────────────────────

            var mMoney = RxLeftMoney.Match(trimmed);
            if (mMoney.Success)
            {
                phase    = DetailPhase.Pricing;
                var cur  = mMoney.Groups["cur"].Value.ToUpper();
                var amt  = Dec(mMoney.Groups["amt"].Value);
                var code = mMoney.Groups["code"].Value.ToUpper();
                var rest = mMoney.Groups["rest"].Value.Trim();
                lastCur  = cur;

                if (string.IsNullOrEmpty(code))
                {
                    // Base fare (first) or grand total (last)
                    var money = new MoneyAmount { Currency = cur, Amount = amt };
                    if (!baseFareSet)
                    {
                        result.BaseFare = money;
                        baseFareSet     = true;
                        if (!fareCalcDone && !string.IsNullOrEmpty(rest))
                            fareCalcParts.Add(rest);
                    }
                    else
                    {
                        result.TotalAmount = money;
                    }
                    ExtractTaxes(rest, cur, result, fareCalcParts, ref fareCalcDone);
                }
                else
                {
                    var tax = new TaxItem
                    {
                        Code   = code,
                        Amount = new MoneyAmount { Currency = cur, Amount = amt }
                    };
                    if (code == "XF")
                        tax.AirportBreakdown = ExtractXfBreakdown(rest);

                    result.Taxes.Add(tax);
                    ExtractTaxes(rest, cur, result, fareCalcParts, ref fareCalcDone);
                }
                continue;
            }

            // ── Pricing continuation (not a monetary anchor line) ─────────────

            if (phase == DetailPhase.Pricing)
            {
                ExtractTaxes(trimmed, lastCur, result, fareCalcParts, ref fareCalcDone);
                continue;
            }

            // ── Segment detection ─────────────────────────────────────────────

            // Standalone city (FXP 2-col initial origin)
            var mCity = RxStandaloneCity.Match(trimmed);
            if (mCity.Success)
            {
                lastDest = trimmed.ToUpper();
                phase    = DetailPhase.Segments;
                continue;
            }

            FlightSegment seg;
            if (TryParseSegmentWithDest(trimmed, lastDest, out seg))
            {
                result.Segments.Add(seg);
                lastDest = seg.Destination;
                phase    = DetailPhase.Segments;
                continue;
            }

            FlightSegment segAl;
            if (TryParseSegmentAirlineFirst(trimmed, lastDest, out segAl))
            {
                result.Segments.Add(segAl);
                if (!string.IsNullOrEmpty(segAl.Destination))
                    lastDest = segAl.Destination;
                phase = DetailPhase.Segments;
                continue;
            }

            result.UnparsedLines.Add(trimmed);
        }

        if (fareCalcParts.Count > 0)
            result.FareCalculation = string.Join(" ", fareCalcParts);

        return result;
    }

    // ── Tax extraction helper ─────────────────────────────────────────────────

    private void ExtractTaxes(string text, string inheritedCur, FareQuoteData result,
                               List<string> fareCalcParts, ref bool fareCalcDone)
    {
        if (string.IsNullOrEmpty(text)) return;

        if (!fareCalcDone && RxBaseFareConfirm.IsMatch(text))
            fareCalcDone = true;

        if (!fareCalcDone)
            fareCalcParts.Add(text);

        // Inline taxes: "USD 11.20-AY"
        foreach (Match m in RxInlineTax.Matches(text))
        {
            var code = m.Groups["code"].Value.ToUpper();
            var cur  = m.Groups["cur"].Value.ToUpper();
            var amt  = Dec(m.Groups["amt"].Value);

            if (IsDuplicate(result.Taxes, code, amt)) continue;

            var tax = new TaxItem
            {
                Code   = code,
                Amount = new MoneyAmount { Currency = cur, Amount = amt }
            };
            if (code == "XF")
                tax.AirportBreakdown = ExtractXfBreakdown(text.Substring(m.Index + m.Length));

            result.Taxes.Add(tax);
        }

        // No-prefix taxes: "4.80-IJ" (inherits last known currency)
        // NOTE: taxes split across two lines (e.g. "6.30" on one line, "-TK" on the next)
        // cannot be reconstructed by this single-line pass and will be missed.
        foreach (Match m in RxNoCurTax.Matches(text))
        {
            int start     = m.Index;
            bool hasPrefix = start >= 3 && char.IsLetter(text[start - 1]);
            if (hasPrefix) continue;

            var code = m.Groups["code"].Value.ToUpper();
            var amt  = Dec(m.Groups["amt"].Value);

            if (IsDuplicate(result.Taxes, code, amt)) continue;

            var tax = new TaxItem
            {
                Code   = code,
                Amount = new MoneyAmount { Currency = inheritedCur, Amount = amt }
            };
            if (code == "XF")
                tax.AirportBreakdown = ExtractXfBreakdown(text.Substring(m.Index + m.Length));

            result.Taxes.Add(tax);
        }
    }

    private static bool IsDuplicate(List<TaxItem> taxes, string code, decimal amount)
    {
        return taxes.Any(t => t.Code == code && t.Amount.Amount == amount);
    }

    private static string ExtractXfBreakdown(string text)
    {
        var parts = RxXfBreakdown.Matches(text);
        if (parts.Count == 0) return string.Empty;

        var sb = new System.Text.StringBuilder();
        foreach (Match m in parts)
            sb.Append(m.Value);
        return sb.ToString();
    }

    // ── Segment parsers ───────────────────────────────────────────────────────

    private static bool TryParseSegmentWithDest(string line, string implicitOrigin,
                                                 out FlightSegment segment)
    {
        segment = null;
        var m = RxSegmentWithDest.Match(line);
        if (!m.Success) return false;

        bool hasOrig = m.Groups["orig"].Success;
        bool xFlag   = m.Groups["xflag"].Success;
        bool hasFin  = m.Groups["fin"].Success;

        segment = new FlightSegment
        {
            Origin               = hasOrig ? m.Groups["orig"].Value.ToUpper() : implicitOrigin,
            Destination          = m.Groups["dest"].Value.ToUpper(),
            IsTransitDestination = xFlag,
            Airline              = m.Groups["al"].Value.ToUpper(),
            FlightNumber         = int.Parse(m.Groups["flt"].Value),
            BookingClass         = m.Groups["cls"].Value.ToUpper(),
            Date                 = m.Groups["date"].Value.ToUpper(),
            DepartureTime        = m.Groups["time"].Value,
            FareBasis            = m.Groups["fb"].Value.ToUpper(),
            NotValidBefore       = m.Groups["nvb"].Success ? m.Groups["nvb"].Value.ToUpper() : string.Empty,
            NotValidAfter        = m.Groups["nva"].Success ? m.Groups["nva"].Value.ToUpper() : string.Empty,
            BaggageAllowance     = m.Groups["bg"].Value.ToUpper()
        };

        if (hasFin)
        {
            var fin  = m.Groups["fin"].Value.ToUpper();
            bool finX = fin.StartsWith("X");
            if (finX)
            {
                segment.Destination          = fin.Substring(1);
                segment.IsTransitDestination = true;
            }
        }
        return true;
    }

    private static bool TryParseSegmentAirlineFirst(string line, string implicitOrigin,
                                                      out FlightSegment segment)
    {
        segment = null;
        var m = RxSegmentAirlineFirst.Match(line);
        if (!m.Success) return false;

        bool hasFin = m.Groups["fin"].Success;
        bool xFin   = m.Groups["xfin"].Success;

        segment = new FlightSegment
        {
            Origin               = implicitOrigin,
            Destination          = hasFin ? m.Groups["fin"].Value.ToUpper() : string.Empty,
            IsTransitDestination = xFin,
            Airline              = m.Groups["al"].Value.ToUpper(),
            FlightNumber         = int.Parse(m.Groups["flt"].Value),
            BookingClass         = m.Groups["cls"].Value.ToUpper(),
            Date                 = m.Groups["date"].Value.ToUpper(),
            DepartureTime        = m.Groups["time"].Value,
            FareBasis            = m.Groups["fb"].Value.ToUpper(),
            NotValidBefore       = m.Groups["nvb"].Success ? m.Groups["nvb"].Value.ToUpper() : string.Empty,
            NotValidAfter        = m.Groups["nva"].Success ? m.Groups["nva"].Value.ToUpper() : string.Empty,
            BaggageAllowance     = m.Groups["bg"].Value.ToUpper()
        };
        return true;
    }

    // ── Detail header parser ──────────────────────────────────────────────────
    //
    //  Supported header formats:
    //    FXP/R,U/FF-MAINFL 01 GOR/ANNA …   (FF-profile, inline pax name)
    //    FXP/R,U/FF-MAINFL                  (FF-profile, pax name on next line)
    //    FXX/R/A4,5,6,7-TLBJP/P3           (A-selector, dash-profile, /Pn suffix)
    // ─────────────────────────────────────────────────────────────────────────

    private static void ParseDetailHeader(string line, FareQuoteData result)
    {
        // FF- style:  "/FF-MAINFL"
        var mFf = Regex.Match(line, @"/FF-(?<ff>[A-Z0-9 ]+?)(?:\s{2,}|\s+\d{2}\s|$)",
            RegexOptions.IgnoreCase);
        if (mFf.Success)
        {
            result.FrequentFlyerProfile = mFf.Groups["ff"].Value.Trim();
        }
        else
        {
            // A-selector style:  "FXX/R/A4,5,6,7-PROFILE/P3"
            var mA = Regex.Match(line,
                @"FX[XP]/R/A[\d,]+-(?<ff>[A-Z0-9]+)(?:/P\d+)?",
                RegexOptions.IgnoreCase);
            if (mA.Success)
                result.FrequentFlyerProfile = mA.Groups["ff"].Value.Trim();
        }

        // Inline passenger name: "GOR/ANNA"
        var mPax = Regex.Match(line, @"\b([A-Z]{2,}/[A-Z]{2,})\b");
        if (mPax.Success)
            result.PassengerName = mPax.Groups[1].Value;

        if (line.IndexOf("ADV PURCHASE", StringComparison.OrdinalIgnoreCase) >= 0)
            result.AdvisoryMessages.Add("SEE ADV PURCHASE");
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Summary parser
    // ─────────────────────────────────────────────────────────────────────────

    private FareQuoteSummaryData ParseSummary(List<string> lines)
    {
        var result = new FareQuoteSummaryData();

        foreach (var line in lines)
        {
            var trimmedLine = line.TrimStart();

            if (trimmedLine.StartsWith("FXP", StringComparison.OrdinalIgnoreCase) ||
                trimmedLine.StartsWith("FXX", StringComparison.OrdinalIgnoreCase))
            {
                var m = RxSummaryHeader.Match(trimmedLine);
                if (m.Success) result.FrequentFlyerProfile = m.Groups["ff"].Value.Trim();
                continue;
            }

            if (RxSummaryColHeader.IsMatch(line))
            {
                var mc = RxFareCurrency.Match(line);
                if (mc.Success) result.FareCurrency = mc.Groups["cur"].Value.ToUpper();
                continue;
            }

            var mTot = RxTotalsRow.Match(line);
            if (mTot.Success)
            {
                result.Totals = new FareSummaryTotals
                {
                    TotalPassengers = int.Parse(mTot.Groups["np"].Value),
                    TotalFare       = Dec(mTot.Groups["fare"].Value),
                    TotalTaxFee     = Dec(mTot.Groups["tax"].Value),
                    GrandTotal      = Dec(mTot.Groups["total"].Value)
                };
                continue;
            }

                // ── 4. Passenger row ──────────────────────────────────────────────
                var mPax = RxPassengerRow.Match(trimmedLine);
                if (mPax.Success)
                {
                    result.Passengers.Add(new PassengerFareSummary
                    {
                        SequenceNumber = int.Parse(mPax.Groups["seq"].Value),
                        Name = mPax.Groups["name"].Value.TrimEnd('*').ToUpper(),
                        PassengerTypeCode = mPax.Groups["ptc"].Value.ToUpper(),
                        NumberOfPassengers = int.Parse(mPax.Groups["np"].Value),
                        BaseFare = Dec(mPax.Groups["fare"].Value),
                        TaxFeeAmount = Dec(mPax.Groups["tax"].Value),
                        Total = Dec(mPax.Groups["total"].Value)
                    });
                    continue;
                }

            var mUp = RxUpsell.Match(line);
            if (mUp.Success)
            {
                result.Upsell = new UpsellOffer
                {
                    CurrentProduct = mUp.Groups["from"].Value.ToUpper(),
                    UpsellProduct  = mUp.Groups["to"].Value.ToUpper(),
                    Amount = new MoneyAmount
                    {
                        Currency = mUp.Groups["cur"].Value.ToUpper(),
                        Amount   = Dec(mUp.Groups["amt"].Value)
                    }
                };
                continue;
            }

            var mFf = RxFareFamilies.Match(line);
            if (mFf.Success)
            {
                if (result.Upsell != null)
                    result.Upsell.FareFamilies = mFf.Groups["families"].Value.Trim();
                else
                    result.Notices.Add(new FareNotice
                    {
                        PassengerRange = mFf.Groups["range"].Value,
                        Message        = "FARE FAMILIES:" + mFf.Groups["families"].Value.Trim()
                    });
                continue;
            }

            var mPg = RxPage.Match(line);
            if (mPg.Success) { result.PageIndicator = mPg.Groups["page"].Value.Trim(); continue; }

            var mNotice = RxRangedNotice.Match(trimmedLine);
            if (mNotice.Success)
            {
                var notice = new FareNotice
                {
                    PassengerRange = mNotice.Groups["range"].Value,
                    Message        = mNotice.Groups["msg"].Value.Trim()
                };
                var mTkt = RxLastTktDate.Match(notice.Message);
                if (mTkt.Success) notice.LastTicketDate = mTkt.Groups["date"].Value;
                result.Notices.Add(notice);
                continue;
            }

            result.UnparsedLines.Add(line.Trim());
        }

        return result;
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Utilities
    // ─────────────────────────────────────────────────────────────────────────

    private static decimal Dec(string s)
    {
        return decimal.Parse(s, CultureInfo.InvariantCulture);
    }

        private static List<string> NormaliseLines(string raw)
        {
            return raw.Replace("\r\n", "\n")
                      .Replace("\r", "\n")
                      .Split('\n')
                      .Select(l => l.TrimEnd())
                      .Where(l => l.Trim().Length > 0)
                      .ToList();
        }
    }

}
