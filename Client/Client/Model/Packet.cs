using System;
using System.Diagnostics;
using System.Xml.Linq;
namespace Common
{
    public class Packet
    {
        public const string XmlVersion = "1.0";

        private XDocument _xmlDocument;

        public Packet()
        {
            CreateNewXml();
        }

        public static Packet Instance
        {
            get
            {
                return new Packet();
            }
        }

        public string RawXml
        {
            get
            {
                return _xmlDocument.ToString(SaveOptions.None);
            }
        }

        public XDocument XmlDocument
        {
            get
            {
                return _xmlDocument;
            }
            set
            {
                _xmlDocument = value;
            }
        }

        public XElement DataNode
        {
            get
            {
                if (_xmlDocument != null && _xmlDocument.Root != null)
                    return _xmlDocument.Root.Element("data");

                return null;
            }
        }

        // ----------------------------------------------------------------------
        public void CreateNewXml()
        {
            _xmlDocument = new XDocument(new XElement("gtw", new XAttribute("version", XmlVersion)),
                new XElement("control"), 
                new XElement("data"));
        }

        // ----------------------------------------------------------------------
        public XElement AddData(string nodeName, string value)
        {
            return AddElement("data", nodeName, value);
        }

        // ----------------------------------------------------------------------
        public XElement AddCommand(string command)
        {
            return AddElement("control", "command", command);
        }

        // ----------------------------------------------------------------------
        public XElement AddElement(string parentNode, string elementName, string value)
        {
            Debug.Assert(_xmlDocument.Root != null, "_xmlDocument.Root != null");
            XElement parent = _xmlDocument.Root.Element(parentNode);
            
            if (parent == null)
            {
                parent = new XElement(parentNode);
                _xmlDocument.Root.Add(parent);
            }

            XElement xmlElement = new XElement(elementName);
            xmlElement.SetValue(value);
            parent.Add(xmlElement);
            return xmlElement;
        }

        public Byte[] ToByte(string message)
        {
            Byte[] byteMessage = new byte[message.Length + 1];
            
            for(int i = 0; i < message.Length; i++)
            {
                byteMessage[i] = Convert.ToByte(message[i]);
            }

            return byteMessage;
        }
    }
}
