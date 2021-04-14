using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Models
{
    /// <summary>
    /// A session token to grant authorization for a player.
    /// </summary>
    public class SessionToken
    {
        /// <summary>
        /// Actual token string.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration date of the token.
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        public SessionToken(string token, DateTime expiry)
        {
            Token = token;
            ExpiryDate = expiry;
        }
    }
}
