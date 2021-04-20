using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("servers")]
    public class ServerList
    {
        [XmlElement("server")]
        public Server[] Servers { get; set; }
    }

    [XmlRoot("server")]
    public class Server
    {
        [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("options")]
        public List<ServerOption> Options { get; set; }
    }

    [XmlRoot("option")]
    public class ServerOption
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
