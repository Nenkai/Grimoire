using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "profile")]
    public class SimpleFriend
    {
        [XmlAttribute(AttributeName = "item")]
        public long Id { get; set; }

        [XmlAttribute(AttributeName = "aspec_level")]
        public int ASpecLevel { get; set; }

        [XmlAttribute(AttributeName = "bspec_level")]
        public int BSpecLevel { get; set; }
    }
}
