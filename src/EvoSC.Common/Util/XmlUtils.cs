using System.Text;
using System.Xml.Linq;

namespace EvoSC.Common.Util;

public static class XmlUtils
{
    private class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    
    public static string GetFullXmlString(this XDocument doc)
    {
        var sw = new Utf8StringWriter();
        doc.Save(sw);
        return sw.ToString();
    }
}
