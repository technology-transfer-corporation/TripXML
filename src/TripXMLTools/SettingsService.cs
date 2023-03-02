using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Configuration;
using System.Xml.Serialization;

namespace TripXMLTools
{
    public static class SettingsService
    {
        public static TripXmlSettings GetAppSettings(NameValueCollection headers)
        {
            var settings = new TripXmlSettings();

            try
            {
                if(Authorize(headers))
                {

                    foreach ( var key in WebConfigurationManager.AppSettings.AllKeys )
                    {
                        settings.Setting.Add(new Setting
                        {
                            Key= key,
                            Value= WebConfigurationManager.AppSettings[key]
                        });
                    }
                }
                else
                {
                    throw new Exception("Authorization failed.");
                }
            }
            catch (Exception ex)
            {
                settings.Setting.Add(new Setting
                {
                    Key = "Error",
                    Value= ex.Message
                });
            }

            return settings;
        }

        private static bool Authorize(NameValueCollection headers)
        {
            var authHeader = headers.Get("Authorization");

            if (string.IsNullOrEmpty(authHeader))
            {
                return false;
            }

            if(!authHeader.StartsWith("Bearer "))
            {
                return false;
            }

            var token = authHeader.Substring(7);

            var jwtToken = new JwtSecurityToken(token);

            var audience = jwtToken.Audiences.FirstOrDefault();

            if (!string.IsNullOrEmpty(audience) && audience != "http://localhost:7103")
            {
                return false;
            }

            var roleClaims = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (roleClaims == null)
            {
                return false;
            }

            if (roleClaims.Value != "Administrator")
            {
                throw new Exception("Access is denied.");
            }

            return true;
        }
    }

    [XmlRoot(ElementName = "TripXmlSettings")]
    public class TripXmlSettings
    {
        [XmlElement]
        public List<Setting> Setting { get; set; } = new List<Setting>();
    }

    [XmlRoot(ElementName = "Setting")]
    public class Setting
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
