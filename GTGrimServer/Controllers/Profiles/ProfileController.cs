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
                _logger.LogWarning("Could not get current player for host {host} (command: {command})");
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
                    return OnProfileUpdate(gRequest);
                case "profile.getspecialstatus":
                    return OnGetSpecialStatus();
                case "profile.updatefriendlist":
                    return OnUpdateFriendList(gRequest);
                case "profile.getsimplefriendlist":
                    return await OnGetSimpleFriendList(player);
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

        private ActionResult OnProfileUpdate(GrimRequest requestReq)
        {
            /* GT5 returns params: 
               * aspec_level
               * aspec_exp
               * bspec_level
               * bspec_exp
               * achievement
               * credit
               * win_count
               * car_count
               * trophy
               * odometer
               * license_level
               * license_gold
               * license_silver
               * license_bronze */

            // requestUpdateHelmet & requestUpdateWear has some extra ones for this endpoint.
            /* helmet/helmet_color */
            /* wear/wear_color */

            // welcomemessage - requestUpdateAutoMessage
            if (requestReq.TryGetParameterByKey("welcomemessage", out var param) && requestReq.Params.ParamList.Count == 1)
            {
                var res = GrimResult.FromInt(0); // need_update
                return Ok(res);
            }

            // No parsing is done.
            return Ok();
        }

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
    }
}
