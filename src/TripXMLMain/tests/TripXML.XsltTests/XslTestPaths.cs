using System;
using System.IO;

namespace TripXML.XsltTests
{
    internal static class XslTestPaths
    {
        /// <summary>
        /// Root of the XSLs repo. Override with the XSLS_ROOT environment variable;
        /// otherwise walk up from the test bin folder until a sibling "XSLs" folder
        /// containing "Amadeus" is found (matches the repos/ workspace layout).
        /// </summary>
        public static string XslsRoot { get; } = Resolve("XSLS_ROOT", "XSLs", "Amadeus");

        /// <summary>Optional golden baselines root (baselines/xslt at the workspace root).</summary>
        public static string BaselinesRoot { get; } = ResolveOptional("XSLT_BASELINES_ROOT", Path.Combine("baselines", "xslt"));

        private static string Resolve(string envVar, string folderName, string mustContain)
        {
            var env = Environment.GetEnvironmentVariable(envVar);
            if (!string.IsNullOrEmpty(env) && Directory.Exists(env))
                return env;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, folderName);
                if (Directory.Exists(Path.Combine(candidate, mustContain)))
                    return candidate;
                dir = dir.Parent;
            }

            throw new DirectoryNotFoundException(
                $"Could not locate the '{folderName}' repo above {AppContext.BaseDirectory}; set {envVar}.");
        }

        private static string ResolveOptional(string envVar, string relative)
        {
            var env = Environment.GetEnvironmentVariable(envVar);
            if (!string.IsNullOrEmpty(env) && Directory.Exists(env))
                return env;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, relative);
                if (Directory.Exists(candidate))
                    return candidate;
                dir = dir.Parent;
            }

            return null;
        }
    }
}
