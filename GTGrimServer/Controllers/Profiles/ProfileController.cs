using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Sony;
using GTGrimServer.Filters;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Config;
using GTGrimServer.Controllers;

namespace GTGrimServer.Controllers.Profiles
{
    /// <summary>
    /// Handles profile related requests.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Route("/ap/[controller]/")]
    [Produces("application/xml")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly GameServerOptions _gsOptions;

        public ProfileController(IOptions<GameServerOptions> gsOptions, ILogger<ProfileController> logger)
        {
            _logger = logger;
            _gsOptions = gsOptions.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- {command}", requestReq.Command);

            switch (requestReq.Command)
            {
                case "profile.update":
                    return OnProfileUpdate(requestReq);
                case "profile.getspecialstatus":
                    return OnGetSpecialStatus();
                case "profile.updatefriendlist":
                    return OnUpdateFriendList();
                case "profile.getsimplefriendlist":
                    return OnGetSimpleFriendList();
                case "profile.setpresence":
                    return SetPresence(requestReq);
                case "profile.getSpecialList":
                    return OnGetUserSpecialPresentList(requestReq);
            }

            _logger.LogDebug("Received unimplemented profile call: {command}", requestReq.Command);
            var res = GrimResult.FromInt(-1);
            return BadRequest(res);
        }

        private ActionResult OnGetSimpleFriendList()
        {
            // Param is "order"
            var friendList = new SimpleFriendList();
            return Ok(friendList);
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
        private ActionResult OnUpdateFriendList()
        {
            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got special status request on a non GT5 server");
                return BadRequest();
            }

            // Param is "friend_list"
            // Response should be a string of comma seperated ints (ids?)
            var res = GrimResult.FromInt(1);
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
        private ActionResult OnGetUserSpecialPresentList(GrimRequest gRequest)
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

            var result = new SpecialList();
            return Ok(result);
        }
    }
}
