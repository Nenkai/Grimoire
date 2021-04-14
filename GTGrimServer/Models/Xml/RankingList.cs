using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "ranking_list")]
    public class RankingList
    {
        public List<Ranking> Rankings { get; set; }
    }

    [XmlRoot(ElementName = "ranking")]
    public class Ranking
    {
        [XmlAttribute(AttributeName = "board_id")]
        public long BoardId { get; set; }

        [XmlAttribute(AttributeName = "score")]
        public int Score { get; set; } 

        [XmlAttribute(AttributeName = "replay_id")]
        public int ReplayId { get; set; }

        [XmlAttribute(AttributeName = "rank")]
        public int Rank { get; set; }

        [XmlAttribute(AttributeName = "src_id")]
        public int SourceId { get; set; }

        [XmlAttribute(AttributeName = "user_id")]
        public string UserId { get; set; }

        [XmlAttribute(AttributeName = "create_time")]
        public string CreateTime { get; set; }

        [XmlAttribute(AttributeName = "stats")]
        public byte[] Stats { get; set; }

        [XmlAttribute(AttributeName = "nickname")]
        public string Nickname { get; set; }
    }
}
