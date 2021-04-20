using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class BbsDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Board ID. Directly linked to the internal user id. User Id 1 = Board Id 1.
        /// </summary>
        public int BbsBoardId { get; set; }

        /// <summary>
        /// Internal database id of the user who posted the comment.
        /// </summary>
        public int AuthorId { get; set; }

        public string Comment { get; set; }

        public DateTime CreateTime { get; set; }

        public BbsDTO() { }

        public BbsDTO(int boardId, string comment, DateTime createTime)
        {
            BbsBoardId = boardId;
            Comment = comment;
            CreateTime = createTime;
        }
    }
}
