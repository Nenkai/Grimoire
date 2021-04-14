using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "user_special_list")]
    public class SpecialList
    {
        public List<UserSpecial> Items { get; set; }
    }

    /// <summary>
    /// Used by GT6 to award special presents?
    /// </summary>
    [XmlRoot(ElementName = "user_special")]
    public class UserSpecial
    {
        [XmlAttribute(AttributeName = "user_id")]
        public long UserId { get; set; }

        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public int Type { get; set; }
    }
}
