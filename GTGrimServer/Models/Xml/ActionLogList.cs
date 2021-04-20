using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    /// <summary>
    /// Represents a GT5 board's comment list.
    /// </summary>
    [XmlRoot("actionlog_list")]
    public class ActionLogList
    {
        [XmlElement("actionlog")]
        public List<ActionLog> Logs { get; set; } = new();
    }

    [XmlRoot("actionlog")]
    public class ActionLog
    {
        public static Dictionary<string, string> Tags = new()
        {
            { "NG", "NEW_GAME" },
            { "CB", "CAR_BUY" },
            { "CR", "CAR_RIDE" },
            { "FR", "FRIENDCAR_RIDE" },
            { "LL", "LICENSE_LEVEL" },
            { "LE", "LICENSE_LE" },
            { "AL", "ASPEC_LEVEL" },
            { "AE", "ASPEC_EVENT" },
            { "BE", "BSPEC_EVENT" },
            { "SE", "SPECIAL_EVENT" },
            { "DC", "DRIVER_CLASS" },
            { "GS", "GIFT_SPECIAL" },
            { "GM", "GIFT_MUSEUMCARD" },
            { "GT", "GIFT_TUNEPARTS" },
            { "GO", "GIFT_OTHERPARTS" },
            { "GD", "GIFT_DRIVER_ITEM" },
            { "TU", "TROPHY_UNLOCK" },
        };

        /// <summary>
        /// Action Log Id.
        /// </summary>
        //[XmlAttribute("actionlog_id")]
        //public long Id { get; set; }

        /// <summary>
        /// Creation date of this comment.
        /// </summary>
        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }

        /// <summary>
        /// Type of action.
        /// </summary>
        //[XmlAttribute("action_type")]
        //public byte ActionType { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("value1")]
        public string Value1 { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("value2")]
        public string Value2 { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("value3")]
        public string Value3 { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("value4")]
        public string Value4 { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("value5")]
        public string Value5 { get; set; }

    }
}
