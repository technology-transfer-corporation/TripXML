using System;
using System.Globalization;
using System.Threading;
using TripXMLMain;
using Xunit;

namespace TripXML.XsltTests
{
    /// <summary>
    /// Value tables for the urn:ttVB extension object that replaced the msxsl:script
    /// VisualBasic blocks. Semantics must match the VB runtime behavior the compiled
    /// scripts had. Legacy servers ran en-US — pinned here to match.
    /// </summary>
    public class TtVbXsltFunctionsTests : IDisposable
    {
        private readonly CultureInfo _previous;

        public TtVbXsltFunctionsTests()
        {
            _previous = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        public void Dispose() => Thread.CurrentThread.CurrentCulture = _previous;

        private static readonly TtVbXsltFunctions F = TtVbXsltFunctions.Instance;

        [Theory]
        [InlineData("2026-01-01", "2026-01-02", "1")]
        [InlineData("2026-01-01 23:00", "2026-01-02 01:00", "0")] // VB DateDiff("d") truncates elapsed time (2h -> 0 days)
        [InlineData("2026-01-02", "2026-01-01", "-1")]
        [InlineData("2026-01-01", "2026-01-01", "0")]
        [InlineData("12/31/2025", "01/02/2026", "2")]
        public void FctDateDuration_DateInputs(string start, string end, string expected)
        {
            Assert.Equal(expected, F.FctDateDuration(start, end));
        }

        [Theory]
        [InlineData("notadate", "2026-01-02", "notadate2026-01-02")] // non-date → concatenation passthrough
        [InlineData("", "", "")]
        public void FctDateDuration_NonDateInputs(string start, string end, string expected)
        {
            Assert.Equal(expected, F.FctDateDuration(start, end));
        }

        [Fact]
        public void FctArrDate_AddsDays()
        {
            Assert.Equal(new DateTime(2026, 1, 3), F.FctArrDate("2026-01-01", 2d));
            Assert.Equal(new DateTime(2025, 12, 31), F.FctArrDate("2026-01-01", -1d));
        }

        [Fact]
        public void FctArrDate_NonDate_ThrowsLikeLegacyImplicitCDate()
        {
            Assert.ThrowsAny<Exception>(() => F.FctArrDate("notadate", 1d));
        }

        [Theory]
        [InlineData("2026-01-05", "2026-01-5")] // single 'd' format is intentional legacy behavior
        [InlineData("2026-01-15", "2026-01-15")]
        [InlineData("notadate", "notadate")]
        public void ShortDateFormat_Cases(string input, string expected)
        {
            Assert.Equal(expected, F.ShortDateFormat(input));
        }

        [Fact]
        public void GetBirthDate_SubtractsYears()
        {
            var expected = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            Assert.Equal(expected, F.GetBirthDate("30"));
        }

        [Fact]
        public void Datenow_FormatsDdMmmYy()
        {
            var result = F.datenow();
            Assert.Matches(@"^\d{2}[A-Za-z]{3}\d{2}$", result);
            Assert.Equal(DateTime.Now.ToString("ddMMMyy"), result);
        }
    }
}
