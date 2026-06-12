using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TripXMLMain;

namespace wsTripXML.wsTravelTalk
{
    /// <summary>
    /// Startup glue that replaces Global.asax Application_Start: publishes configuration
    /// into the TripXMLMain bridges (modCore.config, modCore.*Path, AppState) and runs the
    /// legacy TripXMLStartUp table load in the background once the host is up.
    /// </summary>
    public static class TripXMLRuntime
    {
        private static Microsoft.AspNetCore.Http.IHttpContextAccessor _http;

        /// <summary>Replacement for ASMX Context.Request.UserHostAddress.</summary>
        public static string GetClientIpAddress()
        {
            return _http?.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        public static void Initialize(WebApplication app)
        {
            _http = app.Services.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            // Legacy servers ran en-US; XSLT extension functions and VB conversions depend on it.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            // Web.config <appSettings> -> appsettings.json "AppSettings" section -> modCore.config
            var section = app.Configuration.GetSection("AppSettings");
            var config = new System.Collections.Specialized.NameValueCollection();
            foreach (var child in section.GetChildren())
            {
                config[child.Key] = child.Value;
            }
            modCore.config = config;

            string contentRoot = app.Environment.ContentRootPath;
            string tripXmlFolder = config["TripXMLFolder"];
            string baseFolder = string.IsNullOrEmpty(tripXmlFolder) ? contentRoot : tripXmlFolder;

            modCore.XslPath = EnsureTrailingSlash(config["XslPath"] ?? Path.Combine(baseFolder, "Xsl"));
            modCore.LogPath = EnsureTrailingSlash(config["TripXMLLogFolder"] ?? Path.Combine(baseFolder, "Log"));
            modCore.SchemaPath = EnsureTrailingSlash(config["SchemaPath"] ?? Path.Combine(baseFolder, "Schemas"));

            AppState.Initialize(app.Services.GetRequiredService<IMemoryCache>());

            var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("TripXMLRuntime");
            app.Lifetime.ApplicationStarted.Register(() => Task.Run(() =>
            {
                try
                {
                    modMain.TripXMLStartUp();
                    TripXMLTools.TripXMLLoad.BuildDecodingDataViews();
                    logger.LogInformation("TripXML startup tables loaded.");
                }
                catch (Exception ex)
                {
                    // Legacy Application_Start swallowed startup failures into SendTrace —
                    // the host must come up even when Hasura/SQL are unreachable.
                    logger.LogError(ex, "TripXMLStartUp failed; tables not loaded.");
                    try { CoreLib.SendTrace("Trace", "TripXMLStartUp failed: " + ex.Message, "", "", ""); } catch { }
                }
            }));
        }

        private static string EnsureTrailingSlash(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.EndsWith("\\") || path.EndsWith("/") ? path : path + Path.DirectorySeparatorChar;
        }
    }
}
