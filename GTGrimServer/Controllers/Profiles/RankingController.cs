using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using GTGrimServer.Filters;
using GTGrimServer.Config;
using GTGrimServer.Utils;
using GTGrimServer.Models;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Event Rankings
    /// </summary>
    [ApiController]
    [PDIClient]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class RankingController : ControllerBase
    {
        private readonly ILogger<RankingController> _logger;
        private readonly GameServerOptions _gsOptions;

        public RankingController(IOptions<GameServerOptions> options, ILogger<RankingController> logger)
        {
            _logger = logger;
            _gsOptions = options.Value;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- Got ranking request: {command}", requestReq.Command);

            switch (requestReq.Command)
            {
                case "ranking.calc2":
                    return OnGetCalc2Ranking(requestReq);
                case "ranking.getCount":
                    return OnGetCount(requestReq);
                case "ranking.getListFriends":
                    return OnGetFriendList(requestReq);
            }

            _logger.LogDebug("<- Got unknown ranking command: {command}", requestReq.Command);
            var badReqs = GrimResult.FromInt(-1);
            return BadRequest(badReqs);
        }

        public ActionResult OnGetCount(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("board_id", out var boardIdParam))
            {
                _logger.LogWarning("Got ranking getCount request without 'board_id'");
                return BadRequest();
            }

            return Ok(GrimResult.FromInt(1));
        }

        public ActionResult OnGetFriendList(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("board_id", out var boardIdParam))
            {
                _logger.LogWarning("Got ranking getCount request without 'board_id'");
                return BadRequest();
            }

            var rankingList = new RankingList();
            return Ok(rankingList);
        }

        public ActionResult OnGetCalc2Ranking(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("board_id", out var boardIdParam))
            {
                _logger.LogWarning("Got ranking calc2 request without 'board_id'");
                return BadRequest();
            }

            if (!request.TryGetParameterByKey("user_id", out var userIdParam))
            {
                _logger.LogWarning("Got ranking calc2 request without 'user_id'");
                return BadRequest();
            }

            var rankingList = new RankingList();
            return Ok(rankingList);
        }
    }
}