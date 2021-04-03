using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace GTGrimServer.Utils
{
    public class XmlSerializerOutputFormatterNamespace : XmlSerializerOutputFormatter
    {
        public XmlSerializerOutputFormatterNamespace(XmlWriterSettings settings)
            : base(settings)
        {
            
        }

        protected override void Serialize(XmlSerializer xmlSerializer, XmlWriter xmlWriter, object value)
        {
            //applying "empty" namespace will produce no namespaces
            var emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "any-non-empty-string");
            xmlSerializer.Serialize(xmlWriter, value, emptyNamespaces);
        }
    }
}
