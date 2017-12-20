using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HowLeaky.Tools.XML
{
    public class XMLUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static string readXMLAttribute(XElement elem, string attrName)
        {
            XAttribute attr = elem.Attribute(attrName);

            return (readXMLAttribute(attr));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string readXMLAttribute(XAttribute attr)
        {
            if (attr == null)
            {
                return "";
            }
            else
            {
                return attr.Value.ToString();
            }
        }
    }
}
