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
        
        /// <summary>
        /// Special type? GT6 has it as 3 always
        /// </summary>
        public int Type { get; set; }

        /* CAR_000x
         * 1: A gift for your participation in GT Academy 2013
         * 2: A gift for your participation in the BMW Z4 Challenge
         * 3-7: Entry Bonus: GT5 Final Event
         * 8: A gift for your participation in GT Academy 2014 */
        public string Key { get; set; }

        /// <summary>
        /// If the key is CAR_000x, this is a car label reward
        /// </summary>
        public string Value { get; set; }
    }
}
