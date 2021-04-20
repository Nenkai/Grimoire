using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

using GTGrimServer.Database.Tables;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("user")]
    public class UserProfile
    {
        /// <summary>
        /// PSN User Id of the profile.
        /// </summary>
        [XmlElement("id")]
        public string UserId { get; set; }

        [XmlElement("number")]
        public long Number { get; set; }

        [XmlElement("comment")]
        public string Comment { get; set; }

        [XmlElement("nickname")]
        public string Nickname { get; set; }

        [XmlElement("gt_friend_list")]
        public string GTFriendList { get; set; }

        [XmlElement("photo_avatar")]
        public long PhotoAvatar { get; set; }

        [XmlElement("photo_bg")]
        public string PhotoBackground { get; set; }

        [XmlElement("band_test")]
        public int BandTest { get; set; }

        [XmlElement("band_up")]
        public int BandUp { get; set; }

        [XmlElement("band_down")]
        public int BandDown { get; set; }

        [XmlElement("band_update_time")]
        public string BandUpdateTime { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("stats")]
        public byte[] Stats { get; set; }

        [XmlElement("menu_color")]
        public int MenuColor { get; set; }

        [XmlElement("menu_matiere")]
        public int MenuMatiere { get; set; }

        [XmlElement("helmet")]
        public int Helmet { get; set; }

        [XmlElement("helmet_color")]
        public int HelmetColor { get; set; }

        [XmlElement("menu_helmet")]
        public int MenuHelmet { get; set; }

        [XmlElement("wear")]
        public int Wear { get; set; }

        [XmlElement("wear_color")]
        public int WearColor { get; set; }

        [XmlElement("menu_suit")]
        public int MenuSuit { get; set; }

        [XmlElement("aspec_level")]
        public int ASpecLevel { get; set; }

        [XmlElement("bspec_level")]
        public int BSpecLevel { get; set; }

        [XmlElement("license_level")]
        public int LicenseLevel { get; set; }

        [XmlElement("profile_level")]
        public int ProfileLevel { get; set; }

        [XmlElement("comment_level")]
        public int CommentLevel { get; set; }

        [XmlElement("playtime_level")]
        public int PlaytimeLevel { get; set; }

        [XmlElement("playtime")]
        public string Playtime { get; set; }

        [XmlElement("welcomemessage")]
        public string WelcomeMessage { get; set; }

        [XmlElement("aspec_exp")]
        public int ASpecExp { get; set; }

        [XmlElement("bspec_exp")]
        public int BSpecExp { get; set; }

        [XmlElement("achievement")]
        public int Achievement { get; set; }

        [XmlElement("credit")]
        public int Credit { get; set; }

        [XmlElement("win_count")]
        public int WinCount { get; set; }

        [XmlElement("car_count")]
        public int CarCount { get; set; }

        [XmlElement("trophy")]
        public int Trophy { get; set; }

        [XmlElement("odometer")]
        public double Odometer { get; set; }

        [XmlElement("license_gold")]
        public int LicenseGold { get; set; }

        [XmlElement("license_silver")]
        public int LicenseSilver { get; set; }

        [XmlElement("license_bronze")]
        public int LicenseBronze { get; set; }

        [XmlElement("tag")]
        public int Tag { get; set; }

        [XmlElement("nickname_change")]
        public int NicknameChanges { get; set; }

        public static UserProfile FromDatabaseObject(UserDTO userDto)
        {
            var profile = new UserProfile();
            profile.UserId = userDto.PSNUserId;
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
