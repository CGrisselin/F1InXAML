using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace F1InXAML
{
    internal class ApiHelper
    {
        internal static XDocument ReadResponse(Stream stream, out XNamespace ns)
        {
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var xml = streamReader.ReadToEnd();
            var xDocument = XDocument.Parse(xml);
            ns = xDocument.Root.GetDefaultNamespace();
            return xDocument;
        }

        internal static WebRequest CreateWebRequest(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.UseDefaultCredentials = true;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            return webRequest;
        }
    }
}
