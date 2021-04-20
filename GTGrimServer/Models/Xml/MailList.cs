using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot(ElementName = "gtmail_list")]
    public class MailList
    {
        public List<Mail> Mails { get; set; }
    }

    [XmlRoot(ElementName = "gtmail")]
    public class Mail
    {
        [XmlAttribute(AttributeName = "mail_id")]
        public long MailId { get; set; }

        [XmlAttribute(AttributeName = "from")]
        public string FromUsername { get; set; }

        [XmlAttribute(AttributeName = "to")]
        public string ToUsername { get; set; }

        [XmlAttribute(AttributeName = "from_nickname")]
        public string FromNickname { get; set; }

        [XmlAttribute(AttributeName = "to_nickname")]
        public string ToNickname { get; set; }

        [XmlAttribute(AttributeName = "subject")]
        public string Subject { get; set; }

        [XmlAttribute(AttributeName = "body")]
        public string Body { get; set; }

        [XmlAttribute(AttributeName = "create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }

    }
}
