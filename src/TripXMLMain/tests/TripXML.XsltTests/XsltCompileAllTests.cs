using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Xunit;

namespace TripXML.XsltTests
{
    /// <summary>
    /// The single most valuable gate in the migration: every in-scope stylesheet must load
    /// with the same engine settings CoreLib.TransformXML uses on .NET 10 (document() on,
    /// script off). Any msxsl:script remnant, include breakage, or syntax issue fails here.
    /// </summary>
    public class XsltCompileAllTests
    {
        // Portal*, SITA, TravelFusion are out of migration scope (archived components)
        private static readonly string[] InScopeFolders =
        {
            "Amadeus", "AmadeusWS", "Galileo", "Sabre", "Worldspan", "Travelport",
            "Aggregation", "BL", "TXML"
        };

        public static IEnumerable<object[]> InScopeFolderData() =>
            InScopeFolders.Select(f => new object[] { f });

        [Theory]
        [MemberData(nameof(InScopeFolderData))]
        public void AllStylesheetsInFolderCompile(string folder)
        {
            var dir = Path.Combine(XslTestPaths.XslsRoot, folder);
            Assert.True(Directory.Exists(dir), $"missing folder {dir}");

            var failures = new List<string>();
            int total = 0;

            foreach (var file in Directory.EnumerateFiles(dir, "*.xsl", SearchOption.AllDirectories))
            {
                total++;
                try
                {
                    var t = new XslCompiledTransform();
                    t.Load(file, new XsltSettings(enableDocumentFunction: true, enableScript: false), new XmlUrlResolver());
                }
                catch (Exception ex)
                {
                    failures.Add($"{Path.GetFileName(file)}: {ex.GetBaseException().Message}");
                }
            }

            Assert.True(total > 0, $"no stylesheets found under {dir}");
            Assert.True(failures.Count == 0,
                $"{failures.Count}/{total} stylesheets in {folder} failed to compile:\n" +
                string.Join("\n", failures.Take(40)));
        }
    }
}
