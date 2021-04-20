using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("items")]
    public class ItemBoxList
    {
        [XmlElement("item")]
        public List<ItemBoxList> Items { get; set; }
    }

    [XmlRoot("item")]
    public class ItemBox
    {
        [XmlAttribute("itembox_id")]
        public long ItemBoxId { get; set; }

        [XmlAttribute("type")]
        public byte Type { get; set; }

        [XmlAttribute("sender")]
        public string Sender { get; set; }

        [XmlAttribute("receiver")]
        public byte Receiver { get; set; }

        [XmlAttribute("comment")]
        public byte Comment { get; set; }

        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }

        [XmlAttribute("stats")]
        public byte[] Stats { get; set; }
    }
}
