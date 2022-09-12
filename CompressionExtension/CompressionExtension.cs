using System;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using ICSharpCode.SharpZipLib.GZip;
using System.Xml;

namespace CompressionExtension
{
    /// <summary>
    /// Summary description for ConpressionExtension.
    /// </summary>
    public class CompressionExtension : System.Web.Services.Protocols.SoapExtension
    {
        Stream oldStream;
        Stream newStream;

        Boolean bCompress = false;


        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return attribute;
        }

        // Get the Type
        public override object GetInitializer(Type t)
        {
            return typeof(CompressionExtension);
        }

        // Get the CompressionExtensionAttribute
        public override void Initialize(object initializer)
        {
            CompressionExtensionAttribute attribute = (CompressionExtensionAttribute)initializer;

            return;
        }

        // Process the SOAP Message
        public override void ProcessMessage(SoapMessage message)
        {

            // Check for the various SOAP Message Stages 
            switch (message.Stage)
            {

                case SoapMessageStage.BeforeSerialize:
                    break;

                case SoapMessageStage.AfterSerialize:
                    // ZIP the contents of the SOAP Body after it has
                    // been serialized
                    Zip();
                    break;

                case SoapMessageStage.BeforeDeserialize:
                    // Unzip the contents of the SOAP Body before it is
                    // deserialized
                    Unzip();
                    break;

                case SoapMessageStage.AfterDeserialize:
                    break;

                default:
                    throw new Exception("invalid stage");
            }
        }

        // Gives us the ability to get hold of the RAW SOAP message
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        // Utility method to copy streams
        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }


        // Zip the SOAP Body
        private void Zip()
        {
            newStream.Position = 0;
            // Zip the SOAP Body
            newStream = ZipSoap(newStream);
            // Copy the streams
            Copy(newStream, oldStream);
        }

        // The actual ZIP method
        private byte[] Zip(string stringToZip)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToZip);
            MemoryStream ms = new MemoryStream();

            // Check the #ziplib docs for more information
            //ZipOutputStream zipOut = new ZipOutputStream( ms ) ;
            //ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            //zipOut.PutNextEntry(ZipEntry);
            //zipOut.SetLevel(9);
            //zipOut.Write(inputByteArray, 0 , inputByteArray.Length ) ;     
            //zipOut.Finish();
            //zipOut.Close();

            GZipOutputStream zipOut = new GZipOutputStream(ms);
            zipOut.Write(inputByteArray, 0, inputByteArray.Length);
            zipOut.Finish();
            zipOut.Close();

            // Return the zipped contents
            return ms.ToArray();
        }

        // Select and Zip the appropriate parts of the SOAP message
        public MemoryStream ZipSoap(Stream streamToZip)
        {
            streamToZip.Position = 0;
            // Load a XML Reader
            XmlTextReader reader = new XmlTextReader(streamToZip);
            XmlDocument dom = new XmlDocument();
            MemoryStream ms = new MemoryStream();
            dom.Load(reader);

            if (bCompress)
            {
                // Load a NamespaceManager to enable XPath selection
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(dom.NameTable);
                nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                XmlNode node = dom.SelectSingleNode("//soap:Body", nsmgr);
                // Select the contents within the method defined in the SOAP body

                node = node.FirstChild;
                // Check if there are any nodes selected

                if (node != null && node.LocalName != "Fault")
                {
                    while (node != null)
                    {
                        if (node.InnerXml.Length > 0)
                        {
                            // Zip the data
                            byte[] outData = Zip(node.InnerXml);
                            // Convert it to Base64 for transfer over the internet
                            node.InnerXml = Convert.ToBase64String(outData);
                        }
                        // Move to the next parameter
                        node = node.NextSibling;
                    }
                }
                else
                {
                    bCompress = false;
                }

                dom.Save(ms);
            }
            else
            {
                ms = (MemoryStream)streamToZip;
            }

            // Save the updated data
           
            ms.Position = 0;

            return ms;
        }

        // Unzip the SOAP Body
        private void Unzip()
        {
            MemoryStream unzipedStream = new MemoryStream();

            TextReader reader = new StreamReader(oldStream);
            TextWriter writer = new StreamWriter(unzipedStream);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
            // Unzip the SOAP Body
            unzipedStream = UnzipSoap(unzipedStream);
            // Copy the streams
            Copy(unzipedStream, newStream);

            newStream.Position = 0;
        }

        // Actual Unzip logic
        private byte[] Unzip(string stringToUnzip)
        {
            // Decode the Base64 encoding
            byte[] inputByteArray = Convert.FromBase64String(stringToUnzip);
            MemoryStream ms = new MemoryStream(inputByteArray);
            MemoryStream ret = new MemoryStream();

            // Refer to #ziplib documentation for more info on this
            GZipInputStream zipIn = new GZipInputStream(ms);
            Byte[] buffer = new Byte[2048];
            int size = 2048;
            while (true)
            {
                size = zipIn.Read(buffer, 0, buffer.Length);
                if (size > 0)
                {
                    ret.Write(buffer, 0, size);
                }
                else
                {
                    break;
                }
            }

            return ret.ToArray();

            //ZipInputStream zipIn = new ZipInputStream(ms);
            //ZipEntry theEntry = zipIn.GetNextEntry();
            //Byte[] buffer = new Byte[2048] ;
            //int size = 2048;
            //while (true) 
            //{
            //    size = zipIn.Read(buffer, 0, buffer.Length);
            //    if (size > 0) 
            //    {
            //        ret.Write(buffer, 0, size);
            //    } 
            //    else 
            //    {
            //        break;
            //    }
            //}
            //return ret.ToArray();
        }

        // Unzip the SOAP Body
        public MemoryStream UnzipSoap(Stream streamToUnzip)
        {
            streamToUnzip.Position = 0;
            // Load a XmlReader
            XmlTextReader reader = new XmlTextReader(streamToUnzip);
            XmlDocument dom = new XmlDocument();
            MemoryStream ms = new MemoryStream();
            dom.Load(reader);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(dom.NameTable);
            nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");

            XmlNode nodeHeader = dom.SelectSingleNode("//soap:Header", nsmgr);

            if (nodeHeader != null)
            {
                XmlNode nodeFC = nodeHeader.FirstChild;

                if (nodeFC != null && nodeFC.LocalName == "TripXML")
                {
                    XmlNode node = nodeFC.LastChild;

                    if (node != null && node.LocalName == "compressed" && node.InnerText.ToLower() == "true")
                    {
                        // Select the SOAP Body node 
                        XmlNode nodeBody = dom.SelectSingleNode("//soap:Body", nsmgr);
                        if (nodeBody != null)
                        {
                            node = nodeBody.FirstChild;

                            if (node != null && node.FirstChild.LocalName == "#text")
                            {
                                bCompress = true;

                                // Check if node exists
                                while (node != null)
                                {
                                    if (node.InnerXml.Length > 0)
                                    {
                                        // Send the node's contents to be unziped
                                        byte[] outData = Unzip(node.InnerXml);
                                        string sTmp = Encoding.UTF8.GetString(outData);
                                        node.InnerXml = sTmp;
                                    }
                                    // Move to the next parameter 
                                    node = node.NextSibling;
                                }

                                dom.Save(ms);
                            }
                        }
                    }
                }
            }

            if (bCompress == false)
            {
                ms = (MemoryStream)streamToUnzip;
            }

            ms.Position = 0;

            return ms;
        }
    }
}
