using System;
using System.Diagnostics;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Client.Model
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
            _xmlDocument = new XDocument(new XElement("gtw", new XAttribute("version", XmlVersion),
                new XElement("control"),
                new XElement("data")));
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
        public XElement AddCommand(int command)
        {
            return AddCommand(command.ToString());
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

        // ----------------------------------------------------------------------
        public string GetString(string name)
        {
            XElement valueElement = DataNode.Element(name);
            if (valueElement == null)
                return string.Empty;
            return valueElement.Value;
        }

        // ----------------------------------------------------------------------
        public int GetInt(string name)
        {
            XElement valueElement = DataNode.Element(name);
            if (valueElement == null)
                return -1;
            try
            {
                return Convert.ToInt32(valueElement.Value);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid integer.");
            }
            return -1;
        }

        //------------------------------------------------------------------------------
        public string ReadControl(string element)
        {
            try
            {
                return ReadElement("control", element);
            }
            catch
            {
                return string.Empty;
            }
        }

        //------------------------------------------------------------------------------
        public int GetCommand()
        {
            return Convert.ToInt32(ReadControl("command"));
        }

        //------------------------------------------------------------------------------
        private string ReadElement(string parentNode, string elementName)
        {
            try
            {
                if (_xmlDocument.Root.Element(parentNode).Element(elementName) != null)
                    return _xmlDocument.Root.Element(parentNode).Element(elementName).Value;
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}