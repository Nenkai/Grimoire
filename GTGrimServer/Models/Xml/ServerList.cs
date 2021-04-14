using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;

namespace GTGrimServer
{
    [XmlRoot("servers")]
    public class ServerList
    {
        [XmlElement(ElementName = "server")]
        public Server[] Servers { get; set; }
    }

    [XmlRoot("server")]
    public class Server
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        
        [XmlElement(ElementName = "options")]
        public List<ServerOption> Options { get; set; }
    }

    [XmlRoot("option")]
    public class ServerOption
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
