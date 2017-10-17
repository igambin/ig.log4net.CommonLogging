using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ig.log4net.XmlTools
{
    public class Linq2Xml
    {

        public static IEnumerable<XElement> GetElements(string input, string name = null)
        {
            XDocument doc = XDocument.Parse(input);
            if (string.IsNullOrWhiteSpace(name)) return doc.Root?.Elements();
            return doc.Root?.Elements().Where(e => e.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static int CountElements(string input, string name = null)
        {
            XDocument doc = XDocument.Parse(input);
            if (string.IsNullOrWhiteSpace(name)) return doc.Root?.Elements().Count() ?? 0;
            return doc.Root?.Elements().Count(e => e.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase)) ?? 0;
        }
        
        public static IEnumerable<XElement> GetDescendants(string input, string name = null)
        {
            XDocument doc = XDocument.Parse(input);
            if (string.IsNullOrWhiteSpace(name)) return doc.Root?.Descendants();
            return doc.Root?.Descendants().Where(e => e.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
