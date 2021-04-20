using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    /// <summary>
    /// Represents a NP Login Ticket result.
    /// </summary>
    [XmlRoot("grim")]
    public class TicketResult
    {
        /// <summary>
        /// Whether it succeeded.
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }

        /// <summary>
        /// PSN User ID.
        /// </summary>
        [XmlElement("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Nickname.
        /// </summary>
        [XmlElement("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Grim User Id.
        /// </summary>
        [XmlElement("user_no")]
        public int UserNumber { get; set; }

        /// <summary>
        /// Server Time.
        /// </summary>
        [XmlElement("server_time")]
        public string ServerTimeString { get; set; }
        [XmlIgnore]
        public DateTime ServerTime
        {
            get => DateTimeExtensions.FromRfc3339String(ServerTimeString);
            set => ServerTimeString = value.ToRfc3339String();
        }
    }
}
