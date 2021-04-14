using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GTGrimServer.Database.Tables;

namespace GTGrimServer.Models
{
    public class Player
    {
        public User Data { get; init; }
        public SessionToken Token { get; set; }
        public DateTime LastUpdate { get; set; }

        public void SetLastUpdatedNow()
            => LastUpdate = DateTime.Now;
    }
}
