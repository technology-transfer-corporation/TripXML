using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace TripXMLMain
{
    /// <summary>
    /// Replacement for the msxsl:script VisualBasic blocks that the legacy xsltc-compiled
    /// stylesheets carried, bound to namespace urn:ttVB. Registered on every transform via
    /// XsltArgumentList.AddExtensionObject. XSLT binds extension functions by reflected
    /// method name, case-sensitively — names and signatures must match the script functions.
    /// Implementations call the same Microsoft.VisualBasic runtime overloads the compiled
    /// VB scripts called, so conversion/formatting semantics are identical.
    /// </summary>
    public sealed class TtVbXsltFunctions
    {
        public const string Namespace = "urn:ttVB";
        public static readonly TtVbXsltFunctions Instance = new();

        private TtVbXsltFunctions() { }

        public string FctDateDuration(string p_startDate, string p_endDate)
        {
            if (Information.IsDate(p_startDate) && Information.IsDate(p_endDate))
                return Conversions.ToString(DateAndTime.DateDiff("d", p_startDate, p_endDate));
            return p_startDate + p_endDate;
        }

        public DateTime FctArrDate(string p_startDate, double p_DateChange)
        {
            if (Information.IsDate(p_startDate))
                return DateAndTime.DateAdd("d", p_DateChange, p_startDate);
            // legacy script assigned the string to a Date return under Option Strict Off,
            // i.e. an implicit CDate that throws for non-dates — preserved
            return Conversions.ToDate(p_startDate);
        }

        public string ShortDateFormat(string p_startDate)
        {
            if (Information.IsDate(p_startDate))
                return Convert.ToDateTime(p_startDate).ToString("yyyy-MM-d"); // single 'd' matches legacy output
            return p_startDate;
        }

        public string GetBirthDate(string age)
        {
            return DateTime.Now.AddYears(Convert.ToInt32(age) * -1).ToString("yyyy-MM-dd");
        }

#pragma warning disable IDE1006 // name must match the XSLT call ttVB:datenow()
        public string datenow()
#pragma warning restore IDE1006
        {
            return DateTime.Now.ToString("ddMMMyy");
        }
    }
}
