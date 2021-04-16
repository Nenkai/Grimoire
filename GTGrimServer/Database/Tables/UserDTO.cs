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
        public string PSNName { get; set; }

        /// <summary>
        /// Nickname of the user.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// IP Address of the user.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Country of the user.
        /// </summary>
        public string Country { get; set; }

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

        /// <summary>
        /// Color Id of the helmet that the user is wearing.
        /// </summary>
        public int HelmetId { get; set; }

        /// <summary>
        /// Id of the helmet color that the user is using.
        /// </summary>
        public int HelmetColorId { get; set; }

        /// <summary>
        /// Id of the suit color that the user is wearing.
        /// </summary>
        public int WearId { get; set; }

        /// <summary>
        /// Color Id of the suit that the user is wearing.
        /// </summary>
        public int WearColorId { get; set; }

        /// <summary>
        /// Profile comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Id of the avatar for the player.
        /// </summary>
        public long AvatarPhotoId { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public int BandTest { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public int BandUp { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public int BandDown { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public DateTime BandUpdateTime { get; set; }

        /// <summary>
        /// For GT5. Menu design matiere index.
        /// </summary>
        public int MenuMatiere { get; set; }

        /// <summary>
        /// For GT5. Menu color index.
        /// </summary>
        public int MenuColor { get; set; }

        /// <summary>
        /// For GT5. Unknown.
        /// </summary>
        public int PlaytimeLevel { get; set; }

        /// <summary>
        /// For GT5. Unknown.
        /// </summary>
        public int ProfileLevel { get; set; }

        /// <summary>
        /// For GT5. Unknown.
        /// </summary>
        public int CommentLevel { get; set; }

        /// <summary>
        /// For GT5. Unknown.
        /// </summary>
        public string Playtime { get; set; }

        /// <summary>
        /// GT6 only (?). Current category tag. Refer to [CommunityTag|CATEGORY_*] in RText for an overview.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Remaining nickname changes available.
        /// </summary>
        public int NicknameChanges { get; set; } = DefaultNicknameChangeCount;
        public const int DefaultNicknameChangeCount = 3;
    }
}
