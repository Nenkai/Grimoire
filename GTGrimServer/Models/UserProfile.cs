using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

namespace GTGrimServer.Database.Tables
{
    [XmlRoot(ElementName = "user")]
    public class UserProfile
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "number")]
        public long Number { get; set; }

        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }

        [XmlElement(ElementName = "nickname")]
        public string Nickname { get; set; }

        [XmlElement(ElementName = "gt_friend_list")]
        public string GTFriendList { get; set; }

        [XmlElement(ElementName = "photo_avatar")]
        public string PhotoAvatar { get; set; }

        [XmlElement(ElementName = "photo_bg")]
        public string PhotoBackground { get; set; }

        [XmlElement(ElementName = "band_test")]
        public int BandTest { get; set; }

        [XmlElement(ElementName = "band_up")]
        public int BandUp { get; set; }

        [XmlElement(ElementName = "band_down")]
        public int BandDown { get; set; }

        [XmlElement(ElementName = "band_update_time")]
        public string BandUpdateTime { get; set; }

        [XmlElement(ElementName = "country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "stats")]
        public byte[] Stats { get; set; }

        [XmlElement(ElementName = "menu_color")]
        public int MenuColor { get; set; }

        [XmlElement(ElementName = "menu_matiere")]
        public int MenuMatiere { get; set; }

        [XmlElement(ElementName = "helmet")]
        public int Helmet { get; set; }

        [XmlElement(ElementName = "helmet_color")]
        public int HelmetColor { get; set; }

        [XmlElement(ElementName = "menu_helmet")]
        public int MenuHelmet { get; set; }

        [XmlElement(ElementName = "wear")]
        public int Wear { get; set; }

        [XmlElement(ElementName = "wear_color")]
        public int WearColor { get; set; }

        [XmlElement(ElementName = "menu_suit")]
        public int MenuSuit { get; set; }

        [XmlElement(ElementName = "aspec_level")]
        public int ASpecLevel { get; set; }

        [XmlElement(ElementName = "bspec_level")]
        public int BSpecLevel { get; set; }

        [XmlElement(ElementName = "license_level")]
        public int LicenseLevel { get; set; }

        [XmlElement(ElementName = "profile_level")]
        public int ProfileLevel { get; set; }

        [XmlElement(ElementName = "comment_level")]
        public int CommentLevel { get; set; }

        [XmlElement(ElementName = "playtime_level")]
        public int PlaytimeLevel { get; set; }

        [XmlElement(ElementName = "playtime")]
        public string Playtime { get; set; }

        [XmlElement(ElementName = "welcomemessage")]
        public string WelcomeMessage { get; set; }

        [XmlElement(ElementName = "aspec_exp")]
        public string ASpecExp { get; set; }

        [XmlElement(ElementName = "bspec_exp")]
        public string BSpecExp { get; set; }

        [XmlElement(ElementName = "achievement")]
        public int Achievement { get; set; }

        [XmlElement(ElementName = "credit")]
        public int Credit { get; set; }

        [XmlElement(ElementName = "win_count")]
        public int WinCount { get; set; }

        [XmlElement(ElementName = "car_count")]
        public int CarCount { get; set; }

        [XmlElement(ElementName = "trophy")]
        public int Trophy { get; set; }

        [XmlElement(ElementName = "odometer")]
        public double Odometer { get; set; }

        [XmlElement(ElementName = "license_gold")]
        public int LicenseGold { get; set; }

        [XmlElement(ElementName = "license_silver")]
        public int LicenseSilver { get; set; }

        [XmlElement(ElementName = "license_bronze")]
        public int LicenseBronze { get; set; }
    }
}
