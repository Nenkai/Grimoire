using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "items")]
    public class ItemBoxList
    {
        [XmlAttribute(AttributeName = "item")]
        public List<ItemBox> Items { get; set; }
    }
}
