using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Config;
using GTGrimServer.Services;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Filters;
using GTGrimServer.Results;
using GTGrimServer.Database.Controllers;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Handles logging made by the game for the server to keep track of what the player is doing.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Route("/ap/[controller]/")]
    [Produces("application/xml")]
    public class ActionLogController : GrimControllerBase
    {
        private readonly ILogger<ActionLogController> _logger;
        private readonly GameServerOptions _gsOptions;
        private readonly ActionLogDBManager _actionLogDb;

        public ActionLogController(PlayerManager players,
            IOptions<GameServerOptions> options, 
            ILogger<ActionLogController> logger,
            ActionLogDBManager actionLogDb)
            : base(players)
        {
            _logger = logger;
            _gsOptions = options.Value;
            _actionLogDb = actionLogDb;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string server)
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got actionlog request on a non GT5 server from host: {host}", Request.Host);
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
                case "actionlog.putActionLog":
                    return await OnActionPutLog(player, gRequest);
            }

            _logger.LogDebug("Got unimplemented actionlog call: {command}", gRequest.Command);
            

            return BadRequest();
        }

        [HttpGet]
        [Route("{userNumber}.xml")]
        public async Task GetList(int userNumber)
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got getActionLogList request on a non GT5 server from host: {host}", Request.Host);
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }


            var actions = await _actionLogDb.GetAllActionsOfUser(userNumber);
            if (actions is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            /* Alright, time for a "Here's the deal." time in a typical GT5 dudu situation
             * GT5 has two list requests - requestGetActionLogList and requestGetActionLogList2.
             * The former, just fetches the list and decodes it directly, like any other request to grim.
             * The second, which is what GT5 uses, fetches the response, doesn't decode it, just puts it in the grim cache line any other request.
             *   - It then reads the cached file RAW, splits the lines by '\n', THEN parsing it using REGEX with the FOLLOWING SHIT:
             *     '<actionlog create_time="([^"]*)" value1="([^"]*)" value2="([^"]*)" value3="([^"]*)" value4="([^"]*)" value5="([^"]*)"/>'
             *   As such, XmlSerializer has to be dirty configured to only print new lines and remove any indentation.
             *   BUT EVEN THEN! It doesn't work, because XmlSerializer produces spaces before closing tags which fails the regex match. No way to get around that.
             * So yeah, do that shit manually. If you want to witness that horror, it's located in gtmode.adc:requestActionLogList (Line 2421), Version 2.11.
             */

            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            sw.NewLine = "\n";

                foreach (var action in actions)
                    sw.WriteLine(@$"<actionlog create_time=""{Utils.DateTimeExtensions.ToRfc3339String(action.CreateTime)}"" value1=""{action.Value1}"" value2=""{action.Value2}"" value3=""{action.Value3}"" value4=""{action.Value4}"" value5=""{action.Value5}""/>");
            
            sw.Flush();
            ms.Position = 0;
            await ms.CopyToAsync(Response.Body);
            return;
        }

        // see requestActionLogList in gtmode.ad
        private async Task<ActionResult> OnActionPutLog(Player player, GrimRequest gRequest)
        {
            if (!gRequest.TryGetParameterByKey("action_type", out var actionTypeParam) || !int.TryParse(actionTypeParam.Text, out int action_type))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'action_type' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("value1", out var value1Param))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'value1' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("value2", out var value2Param))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'value2' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("value3", out var value3Param))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'value3' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("value4", out var value4Param))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'value4' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("value5", out var value5Param))
            {
                _logger.LogWarning($"Got actionlog.putActionLog request with missing 'value5' parameter");
                return BadRequest();
            }

            if (!ActionLog.Tags.TryGetValue(value1Param.Text, out string def))
                return Ok();

            // TODO: Some actionlog checks
            LogAction(player, def, value2Param.Text, value3Param.Text, value4Param.Text, value5Param.Text);

            var log = new ActionLogDTO(player.Data.Id, DateTime.Now, value1Param.Text, value2Param.Text, 
                value3Param.Text, value4Param.Text, value5Param.Text);

            await _actionLogDb.AddAsync(log);
            return Ok(GrimResult.FromBool(true));
        }

        private void LogAction(Player player, string def, string v2, string v3, string v4, string v5)
        {
            switch (def)
            {
                case "NEW_GAME":
                    _logger.LogInformation("[{player}] started new game", player); break;
                case "FRIENDCAR_RIDE":
                    _logger.LogInformation("[{player}] borrowing friend car: {car} from {userid}", player, v2, v3); break;


                case "CAR_BUY":
                    _logger.LogInformation("[{player}] bought car: {car}", player, v2); break;
                case "CAR_RIDE":
                    _logger.LogInformation("[{player}] now using car: {car}", player, v2); break;

                case "ASPEC_EVENT":
                    if (!int.TryParse(v2, out int agame_id))
                        return;
                    _logger.LogInformation("[{player}] finished aspec event: game_id:{game_id} course:{v3}", player, agame_id, v3); break;
                case "BSPEC_EVENT":
                    if (!int.TryParse(v2, out int bgame_id))
                        return;
                    _logger.LogInformation("[{player}] finished bspec event: game_id:{game_id} course:{v3}", player, bgame_id, v3); break;

                case "ASPEC_LEVEL":
                    if (!int.TryParse(v2, out int alevel))
                        return;
                    _logger.LogInformation("[{player}] a-spec level: {old}->{new}", player, alevel - 1, alevel); break;
                case "BSPEC_LEVEL":
                    if (!int.TryParse(v2, out int blevel))
                        return;
                    _logger.LogInformation("[{player}] b-spec level: {old}->{new}", player, blevel - 1, blevel); break;

                case "LICENSE_LEVEL":
                    if (!int.TryParse(v2, out int lLevel))
                        return;
                    _logger.LogInformation("[{player}] new license: {level}", player, lLevel); break;
                case "LICENSE_EVENT":
                    if (!int.TryParse(v2, out int license_id) || !int.TryParse(v3, out int result))
                        return;
                    _logger.LogInformation("[{player}] finished license event: id:{v2} result:{v3}", player, license_id, result); break;

                case "SPECIAL_EVENT":
                    if (!int.TryParse(v2, out int event_id))
                        return;
                    _logger.LogInformation("[{player}] finished special event: event_id:{game_id} course:{v3} result:", player, event_id, v3, v4); break;

                case "DRIVER_CLASS":
                    _logger.LogInformation("[{player}] driver class: {name} class: {class)", player, v2, v3); break;

                case "GIFT_MUSEUMCARD":
                    _logger.LogInformation("[{player}] gifted museum card: id:{museum_id} dealer:{dealer} dealer_id:{dealer_id}", player, v2, v3, v4); break;
                case "GIFT_SPECIAL":
                    _logger.LogInformation("[{player}] gifted car ticket: gameitem_id:{gameitem_id} label:{label}", player, v2, v3); break;
                case "GIFT_TUNEPARTS":
                    _logger.LogInformation("[{player}] gifted tune parts: gameitem_id:{gameitem_id} horn_id:{horn_id}", player, v2, v3); break;
                case "GIFT_OTHERPARTS":
                    _logger.LogInformation("[{player}] gifted paint: gameitem_id:{gameitem_id} color:{color}", player, v2, v3); break;
                case "GIFT_DRIVER_ITEM":
                    _logger.LogInformation("[{player}] gifted driver item: gameitem_id:{gameitem_id} code:{code}, color:{v4}", player, v2, v3, v4); break;

                case "TROPHY_UNLOCK":
                    _logger.LogInformation("[{player}] unlocked trophy: {type}", player, v2); break;
            }
        }
    }
}
