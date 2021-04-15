using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class UserSpecialDTO
    {
        /// <summary>
        /// Row ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Database User ID
        /// </summary>
        public long UserId { get; set; }

        public int Type { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
