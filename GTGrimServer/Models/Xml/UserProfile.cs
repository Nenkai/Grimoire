using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

using GTGrimServer.Database.Tables;
using GTGrimServer.Utils;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "user")]
    public class UserProfile
    {
        /// <summary>
        /// User name of the player. Attribute says 'id', but its actually the user name.
        /// </summary>
        [XmlElement(ElementName = "id")]
        public string UserName { get; set; }

        [XmlElement(ElementName = "number")]
        public long Number { get; set; }

        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }

        [XmlElement(ElementName = "nickname")]
        public string Nickname { get; set; }

        [XmlElement(ElementName = "gt_friend_list")]
        public string GTFriendList { get; set; }

        [XmlElement(ElementName = "photo_avatar")]
        public long PhotoAvatar { get; set; }

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
        public int ASpecExp { get; set; }

        [XmlElement(ElementName = "bspec_exp")]
        public int BSpecExp { get; set; }

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

        [XmlElement(ElementName = "tag")]
        public int Tag { get; set; }

        [XmlElement(ElementName = "nickname_change")]
        public int NicknameChanges { get; set; }

        public static UserProfile FromDatabaseObject(UserDTO userDto)
        {
            var profile = new UserProfile();
            profile.UserName = userDto.PSNName;
            profile.Number = userDto.Id;
            profile.Comment = userDto.Comment;
            profile.Nickname = userDto.Nickname;
            profile.PhotoAvatar = userDto.AvatarPhotoId;
            profile.PhotoBackground = "";

            profile.BandDown = userDto.BandDown;
            profile.BandUp = userDto.BandUp;
            profile.BandTest = userDto.BandTest;
            profile.BandUpdateTime = userDto.BandUpdateTime.ToRfc3339String();

            profile.Country = userDto.Country;
            profile.Playtime = userDto.Playtime;

            profile.MenuColor = userDto.MenuColor;
            profile.MenuMatiere = userDto.MenuMatiere;

            profile.Odometer = userDto.Odometer;
            profile.Credit = userDto.Credit;

            profile.ASpecExp = userDto.ASpecExp;
            profile.BSpecExp = userDto.BSpecExp;
            profile.ASpecLevel = userDto.ASpecLevel;
            profile.BSpecLevel = userDto.BSpecLevel;

            profile.LicenseLevel = userDto.LicenseLevel;
            profile.CommentLevel = userDto.CommentLevel;
            profile.PlaytimeLevel = userDto.PlaytimeLevel;
            profile.ProfileLevel = userDto.ProfileLevel;

            profile.LicenseGold = userDto.LicenseGoldCount;
            profile.LicenseSilver = userDto.LicenseSilverCount;
            profile.LicenseBronze = userDto.LicenseBronzeCount;
            profile.Achievement = userDto.AchievementCount;
            profile.Trophy = userDto.TrophyCount;
            profile.WinCount = userDto.WinCount;
            profile.CarCount = userDto.CarCount;
            profile.WelcomeMessage = userDto.WelcomeMessage;


            // TODO: Figure menu_suit out
            profile.Helmet = userDto.HelmetId;
            profile.HelmetColor = userDto.HelmetColorId;
            profile.Wear = userDto.WearId;
            profile.WearColor = userDto.WearColorId;

            profile.NicknameChanges = userDto.NicknameChanges;
            return profile;
        }
    }
}
