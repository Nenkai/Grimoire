using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Tables
{
    public class User
    {
        public long Id { get; set; }
        public long PsnId { get; set; }
        public string Nickname { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }

        public int ASpecLevel { get; set; }
        public int ASpecExp { get; set; }
        public int BSpecLevel { get; set; }
        public int BSpecExp { get; set; }
        public int AchievementCount { get; set; }
        public int Credit { get; set; }
        public int WinCount { get; set; }
        public int CarCount { get; set; }
        public int TrophyCount { get; set; }
        public int Odometer { get; set; }
        public int LicenseLevel { get; set; }
        public int LicenseGoldCount { get; set; }
        public int LicenseSilverCount { get; set; }
        public int LicenseBronzeCount { get; set; }

        public int HelmetId { get; set; }
        public int HelmetColorId { get; set; }

        public int WearId { get; set; }
        public int WearColorId { get; set; }
    }
}
