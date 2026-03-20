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
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1:F2}", Currency, Amount);
        }
    }

    public class TaxItem
    {
        public string Code { get; set; } = string.Empty;
        public MoneyAmount Amount { get; set; } = new MoneyAmount();
        /// <summary>Airport fee breakdown when code is XF, e.g. "DCA4.50CLT4.50".</summary>
        public string AirportBreakdown { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.IsNullOrEmpty(AirportBreakdown)
                ? string.Format("{0} -{1}", Amount, Code)
                : string.Format("{0} -{1} {2}", Amount, Code, AirportBreakdown);
        }
    }

    public class FlightSegment
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public bool IsTransitDestination { get; set; }
        public string Airline { get; set; } = string.Empty;
        public int FlightNumber { get; set; }
        public string BookingClass { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string FareBasis { get; set; } = string.Empty;
        public string NotValidBefore { get; set; } = string.Empty;
        public string BaggageAllowance { get; set; } = string.Empty;
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
        public string PassengerName { get; set; } = string.Empty;
        public string FrequentFlyerProfile { get; set; } = string.Empty;
        public string LastTicketDate { get; set; } = string.Empty;
        public string FareType { get; set; } = string.Empty;
        public List<string> AdvisoryMessages { get; private set; } = new List<string>();
        public List<FlightSegment> Segments { get; private set; } = new List<FlightSegment>();
        /// <summary>Raw fare-calculation string (text between base fare and END marker).</summary>
        public string FareCalculation { get; set; } = string.Empty;
        /// <summary>First left-column USD amount after segments (no tax code).</summary>
        public MoneyAmount BaseFare { get; set; } = new MoneyAmount();
        public List<TaxItem> Taxes { get; private set; } = new List<TaxItem>();
        /// <summary>Last left-column USD amount after segments (no tax code).</summary>
        public MoneyAmount TotalAmount { get; set; } = new MoneyAmount();
        public string BaggageInfo { get; set; } = string.Empty;
        public string PageIndicator { get; set; } = string.Empty;
        public List<string> UnparsedLines { get; private set; } = new List<string>();
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
        public int TotalPassengers { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalTaxFee { get; set; }
        public decimal GrandTotal { get; set; }

        public override string ToString()
        {
            return string.Format("Pax:{0}  Fare:{1:F2}  Tax:{2:F2}  Total:{3:F2}",
                TotalPassengers, TotalFare, TotalTaxFee, GrandTotal);
        }
    }

    public class UpsellOffer
    {
        public string CurrentProduct { get; set; } = string.Empty;
        public string UpsellProduct { get; set; } = string.Empty;
        public MoneyAmount Amount { get; set; } = new MoneyAmount();
        public string FareFamilies { get; set; } = string.Empty;
    }

    public class FareNotice
    {
        public string PassengerRange { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        /// <summary>Null when this notice is not a LAST TKT DTE line.</summary>
        public string LastTicketDate { get; set; }   // null = not a tkt-date notice
    }

    public class FareQuoteSummaryData
    {
        public string FrequentFlyerProfile { get; set; } = string.Empty;
        public string FareCurrency { get; set; } = string.Empty;
        public List<PassengerFareSummary> Passengers { get; private set; } = new List<PassengerFareSummary>();
        public FareSummaryTotals Totals { get; set; }   // null when not parsed
        public UpsellOffer Upsell { get; set; }   // null when not present
        public List<FareNotice> Notices { get; private set; } = new List<FareNotice>();
        public string PageIndicator { get; set; } = string.Empty;
        public List<string> UnparsedLines { get; private set; } = new List<string>();

        /// <summary>
        /// Returns the last-ticket date from the first LAST TKT DTE notice,
        /// or an empty string when not present.
        /// </summary>
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

    /// <summary>
    /// Returned by <see cref="FareQuoteParser.Parse"/>. Check <see cref="Format"/>
    /// to determine which property to use.
    /// </summary>
    public class FareQuoteResult
    {
        public FareQuoteFormat Format { get; set; }
        /// <summary>Populated when Format is Detail; null otherwise.</summary>
        public FareQuoteData Detail { get; set; }
        /// <summary>Populated when Format is Summary; null otherwise.</summary>
        public FareQuoteSummaryData Summary { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════════
    //  Parser
    // ══════════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Parses both variants of the GDS FXP/R pricing response:
    ///   Detail  – itinerary + fare calculation + taxes (single pricing unit)
    ///   Summary – multi-passenger table with per-pax and aggregate totals
    ///
    /// Call Parse() and inspect FareQuoteResult.Format to decide which
    /// strongly-typed result to use.
    /// </summary>
    public class FareQuoteParser
    {
        // ── Shared ────────────────────────────────────────────────────────────────

        private static readonly Regex RxLastTktDate = new Regex(
            @"LAST\s+TKT\s+DTE\s+(?<date>\d{1,2}[A-Z]{3}\d{2}/\d{2}:\d{2})",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // ── Detail: segment regexes ───────────────────────────────────────────────

        // Segment line that carries an explicit destination (optionally preceded by origin).
        // Handles:
        //   FXP inline  "WAS XCLT AA 1581 Q 27JUL 0618 QNU8NBM5/SHRF  0P SLU"
        //   FXP 2-col   "XCLT AA  1581 Q    27JUL 0618  QNU8NBM5/SHRF   0P"
        //               " SLU AA   823 Q    27JUL 0930  QNU8NBM5/SHRF   0P"
        private static readonly Regex RxSegmentWithDest = new Regex(
            @"^(?:(?<orig>[A-Z]{3})\s+)?(?<xflag>X)?(?<dest>[A-Z]{3})\s+" +
            @"(?<al>[A-Z]{2})\s+(?<flt>\d{1,4})\s+" +
            @"(?<cls>[A-Z])\s+(?<date>\d{1,2}[A-Z]{3})\s+(?<time>\d{4})\s+" +
            @"(?<fb>[A-Z0-9]+(?:/[A-Z0-9]+)?)\s+" +
            @"(?:(?<nvb>\d{1,2}[A-Z]{3})\s+)?" +
            @"(?<bg>\d+[KPF]|0P)\s*(?<fin>X?[A-Z]{3})?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Airline-first segment: FXP inline continuation lines that omit origin+dest prefix.
        //   "AA 823 Q 27JUL 0930 QNU8NBM5/SHRF  0P XMIA"
        // Destination is inferred from the trailing fin field.
        private static readonly Regex RxSegmentAirlineFirst = new Regex(
            @"^(?<al>[A-Z]{2})\s+(?<flt>\d{1,4})\s+" +
            @"(?<cls>[A-Z])\s+(?<date>\d{1,2}[A-Z]{3})\s+(?<time>\d{4})\s+" +
            @"(?<fb>[A-Z0-9]+(?:/[A-Z0-9]+)?)\s+" +
            @"(?:(?<nvb>\d{1,2}[A-Z]{3})\s+)?" +
            @"(?<bg>\d+[KPF]|0P)\s*(?<xfin>X)?(?<fin>[A-Z]{3})?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Standalone 3-letter city code on its own line (FXP 2-col initial-origin).
        private static readonly Regex RxStandaloneCity = new Regex(
            @"^([A-Z]{3})$",
            RegexOptions.Compiled);

        // ── Detail: monetary / tax regexes ────────────────────────────────────────

        // Left-column monetary anchor.
        // Matches lines whose meaningful content starts with:  CUR  amount  [-CODE]  [rest]
        //   FXP inline: "USD 813.00 27JUL26WAS AA ..."
        //   FXP 2-col:  "USD   813.00      27JUL26WAS AA ..."
        //               "USD    11.20-AY   USD863.00END"
        //               "USD  1000.93      4.80-IJ USD 35.00..."
        private static readonly Regex RxLeftMoney = new Regex(
            @"^\s*(?<cur>[A-Z]{3})\s+(?<amt>\d+\.\d+)(?:\s*-(?<code>[A-Z0-9]{2,3}))?(?:\s+(?<rest>.+))?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Inline tax anywhere in a string:  "USD 11.20-AY"  or  "USD11.20-AY"
        private static readonly Regex RxInlineTax = new Regex(
            @"(?<cur>[A-Z]{3})\s*(?<amt>\d+\.\d+)-(?<code>[A-Z0-9]{2,3})",
            RegexOptions.Compiled);

        // No-currency tax: "4.80-IJ" — inherits currency from last seen CUR.
        private static readonly Regex RxNoCurTax = new Regex(
            @"(?<![.\d/])(?<amt>\d+\.\d+)-(?<code>[A-Z]{2,3})\b",
            RegexOptions.Compiled);

        // Base-fare confirmation token: "USD813.00END" or "USD 813.00END"
        private static readonly Regex RxBaseFareConfirm = new Regex(
            @"(?<cur>[A-Z]{3})\s*(?<amt>\d+\.\d+)END",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Airport XF breakdown: "DCA4.50CLT4.50MIA4.50"
        private static readonly Regex RxXfBreakdown = new Regex(
            @"[A-Z]{3}\d+\.\d+",
            RegexOptions.Compiled);

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

        /// <summary>
        /// Auto-detects the FXP/R format and returns a FareQuoteResult.
        /// </summary>
        public FareQuoteResult Parse(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                throw new ArgumentNullException("raw");

            var lines = NormaliseLines(raw);

            if (IsSummaryFormat(lines))
                return new FareQuoteResult { Format = FareQuoteFormat.Summary, Summary = ParseSummary(lines) };
            else
                return new FareQuoteResult { Format = FareQuoteFormat.Detail, Detail = ParseDetail(lines) };
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  Format detection
        // ─────────────────────────────────────────────────────────────────────────

        private static bool IsSummaryFormat(List<string> lines)
        {
            return lines.Any(l => RxSummaryColHeader.IsMatch(l));
        }

        // ─────────────────────────────────────────────────────────────────────────
        //  Detail parser  —  handles both FXP inline and FXP two-column layouts
        // ─────────────────────────────────────────────────────────────────────────
        //
        //  Layout comparison
        //  ─────────────────
        //  FXP inline:
        //    USD 813.00 27JUL26WAS AA X/CLT AA SLU374.00QNU8NBM5      ← base fare + fare calc
        //    /SHRF … USD 11.20-AY USD813.00END                         ← continuation + tax + END
        //    USD 23.40-US XT USD 23.40-US                              ← tax lines
        //    USD 1000.93                                                ← TOTAL (standalone)
        //    4.80-IJ USD 35.00-J1 … USD 13.50-XF DCA4.50CLT4.50MIA4.50
        //
        //  FXP two-column (left col = primary value, right col = continuation/extra taxes):
        //    USD   813.00      27JUL26WAS AA X/CLT AA SLU374.00QNU8NBM5  ← base fare | fare calc
        //                      /SHRF … AA WAS439.00NNV2NBM5/SHRF          ← right-col continuation
        //    USD    11.20-AY   USD813.00END                               ← tax AY | END confirm
        //    USD    23.40-US   XT USD 23.40-US USD 3.84-XA USD 7.00-XY   ← tax US | extra taxes
        //    USD  1000.93      4.80-IJ USD 35.00-J1 … USD                 ← TOTAL | extra taxes
        //                      13.50-XF DCA4.50CLT4.50MIA4.50             ← continuation
        //
        //  Key rules:
        //    • Base fare  = FIRST  left-column CUR amount with NO tax code after segments
        //    • Total      = LAST   left-column CUR amount with NO tax code (keeps updating)
        //    • Taxes      = ALL    CUR amount-CODE occurrences anywhere in pricing lines
        //    • No-cur tax = bare "4.80-IJ" — inherits the last seen currency
        // ─────────────────────────────────────────────────────────────────────────

        private enum DetailPhase { Header, Segments, Pricing }

        private FareQuoteData ParseDetail(List<string> lines)
        {
            var result = new FareQuoteData();
            var phase = DetailPhase.Header;
            var lastDest = string.Empty;   // implicit origin for next segment
            var lastCur = "USD";          // currency context for no-prefix taxes
            var baseFareSet = false;

            // Fare-calc accumulator: collects right-column / continuation text
            // that runs from the base-fare line up to the END marker.
            var fareCalcParts = new List<string>();
            var fareCalcDone = false;

            foreach (var rawLine in lines)
            {
                var trimmed = rawLine.Trim();
                if (trimmed.Length == 0) continue;

                // ── Always-active parsers ─────────────────────────────────────────

                // FXP/R or FXX/R header
                if (trimmed.StartsWith("FXP", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("FXX", StringComparison.OrdinalIgnoreCase))
                {
                    ParseDetailHeader(trimmed, result);
                    continue;
                }

                // FXP two-column passenger line: "01 GOR/ANNA"
                var mPaxSeq = Regex.Match(trimmed,
                    @"^\d{2}\s+(?<n>[A-Z]+/[A-Z]+)\s*$", RegexOptions.IgnoreCase);
                if (mPaxSeq.Success)
                {
                    result.PassengerName = mPaxSeq.Groups["n"].Value.ToUpper();
                    continue;
                }

                // Last ticket date
                var mTkt = RxLastTktDate.Match(trimmed);
                if (mTkt.Success)
                {
                    result.LastTicketDate = mTkt.Groups["date"].Value;
                    var after = trimmed.Substring(mTkt.Index + mTkt.Length).Trim();
                    if (!string.IsNullOrEmpty(after))
                        result.AdvisoryMessages.Add(after);
                    continue;
                }

                // Section markers and column headers
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

                // Baggage advisory
                if (trimmed.StartsWith("NO BAG", StringComparison.OrdinalIgnoreCase) ||
                    (trimmed.StartsWith("BAG", StringComparison.OrdinalIgnoreCase) &&
                     phase == DetailPhase.Pricing))
                {
                    result.BaggageInfo = trimmed;
                    continue;
                }

                // ── Monetary line?  (left-column CUR amount [-CODE]) ─────────────
                //
                // This is the primary entry point into the pricing section.

                var mMoney = RxLeftMoney.Match(trimmed);
                if (mMoney.Success)
                {
                    phase = DetailPhase.Pricing;
                    var cur = mMoney.Groups["cur"].Value.ToUpper();
                    var amt = Dec(mMoney.Groups["amt"].Value);
                    var code = mMoney.Groups["code"].Value.ToUpper();
                    var rest = mMoney.Groups["rest"].Value.Trim();
                    lastCur = cur;

                    if (string.IsNullOrEmpty(code))
                    {
                        // ── Base fare (first) or Total (any subsequent — last wins) ──
                        var money = new MoneyAmount { Currency = cur, Amount = amt };
                        if (!baseFareSet)
                        {
                            result.BaseFare = money;
                            baseFareSet = true;
                            // Right column / rest = start of fare calculation text
                            if (!fareCalcDone && !string.IsNullOrEmpty(rest))
                                fareCalcParts.Add(rest);
                        }
                        else
                        {
                            result.TotalAmount = money;
                        }

                        // Right column may still contain taxes (e.g. FXP 2-col total line)
                        ExtractTaxes(rest, cur, result, fareCalcParts, ref fareCalcDone);
                    }
                    else
                    {
                        // ── Primary tax on this line ──────────────────────────────
                        var tax = new TaxItem
                        {
                            Code = code,
                            Amount = new MoneyAmount { Currency = cur, Amount = amt }
                        };
                        if (code == "XF")
                            tax.AirportBreakdown = ExtractXfBreakdown(rest);

                        result.Taxes.Add(tax);

                        // Right column may contain more inline taxes and the END token
                        ExtractTaxes(rest, cur, result, fareCalcParts, ref fareCalcDone);
                    }
                    continue;
                }

                // ── Not a monetary line ──────────────────────────────────────────

                if (phase == DetailPhase.Pricing)
                {
                    // Could be:
                    //   (a) Fare-calc continuation:  "/SHRF AA X/MIA AA WAS489.00…"
                    //   (b) No-currency tax:         "4.80-IJ USD 35.00-J1 …"
                    //   (c) FXP 2-col indented right-column continuation
                    ExtractTaxes(trimmed, lastCur, result, fareCalcParts, ref fareCalcDone);
                    continue;
                }

                // ── Segment detection (header / segments phase) ──────────────────

                // FXP 2-col standalone city: sets the initial departure city
                var mCity = RxStandaloneCity.Match(trimmed);
                if (mCity.Success)
                {
                    lastDest = trimmed.ToUpper();
                    phase = DetailPhase.Segments;
                    continue;
                }

                // Segment with explicit destination (FXP inline and FXP 2-col)
                FlightSegment seg;
                if (TryParseSegmentWithDest(trimmed, lastDest, out seg))
                {
                    result.Segments.Add(seg);
                    lastDest = seg.Destination;
                    phase = DetailPhase.Segments;
                    continue;
                }

                // Airline-first segment (FXP inline continuation: "AA 823 Q 27JUL…")
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
        //
        // Scans an arbitrary text fragment for:
        //   1. Inline taxes:    "USD 11.20-AY"  →  TaxItem(AY, USD, 11.20)
        //   2. No-prefix taxes: "4.80-IJ"        →  TaxItem(IJ, inheritedCur, 4.80)
        //   3. END marker:      "USD813.00END"   →  closes fare-calc accumulation
        //
        // Already-recorded taxes (same code+amount) are de-duplicated.
        // ─────────────────────────────────────────────────────────────────────────

        private void ExtractTaxes(string text, string inheritedCur, FareQuoteData result,
                                   List<string> fareCalcParts, ref bool fareCalcDone)
        {
            if (string.IsNullOrEmpty(text)) return;

            // END marker → close fare calc
            if (!fareCalcDone && RxBaseFareConfirm.IsMatch(text))
                fareCalcDone = true;

            // Fare-calc accumulation (before END is seen)
            if (!fareCalcDone)
                fareCalcParts.Add(text);

            // Inline taxes: "USD 11.20-AY"
            foreach (Match m in RxInlineTax.Matches(text))
            {
                var code = m.Groups["code"].Value.ToUpper();
                var cur = m.Groups["cur"].Value.ToUpper();
                var amt = Dec(m.Groups["amt"].Value);

                if (IsDuplicate(result.Taxes, code, amt)) continue;

                var tax = new TaxItem
                {
                    Code = code,
                    Amount = new MoneyAmount { Currency = cur, Amount = amt }
                };
                if (code == "XF")
                    tax.AirportBreakdown = ExtractXfBreakdown(text.Substring(m.Index + m.Length));

                result.Taxes.Add(tax);
            }

            // No-prefix taxes: "4.80-IJ"
            // Skip matches that were already captured by the inline tax scan
            // (those have a 3-letter uppercase prefix immediately before them).
            foreach (Match m in RxNoCurTax.Matches(text))
            {
                int start = m.Index;
                bool hasPrefix = start >= 3 && char.IsLetter(text[start - 1]);
                if (hasPrefix) continue;

                var code = m.Groups["code"].Value.ToUpper();
                var amt = Dec(m.Groups["amt"].Value);

                if (IsDuplicate(result.Taxes, code, amt)) continue;

                var tax = new TaxItem
                {
                    Code = code,
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
            bool xFlag = m.Groups["xflag"].Success;
            bool hasFin = m.Groups["fin"].Success;

            segment = new FlightSegment
            {
                Origin = hasOrig ? m.Groups["orig"].Value.ToUpper() : implicitOrigin,
                Destination = m.Groups["dest"].Value.ToUpper(),
                IsTransitDestination = xFlag,
                Airline = m.Groups["al"].Value.ToUpper(),
                FlightNumber = int.Parse(m.Groups["flt"].Value),
                BookingClass = m.Groups["cls"].Value.ToUpper(),
                Date = m.Groups["date"].Value.ToUpper(),
                DepartureTime = m.Groups["time"].Value,
                FareBasis = m.Groups["fb"].Value.ToUpper(),
                NotValidBefore = m.Groups["nvb"].Success
                                           ? m.Groups["nvb"].Value.ToUpper()
                                           : string.Empty,
                BaggageAllowance = m.Groups["bg"].Value.ToUpper()
            };

            // If there is a trailing X?city token (fin), handle transit flag propagation.
            if (hasFin)
            {
                var fin = m.Groups["fin"].Value.ToUpper();
                var finX = fin.StartsWith("X");
                if (finX)
                {
                    // e.g. "0P XMIA" → the stated destination is the transit point
                    segment.Destination = fin.Substring(1);
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
            bool xFin = m.Groups["xfin"].Success;

            segment = new FlightSegment
            {
                Origin = implicitOrigin,
                Destination = hasFin ? m.Groups["fin"].Value.ToUpper() : string.Empty,
                IsTransitDestination = xFin,
                Airline = m.Groups["al"].Value.ToUpper(),
                FlightNumber = int.Parse(m.Groups["flt"].Value),
                BookingClass = m.Groups["cls"].Value.ToUpper(),
                Date = m.Groups["date"].Value.ToUpper(),
                DepartureTime = m.Groups["time"].Value,
                FareBasis = m.Groups["fb"].Value.ToUpper(),
                NotValidBefore = m.Groups["nvb"].Success
                                           ? m.Groups["nvb"].Value.ToUpper()
                                           : string.Empty,
                BaggageAllowance = m.Groups["bg"].Value.ToUpper()
            };
            return true;
        }

        // ── Detail header parser ──────────────────────────────────────────────────

        private static void ParseDetailHeader(string line, FareQuoteData result)
        {
            // Passenger name embedded in header (FXP inline):  GOR/ANNA
            var mPax = Regex.Match(line, @"\b([A-Z]{2,}/[A-Z]{2,})\b");
            if (mPax.Success)
                result.PassengerName = mPax.Groups[1].Value;

            // FF / corporate profile
            var mFf = Regex.Match(line, @"FF-(?<ff>[A-Z0-9 ]+?)(?:\s{2,}|$)");
            if (mFf.Success)
                result.FrequentFlyerProfile = mFf.Groups["ff"].Value.Trim();

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

                // ── 1. FXP header ─────────────────────────────────────────────────
                if (trimmedLine.StartsWith("FXP", StringComparison.OrdinalIgnoreCase) ||
                    trimmedLine.StartsWith("FXX", StringComparison.OrdinalIgnoreCase))
                {
                    var m = RxSummaryHeader.Match(trimmedLine);
                    if (m.Success)
                        result.FrequentFlyerProfile = m.Groups["ff"].Value.Trim();
                    continue;
                }

                // ── 2. Column header – capture fare currency ───────────────────────
                if (RxSummaryColHeader.IsMatch(line))
                {
                    var mc = RxFareCurrency.Match(line);
                    if (mc.Success)
                        result.FareCurrency = mc.Groups["cur"].Value.ToUpper();
                    continue;
                }

                // ── 3. TOTALS row ─────────────────────────────────────────────────
                var mTot = RxTotalsRow.Match(line);
                if (mTot.Success)
                {
                    result.Totals = new FareSummaryTotals
                    {
                        TotalPassengers = int.Parse(mTot.Groups["np"].Value),
                        TotalFare = Dec(mTot.Groups["fare"].Value),
                        TotalTaxFee = Dec(mTot.Groups["tax"].Value),
                        GrandTotal = Dec(mTot.Groups["total"].Value)
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

                // ── 5. Upsell offer ───────────────────────────────────────────────
                var mUp = RxUpsell.Match(line);
                if (mUp.Success)
                {
                    result.Upsell = new UpsellOffer
                    {
                        CurrentProduct = mUp.Groups["from"].Value.ToUpper(),
                        UpsellProduct = mUp.Groups["to"].Value.ToUpper(),
                        Amount = new MoneyAmount
                        {
                            Currency = mUp.Groups["cur"].Value.ToUpper(),
                            Amount = Dec(mUp.Groups["amt"].Value)
                        }
                    };
                    continue;
                }

                // ── 6. Fare families line ─────────────────────────────────────────
                var mFf = RxFareFamilies.Match(line);
                if (mFf.Success)
                {
                    if (result.Upsell != null)
                        result.Upsell.FareFamilies = mFf.Groups["families"].Value.Trim();
                    else
                        result.Notices.Add(new FareNotice
                        {
                            PassengerRange = mFf.Groups["range"].Value,
                            Message = "FARE FAMILIES:" + mFf.Groups["families"].Value.Trim()
                        });
                    continue;
                }

                // ── 7. Page indicator ─────────────────────────────────────────────
                var mPg = RxPage.Match(line);
                if (mPg.Success)
                {
                    result.PageIndicator = mPg.Groups["page"].Value.Trim();
                    continue;
                }

                // ── 8. Ranged notices  (1-2 LAST TKT DTE … / 1-2 FARE VALID …) ──
                var mNotice = RxRangedNotice.Match(trimmedLine);
                if (mNotice.Success)
                {
                    var notice = new FareNotice
                    {
                        PassengerRange = mNotice.Groups["range"].Value,
                        Message = mNotice.Groups["msg"].Value.Trim()
                    };
                    var mTkt = RxLastTktDate.Match(notice.Message);
                    if (mTkt.Success)
                        notice.LastTicketDate = mTkt.Groups["date"].Value;

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
