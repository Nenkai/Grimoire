using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    /// <summary>
    /// Represents a GT5 board's comment list.
    /// </summary>
    [XmlRoot("bbs_comment_list")]
    public class BbsCommentList
    {
        [XmlElement("bbs_comment")]
        public List<BbsComment> Comments { get; set; } = new();
    }

    public class BbsComment
    {
        /// <summary>
        /// Comment Id.
        /// </summary>
        [XmlAttribute("bbs_comment_id")]
        public long CommentId { get; set; }

        /// <summary>
        /// Board Id that this comment belongs to.
        /// </summary>
        [XmlAttribute("bbs_board_id")]
        public long BoardId { get; set; }

        /// <summary>
        /// Author User Id of this comment.
        /// </summary>
        [XmlAttribute("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Contents of this comment.
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Nickname of the author of this comment.
        /// </summary>
        [XmlAttribute("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Creation date of this comment.
        /// </summary>
        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }
    }
}
