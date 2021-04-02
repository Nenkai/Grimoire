using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot("result")]
    public class TicketResult
    {
        /// <summary>
        /// Whether it succeeded.
        /// </summary>
        [XmlElement(ElementName = "result")]
        public string Result { get; set; }

        /// <summary>
        /// PSN User ID.
        /// </summary>
        [XmlElement(ElementName = "user_id")]
        public ulong UserId { get; set; }

        /// <summary>
        /// Nickname.
        /// </summary>
        [XmlElement(ElementName = "nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// User Number.
        /// </summary>
        [XmlElement(ElementName = "user_no")]
        public string UserNumber { get; set; }

        /// <summary>
        /// Server Time.
        /// </summary>
        [XmlElement(ElementName = "server_time")]
        public string ServerTime { get; set; }
    }
}
