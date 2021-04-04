using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Sony;
using GTGrimServer.Models;
using GTGrimServer.Utils;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles profile related requests.
    /// </summary>
    [ApiController]
    [Route("/ap/[controller]/")]
    [Produces("application/xml")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                var res = GrimResult.FromInt(0);
                return Ok(res);
            }

            _logger.LogDebug("<- {command}", requestReq.Command);

            if (requestReq.Command == "profile.update") // requestUpdateMyHomeProfile
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

                // No parsing is done.
                return Ok();

            }
            else if (requestReq.Command == "profile.getspecialstatus")
            {
                // Related to ranking stuff, possibly a cheat? See gtmode -> ATTRIBUTE_EVAL: requestSpecialStatus
                // Saw param "1001"
                var res = GrimResult.FromInt(1);
                return Ok(res);
            }
            else if (requestReq.Command == "profile.updatefriendlist")
            {
                // Param is "friend_list"
                // Response should be a string of comma seperated ints (ids?)
                var res = GrimResult.FromInt(1);
                return Ok(res);
            }
            else if (requestReq.Command == "profile.getsimplefriendlist")
            {
                // Param is "order"
                var friendList = new SimpleFriendList();
                return Ok(friendList);
            }
            else
            {
                _logger.LogDebug("Received unimplemented profile call: {command}", requestReq.Command);
                var res = GrimResult.FromInt(1);
                return Ok(res);
            }

            
        }

    }
}
