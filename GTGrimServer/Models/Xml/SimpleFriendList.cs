using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    /// <summary>
    /// Basic list of friends.
    /// </summary>
    [XmlRoot(ElementName = "friends")]
    public class SimpleFriendList
    {
        [XmlElement("profile")]
        public List<SimpleFriend> Items { get; set; } = new List<SimpleFriend>();
    }

    /// <summary>
    /// Simple friend profile.
    /// </summary>
    [XmlRoot("profile")]
    public class SimpleFriend
    {
        /// <summary>
        /// PSN Id of the friend.
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public long UserId { get; set; }

        /// <summary>
        /// A-Spec Level of the friend (GT5 only).
        /// </summary>
        [XmlAttribute(AttributeName = "aspec_level")]
        public int ASpecLevel { get; set; }

        /// <summary>
        /// B-Spec Level of the friend (GT5 only).
        /// </summary>
        [XmlAttribute(AttributeName = "bspec_level")]
        public int BSpecLevel { get; set; }

        public SimpleFriend() { }

        public SimpleFriend(long userId, int aspecLevel, int bspecLevel)
        {
            UserId = userId;
            ASpecLevel = aspecLevel;
            BSpecLevel = bspecLevel;
        }
    }
}
