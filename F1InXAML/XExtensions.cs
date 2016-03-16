using System.Xml.Linq;

namespace F1InXAML
{
    internal static class XExtensions
    {
        public static string ElementValueOrNull(this XElement xElement, XName name)
        {
            var element = xElement.Element(name);
            return element?.Value;
        }
    }
}