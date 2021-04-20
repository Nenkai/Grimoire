using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models.Xml
{
    /// <summary>
    /// Basic list of friends.
    /// </summary>
    [XmlRoot("friends")]
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
        /// PSN User Id of the friend.
        /// </summary>
        [XmlAttribute("id")]
        public string UserId { get; set; }

        /// <summary>
        /// A-Spec Level of the friend (GT5 only).
        /// </summary>
        [XmlAttribute("aspec_level")]
        public int ASpecLevel { get; set; }

        /// <summary>
        /// B-Spec Level of the friend (GT5 only).
        /// </summary>
        [XmlAttribute("bspec_level")]
        public int BSpecLevel { get; set; }

        public SimpleFriend() { }

        public SimpleFriend(string userId, int aspecLevel, int bspecLevel)
        {
            UserId = userId;
            ASpecLevel = aspecLevel;
            BSpecLevel = bspecLevel;
        }
    }
}
