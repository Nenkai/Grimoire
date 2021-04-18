using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Database.Controllers;
using GTGrimServer.Database.Tables;
using GTGrimServer.Filters;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Config;
using GTGrimServer.Services;

namespace GTGrimServer.Controllers.Profiles
{
    /// <summary>
    /// Handles profile related requests.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Route("/ap/[controller]/")]
    [Produces("application/xml")]
    [Authorize]
    public class ProfileController : GrimControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly GameServerOptions _gsOptions;

        private readonly FriendDBManager _friendDB;
        private readonly UserDBManager _userDB;
        private readonly UserSpecialDBManager _userSpecialDB;

        public ProfileController(PlayerManager players,
            UserDBManager userDB,
            FriendDBManager friendDB,
            UserSpecialDBManager userSpecialDB,
            IOptions<GameServerOptions> gsOptions, 
            ILogger<ProfileController> logger)
            : base(players)
        {
            _logger = logger;
            _gsOptions = gsOptions.Value;

            _userDB = userDB;
            _friendDB = friendDB;
            _userSpecialDB = userSpecialDB;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            GrimRequest gRequest = await GrimRequest.Deserialize(Request.Body);
            if (gRequest is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- {command}", gRequest.Command);

            switch (gRequest.Command)
            {
                case "profile.update":
                    return await OnProfileUpdate(player, gRequest);
                case "profile.getspecialstatus":
                    return OnGetSpecialStatus();
                case "profile.updatefriendlist":
                    return OnUpdateFriendList(gRequest);
                case "profile.getsimplefriendlist":
                    return await OnGetSimpleFriendList(player);
                case "profile.updateNickname":
                    return await OnUpdateNickname(gRequest, player);
                case "profile.setpresence":
                    return SetPresence(gRequest);
                case "profile.getSpecialList":
                    return await OnGetUserSpecialPresentList(gRequest);
            }

            _logger.LogDebug("Received unimplemented profile call: {command}", gRequest.Command);
            var res = GrimResult.FromInt(-1);
            return BadRequest(res);
        }

        private async Task<ActionResult> OnGetSimpleFriendList(Player player)
        {
            // TODO/Note: Param is "order", where 'A' is alphabetical?
            var friends = await _friendDB.GetAllFriendsOfUser(player.Data.Id);

            var simpleFriendList = new SimpleFriendList();
            foreach (var friend in friends)
            {
                var friendData = await _userDB.GetByIDAsync(friend.FriendId);
                simpleFriendList.Items.Add(new SimpleFriend(friendData.PsnId, friendData.ASpecLevel, friendData.BSpecLevel));
            }

            return Ok(simpleFriendList);
        }

        private async Task<ActionResult> OnProfileUpdate(Player player, GrimRequest requestReq)
        {
            // Can we assume its requestUpdateGameStats?
            if (requestReq.TryGetParameterByKey("license_level", out var licenseLevelParam) && int.TryParse(licenseLevelParam.Text, out int license_level))
            {
                if (!requestReq.TryGetParameterByKey("achievement", out var achievementParam) || !int.TryParse(achievementParam.Text, out int achievement))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("trophy", out var trophyParam) || !int.TryParse(trophyParam.Text, out int trophy))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("car_count", out var carCountParam) || !int.TryParse(carCountParam.Text, out int car_count))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("license_gold", out var licenseGoldParam) || !int.TryParse(licenseGoldParam.Text, out int license_gold))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("odometer", out var odometerParam) || !float.TryParse(odometerParam.Text, out float odometer))
                    return BadRequest();

                // Can we assume we have don't have more? If so, it's requestUpdateGameStats
                if (!requestReq.TryGetParameterByKey("win_count", out var winCountParam))
                    return await OnRequestUpdateGameStats(player, license_level, achievement, trophy, car_count, license_gold, odometer);

                // Then we know its a global profile update
                if (!int.TryParse(winCountParam.Text, out int win_count))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("credit", out var creditParam) || !int.TryParse(creditParam.Text, out int credit))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("license_silver", out var licenseSilverParam) || !int.TryParse(licenseSilverParam.Text, out int license_silver))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("license_bronze", out var licenseBronzeParam) || !int.TryParse(licenseBronzeParam.Text, out int license_bronze))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("aspec_level", out var aspecLevelParam) || !int.TryParse(aspecLevelParam.Text, out int aspec_level))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("bspec_level", out var bspecLevelParam) || !int.TryParse(bspecLevelParam.Text, out int bspec_level))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("aspec_exp", out var aspecExpParam) || !int.TryParse(aspecExpParam.Text, out int aspec_exp))
                    return BadRequest();

                if (!requestReq.TryGetParameterByKey("bspec_exp", out var bspecExpParam) || !int.TryParse(bspecExpParam.Text, out int bspec_exp))
                    return BadRequest();

                var data = player.Data;
                data.WinCount = win_count;
                data.Credit = credit;
                data.LicenseSilverCount = license_silver;
                data.LicenseBronzeCount = license_bronze;
                data.ASpecLevel = aspec_level;
                data.ASpecExp = aspec_exp;
                data.BSpecLevel = bspec_level;
                data.BSpecExp = bspec_exp;

                await _userDB.UpdateGeneralData(data);
                return Ok(GrimResult.FromBool(true));
            }
            // Is it requestUpdateAutoMessage?
            else if (requestReq.TryGetParameterByKey("welcomemessage", out var welcomeMessage) && requestReq.Params.ParamList.Count == 1)
                return await OnUpdateAutoMessage(player, welcomeMessage.Text);
            else if (requestReq.TryGetParameterByKey("helmet", out var helmet) && requestReq.TryGetParameterByKey("helmet_color", out var helmetColor))
                return await OnUpdateHelmet(player, helmet.Text, helmetColor.Text);
            else if (requestReq.TryGetParameterByKey("wear", out var wear) && requestReq.TryGetParameterByKey("wear_color", out var wear_color))
                return await OnUpdateWear(player, wear.Text, wear_color.Text);

            // No parsing is done.
            return Ok();
        }

        #region Profile Updaters
        private async Task<ActionResult> OnUpdateHelmet(Player player, string helmetParam, string helmetColorParam)
        {
            if (!int.TryParse(helmetParam, out int helmetId))
                return BadRequest();
            if (!int.TryParse(helmetColorParam, out int helmetColorId))
                return BadRequest();

            player.Data.HelmetId = helmetId;
            player.Data.HelmetColorId = helmetColorId;

            await _userDB.UpdateHelmet(player.Data);
            return Ok(GrimResult.FromBool(true));
        }

        private async Task<ActionResult> OnUpdateWear(Player player, string wearParam, string wearColorParam)
        {
            if (!int.TryParse(wearParam, out int wearId))
                return BadRequest();
            if (!int.TryParse(wearColorParam, out int wearColorId))
                return BadRequest();

            player.Data.WearId = wearId;
            player.Data.WearColorId = wearColorId;

            await _userDB.UpdateWear(player.Data);
            return Ok(GrimResult.FromBool(true));
        }

        private async Task<ActionResult> OnRequestUpdateGameStats(Player player, int license_level, int achievement, int trophy, 
            int car_count, int license_gold, float odometer)
        {
            // TODO: Do game stats verification
            var data = player.Data;
            data.LicenseLevel = license_level;
            data.AchievementCount = achievement;
            data.TrophyCount = trophy;
            data.CarCount = car_count;
            data.LicenseGoldCount = license_gold;
            data.Odometer = odometer;

            await _userDB.UpdateGameStats(data);
            return Ok(GrimResult.FromBool(true));
        }

        private async Task<ActionResult> OnUpdateAutoMessage(Player player, string message)
        {
            // TODO: Do game stats verification
            if (message.Length > 30)
                return BadRequest();

            var data = player.Data;
            data.WelcomeMessage = message;

            await _userDB.UpdateWelcomeMessage(data);
            return Ok(GrimResult.FromBool(true));
        }
        #endregion

        /// <summary>
        /// Fired by GT5 to get a special privilege
        /// </summary>
        /// <returns></returns>
        private ActionResult OnGetSpecialStatus()
        {
            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got special status request on a non GT5 server");
                return Forbid();
            }

            // Related to ranking stuff, possibly a cheat? See gtmode -> ATTRIBUTE_EVAL: requestSpecialStatus
            // Saw param "1001"
            var res = GrimResult.FromInt(1);
            return Ok(res);
        }

        /// <summary>
        /// Fired by GT5 to get the friend list
        /// </summary>
        /// <returns></returns>
        private ActionResult OnUpdateFriendList(GrimRequest gRequest)
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for friend list request");
                return Unauthorized();
            }

            if (!gRequest.TryGetParameterByKey("friend_list", out var param))
            {
                _logger.LogWarning("Got OnUpdateFriendList with missing or invalid friend_list key param");
                return BadRequest();
            }

            // Param is "friend_list"
            // Response is 0/1 bool, 1 is to ask for a refresh, 0 is not
            var res = GrimResult.FromInt(0);
            return Ok(res);
        }

        /// <summary>
        /// Fired by GT6 to set current presence
        /// </summary>
        /// <param name="gRequest"></param>
        /// <returns></returns>
        private ActionResult SetPresence(GrimRequest gRequest)
        {
            if (_gsOptions.GameType != GameType.GT6)
            {
                _logger.LogWarning("Got profile.setpresence request on a non GT6 server");
                return Forbid();
            }

            if (!gRequest.TryGetParameterByKey("stats", out var param))
            {
                _logger.LogWarning("Got SetPresence with missing parameter");
                return BadRequest();
            }

            _logger.LogDebug("<- SetPresence - {param}", param.Text);

            // No specific response needed
            var result = GrimResult.FromInt(0);
            return Ok(result);
        }

        /// <summary>
        /// Fired by GT6 to rename the user
        /// </summary>
        /// <param name="gRequest"></param>
        /// <returns></returns>
        private async Task<ActionResult> OnUpdateNickname(GrimRequest gRequest, Player player)
        {
            if (_gsOptions.GameType != GameType.GT6)
            {
                _logger.LogWarning("Got profile.updateNickname request on a non GT6 server");
                return Forbid();
            }

            if (!gRequest.TryGetParameterByKey("nickname", out var param))
            {
                _logger.LogWarning("Got SetPresence with missing nickname parameter");
                return BadRequest();
            }

            if (player.Data.NicknameChanges <= 0) 
                return BadRequest(GrimResult.FromBool(false)); // Exceeded nickname changes

            if (!IsValidNickname(param.Text))
                return BadRequest(GrimResult.FromBool(false));

            // TODO: Swear name filter?

            _logger.LogDebug("[{username}] updated nickname: {nickname}", player.Data.PSNName, param.Text);

            player.Data.NicknameChanges--;
            await _userDB.UpdateNewNickname(player.Data);

            var result = GrimResult.FromBool(true);
            return Ok(result);

        }

        /// <summary>
        /// Fired by GT6 - for special presents
        /// </summary>
        /// <param name="gRequest"></param>
        /// <returns></returns>
        private async Task<ActionResult> OnGetUserSpecialPresentList(GrimRequest gRequest)
        {
            if (_gsOptions.GameType != GameType.GT6)
            {
                _logger.LogWarning("Got profile.getSpecialList request on a non GT6 server");
                return BadRequest();
            }

            if (!gRequest.TryGetParameterByKey("type", out var param))
            {
                _logger.LogWarning("Got profile.getSpecialList request with missing parameter");
                return BadRequest();
            }

            if (param.Text != "3")
            {
                _logger.LogWarning("Got profile.getSpecialList request with type parameter different than 3: {type}", param.Text);
                return BadRequest();
            }

            var specialDataList = await _userSpecialDB.GetAllPresentsOfUserAsync(Player.Data.Id, 3);
            var result = new SpecialList();
            foreach (var specialData in specialDataList)
            {
                var special = new UserSpecial(specialData.UserId, specialData.Type, specialData.Key, specialData.Value);
                result.Items.Add(special);

                if (specialData.Type == 3 && specialData.Key.StartsWith("CAR"))
                {
                    // Its a one-time car present
                    await _userSpecialDB.RemoveAsync(specialData.Id);
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Checks if the provided input is a valid nickname. A valid nickname would be 'E. xample'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsValidNickname(string name)
        {
            if (name.Length > 12 || name.Length == 0)
                return false;

            int spaceCount = 0;
            int dotCount = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsLetterOrDigit(name[i]))
                    continue;

                if (i == '.')
                    dotCount++;
                else if (i == ' ')
                    spaceCount++;
            }

            if (dotCount > 1 || spaceCount > 1)
                return false;

            return true;
        }
    }
}
