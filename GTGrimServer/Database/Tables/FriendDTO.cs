using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class FriendDTO
    {
        /// <summary>
        /// Row Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Database Id of the main user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Database Id of the friend.
        /// </summary>
        public long FriendId { get; set; }
    }
}
