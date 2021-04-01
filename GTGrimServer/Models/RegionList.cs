using System;
using System.Xml.Serialization;

namespace GTGrimServer
{
    [XmlRoot("region")]
    public class RegionList
    {
        [XmlAttribute(AttributeName = "key")]
        public string RegionCode { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string RegionName { get; set; }
    }
}
