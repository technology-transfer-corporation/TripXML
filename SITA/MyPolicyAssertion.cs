using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Services3.Security.Tokens;
using System.Xml;
using Microsoft.Web.Services3;

namespace Sita.Sws
{
    class MyOutputFilter : SendSecurityFilter
    {
        private string certificatePath;
        private string certificatePassword;

        private X509SecurityToken certToken;
        private MessageSignature oSignature;
        public MyOutputFilter(string certificatePath, string certificatePassword)
            : base("", true)
        {
            this.certificatePath = certificatePath;
            this.certificatePassword = certificatePassword;

            //load the certificate
            X509Certificate2 certLocal2 = new X509Certificate2(certificatePath, certificatePassword);
            certToken = new X509SecurityToken(certLocal2);

            //create the signature
            oSignature = new MessageSignature(certToken);
            oSignature.SignatureOptions = SignatureOptions.IncludeSoapBody;
        }

        public override void SecureMessage(SoapEnvelope envelope, Security security)
        {
            //add the token to the message
            security.Tokens.Add(certToken);
            security.MustUnderstand = true;
            security.Elements.Add(oSignature);
        }

        public override SoapFilterResult ProcessMessage(SoapEnvelope envelope)
        {
            base.ProcessMessage(envelope);

            //Remove the unneeded elements
            XmlNode actionNode = envelope.Header["wsa:Action"];
            if (actionNode != null)
                envelope.Header.RemoveChild(actionNode);

            XmlNode messageNode = envelope.Header["wsa:MessageID"];
            if (messageNode != null)
                envelope.Header.RemoveChild(messageNode);

            XmlNode replyToNode = envelope.Header["wsa:ReplyTo"];
            if (replyToNode != null)
                envelope.Header.RemoveChild(replyToNode);

            XmlNode toNode = envelope.Header["wsa:To"];
            if (toNode != null)
                envelope.Header.RemoveChild(toNode);

            XmlNode sec = envelope.Header["wsse:Security"];
            if (sec != null)
            {
                XmlNode ts = sec.FirstChild;
                if (ts != null)
                    sec.RemoveChild(ts);
            }

            return SoapFilterResult.Continue;
        }
    }

    class MyPolicyAssertion : SecurityPolicyAssertion
    {
        private string certificatePath;
        private string certificatePassword;

        public MyPolicyAssertion(string certificatePath, string certificatePassword)
        {
            this.certificatePath = certificatePath;
            this.certificatePassword = certificatePassword;
        }

        public override Microsoft.Web.Services3.SoapFilter CreateClientOutputFilter(FilterCreationContext context)
        {
            return new MyOutputFilter(certificatePath, certificatePassword);
        }

        public override Microsoft.Web.Services3.SoapFilter CreateClientInputFilter(FilterCreationContext context)
        {
            return null;
        }

        public override Microsoft.Web.Services3.SoapFilter CreateServiceInputFilter(FilterCreationContext context)
        {
            return null;
        }

        public override Microsoft.Web.Services3.SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
        {
            return null;
        }
    }
}
