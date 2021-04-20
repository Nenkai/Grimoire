using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GTGrimServer.Database.Tables;

namespace GTGrimServer.Models
{
    /// <summary>
    /// Grim Player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Database Object.
        /// </summary>
        public UserDTO Data { get; }

        /// <summary>
        /// Current Session Token.
        /// </summary>
        public SessionToken Token { get; set; }

        /// <summary>
        /// Last Request Date.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Sets the last update to now.
        /// </summary>
        public void SetLastUpdatedNow()
            => LastUpdate = DateTime.Now;

        public override string ToString()
            => Data.PSNUserId;

        public Player(UserDTO user, SessionToken token)
        {
            Data = user;
            Token = token;
        }
    }
}
