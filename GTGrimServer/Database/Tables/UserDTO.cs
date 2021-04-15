using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class UserDTO
    {
        /// <summary>
        /// Internal Database Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// PSN Id of the user.
        /// </summary>
        public long PsnId { get; set; }

        /// <summary>
        /// PSN Name of the user.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// IP Address of the user.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Mac address of the user.
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// A-Spec Level of the user (GT5 only).
        /// </summary>
        public int ASpecLevel { get; set; }

        /// <summary>
        /// A-Spec XP of the user (GT5 only).
        /// </summary>
        public int ASpecExp { get; set; }

        /// <summary>
        /// B-Spec Level of the user (GT5 only).
        /// </summary>
        public int BSpecLevel { get; set; }

        /// <summary>
        /// B-Spec XP of the user (GT5 only).
        /// </summary>
        public int BSpecExp { get; set; }

        /// <summary>
        /// Achievement count of the user.
        /// </summary>
        public int AchievementCount { get; set; }

        /// <summary>
        /// Last registered cash of the user.
        /// </summary>
        public int Credit { get; set; }

        /// <summary>
        /// Races won count.
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// Garage car count.
        /// </summary>
        public int CarCount { get; set; }

        /// <summary>
        /// Event trophy count.
        /// </summary>
        public int TrophyCount { get; set; }

        /// <summary>
        /// Total distance driven.
        /// </summary>
        public int Odometer { get; set; }

        /// <summary>
        /// Current license of the user.
        /// </summary>
        public int LicenseLevel { get; set; }

        /// <summary>
        /// Total golded licenses.
        /// </summary>
        public int LicenseGoldCount { get; set; }

        /// <summary>
        /// Total silvered licenses.
        /// </summary>
        public int LicenseSilverCount { get; set; }

        /// <summary>
        /// Total bronzed licenses.
        /// </summary>
        public int LicenseBronzeCount { get; set; }

        public int HelmetId { get; set; }
        public int HelmetColorId { get; set; }

        public int WearId { get; set; }
        public int WearColorId { get; set; }
    }
}
