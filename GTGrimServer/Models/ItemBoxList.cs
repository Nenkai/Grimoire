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
        public List<ItemBoxList> Items { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class ItemBox
    {
        [XmlAttribute(AttributeName = "itembox_id")]
        public long ItemBoxId { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public byte Type { get; set; }

        [XmlAttribute(AttributeName = "sender")]
        public string Sender { get; set; }

        [XmlAttribute(AttributeName = "receiver")]
        public byte Receiver { get; set; }

        [XmlAttribute(AttributeName = "comment")]
        public byte Comment { get; set; }

        [XmlAttribute(AttributeName = "create_time")]
        public byte CreateTime { get; set; }

        [XmlAttribute(AttributeName = "stats")]
        public byte[] Stats { get; set; }
    }
}
