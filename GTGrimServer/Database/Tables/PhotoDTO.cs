using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class PhotoDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// Database Id of the user that triggered this action.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// When the activity occured.
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string Comment { get; set; }
        public string CarName { get; set; }
        public string Place { get; set; }

        public PhotoDTO() { }

        public PhotoDTO(int userId, DateTime createTime, string comment, string carName, string place)
        {
            UserId = userId;
            CreateTime = createTime;
            Comment = comment;
            CarName = carName;
            Place = place;
        }
    }
}
