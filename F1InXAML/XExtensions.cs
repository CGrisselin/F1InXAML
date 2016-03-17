using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf.Transitions;

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