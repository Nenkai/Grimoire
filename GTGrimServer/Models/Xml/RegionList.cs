using System;
using System.Xml.Serialization;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("region")]
    public class RegionList
    {
        [XmlAttribute("key")]
        public string RegionCode { get; set; }

        [XmlAttribute("value")]
        public string RegionName { get; set; }
    }
}
