using System.Text;
using System.Xml.Linq;

namespace EvoSC.Common.Util;

public static class XmlUtils
{
    private sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    
    /// <summary>
    /// Generate a full and formatted XML string of the provided document.
    /// </summary>
    /// <param name="doc">The document to generate an XML string from.</param>
    /// <returns></returns>
    public static string GetFullXmlString(this XDocument doc)
    {
        var sw = new Utf8StringWriter();
        doc.Save(sw);
        return sw.ToString();
    }
}
