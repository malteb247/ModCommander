using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModCommander.Utils
{
    public static class XmlDocumentExtensions
    {
        #region XML related

        public static string GetInnerTextSave(this XmlDocument doc, string xpath)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return string.Empty;
            else
                return node.InnerText;
        }

        public static string GetInnerTextNormalized(this XmlDocument doc, string xpath)
        {
            string result = doc.GetInnerTextSave(xpath);
            return result.NormalizeText();
        }

        public static string GetInnerTextSave(this XmlNode node, string xpath)
        {
            XmlNode selectedNode = node.SelectSingleNode(xpath);
            if (selectedNode == null)
                return string.Empty;
            else
                return selectedNode.InnerText;
        }

        public static string GetInnerTextNormalized(this XmlNode node, string xpath)
        {
            string result = node.GetInnerTextSave(xpath);
            return result.NormalizeText();
        }

        #endregion XML related
    }
}
