using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "presence_list")]
    public class PresenceList
    {
        public List<Mail> Mails { get; set; }
    }

    [XmlRoot(ElementName = "user_presence")]
    public class Presence
    {
        [XmlAttribute(AttributeName = "id")]
        public long Id { get; set; }

        [XmlAttribute(AttributeName = "stats")]
        public string Stats { get; set; } // ENTER COMMUNITY WEB

        [XmlAttribute(AttributeName = "type")]
        public int Type { get; set; }

        [XmlAttribute(AttributeName = "update_time")]
        public string UpdateTime { get; set; }
    }

    public enum PresenceState
    {
        Offline,
        OutOfContext,
        SameContext,
        Error
    }
}
