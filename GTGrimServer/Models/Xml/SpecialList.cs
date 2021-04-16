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
        [XmlElement("user_special")]
        public List<UserSpecial> Items { get; set; } = new List<UserSpecial>();
    }

    /// <summary>
    /// Used by GT6 to award special presents?
    /// </summary>
    [XmlRoot("user_special")]
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

        public UserSpecial() { }

        public UserSpecial(long userId, int type, string key, string value)
        {
            UserId = userId;
            Key = key;
            Value = value;
            Type = type;
        }
    }
}
