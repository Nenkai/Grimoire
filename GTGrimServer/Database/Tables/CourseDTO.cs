using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class CourseDTO
    {
        /// <summary>
        /// Row Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Internal Database Id of the main user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Internal Database Id of the friend.
        /// </summary>
        public long FriendId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int Status { get; set; }

        public long PhotoId { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public int TitleHidden { get; set; }

        public int CommentHidden { get; set; }

        public int PhotoHidden { get; set; }

        public string Theme { get; set; }

        public int Length { get; set; }

        public int OneWay { get; set; }

        /// <summary>
        /// Internal database id of the source user who created this track.
        /// </summary>
        public long SourceUserId { get; set; }

        public int Straight { get; set; }

        public int Height { get; set; }
    }
}
