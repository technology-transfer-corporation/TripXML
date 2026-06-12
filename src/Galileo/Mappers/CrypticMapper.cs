using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Galileo.Mappers
{
    public static class CrypticMapper
    {
        /// <summary>
        /// Maps an OTA CrypticRQ (XML input) to the native Galileo request format (string).
        /// </summary>
        public static string MapRequest(string xmlRequest)
        {
            try
            {
                var doc = XDocument.Parse(xmlRequest);
                var entry = doc.Root?.Element("Entry")?.Value;

                if (string.IsNullOrEmpty(entry))
                    throw new Exception("Missing 'Entry' element in CrypticRQ.");

                return entry;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing CrypticRQ request: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps the raw Galileo response to the OTA CrypticRS (XML format).
        /// </summary>
        public static string MapResponse(string rawResponse, string formattedScreen, string conversationId)
        {
            try
            {
                var screenLines = formattedScreen?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None) ?? Array.Empty<string>();

                var doc = new XDocument(new XElement("CrypticRS",
                    new XElement("Success"),
                    new XElement("Response", rawResponse.Replace(" & ", " and ")),
                    new XElement("Screen",
                        new List<XElement>(Array.ConvertAll(screenLines, line => new XElement("Line", line)))
                    ),
                    new XElement("ConversationID", conversationId ?? string.Empty)
                ));

                return doc.ToString(SaveOptions.DisableFormatting);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error mapping CrypticRS response: {ex.Message}", ex);
            }
        }
    }
}
