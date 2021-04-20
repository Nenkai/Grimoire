using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("ranking_list")]
    public class RankingList
    {
        [XmlElement("ranking")]
        public List<Ranking> Rankings { get; set; }
    }

    [XmlRoot("ranking")]
    public class Ranking
    {
        [XmlAttribute("board_id")]
        public long BoardId { get; set; }

        [XmlAttribute("score")]
        public int Score { get; set; } 

        [XmlAttribute("replay_id")]
        public int ReplayId { get; set; }

        [XmlAttribute("rank")]
        public int Rank { get; set; }

        [XmlAttribute("src_id")]
        public int SourceId { get; set; }

        [XmlAttribute("user_id")]
        public string UserId { get; set; }

        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }

        [XmlAttribute("stats")]
        public byte[] Stats { get; set; }

        [XmlAttribute("nickname")]
        public string Nickname { get; set; }
    }
}
