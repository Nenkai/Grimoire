﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "gtmail")]
    public class GTMail
    {
        [XmlAttribute(AttributeName = "mail_id")]
        public long MailId { get; set; }

        [XmlAttribute(AttributeName = "from")]
        public long From { get; set; }

        [XmlAttribute(AttributeName = "to")]
        public long To { get; set; }

        [XmlAttribute(AttributeName = "from_nickname")]
        public string FromNickname { get; set; }

        [XmlAttribute(AttributeName = "to_nickname")]
        public string ToNickname { get; set; }

        [XmlAttribute(AttributeName = "subject")]
        public string Subject { get; set; }

        [XmlAttribute(AttributeName = "body")]
        public string Body { get; set; }

        [XmlAttribute(AttributeName = "create_time")]
        public string CreateTime { get; set; }

    }
}
