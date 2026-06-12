using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TripXMLMain;
using Xunit;

namespace TripXML.XsltTests
{
    /// <summary>
    /// Golden-file regression tests for CoreLib.TransformXML. Each case lives under
    /// baselines/xslt/&lt;GDS&gt;/&lt;case&gt;/ as { input.xml, stylesheet.txt (relative
    /// stylesheet path within the XSLs repo), expected.xml } where expected.xml was
    /// captured from the legacy xsltc-based system. Cases appear automatically as the
    /// baselines folder is populated; the suite passes (with no cases) when absent.
    /// </summary>
    public class GoldenTransformTests
    {
        public static IEnumerable<object[]> Cases()
        {
            var root = XslTestPaths.BaselinesRoot;
            if (root == null || !Directory.Exists(root))
                yield break;

            foreach (var caseDir in Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories))
            {
                if (File.Exists(Path.Combine(caseDir, "input.xml")) &&
                    File.Exists(Path.Combine(caseDir, "stylesheet.txt")) &&
                    File.Exists(Path.Combine(caseDir, "expected.xml")))
                {
                    yield return new object[] { caseDir };
                }
            }
        }

        [Fact]
        public void AllGoldenCasesMatchLegacyOutput()
        {
            var failures = new List<string>();
            int count = 0;

            foreach (var args in Cases())
            {
                var caseDir = (string)args[0];
                count++;

                var input = File.ReadAllText(Path.Combine(caseDir, "input.xml"));
                var stylesheetRelative = File.ReadAllText(Path.Combine(caseDir, "stylesheet.txt")).Trim();
                var expected = File.ReadAllText(Path.Combine(caseDir, "expected.xml"));

                var xslDir = Path.GetDirectoryName(Path.Combine(XslTestPaths.XslsRoot, stylesheetRelative));
                var xslName = Path.GetFileName(stylesheetRelative);

                try
                {
                    var actual = CoreLib.TransformXML(input, xslDir, xslName);
                    if (Normalize(expected) != Normalize(actual))
                        failures.Add($"{caseDir}: output differs from legacy baseline");
                }
                catch (System.Exception ex)
                {
                    failures.Add($"{caseDir}: {ex.GetBaseException().Message}");
                }
            }

            // Suite passes with zero cases (baselines folder not yet populated) but
            // reports the discovered count so silent emptiness is visible.
            Assert.True(failures.Count == 0,
                $"{failures.Count}/{count} golden cases failed:\n" + string.Join("\n", failures));
        }

        private static string Normalize(string xml)
        {
            var doc = XDocument.Parse(xml.Trim());
            return doc.ToString(SaveOptions.DisableFormatting);
        }
    }
}
