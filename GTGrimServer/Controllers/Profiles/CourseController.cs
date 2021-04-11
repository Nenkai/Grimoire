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

using GTGrimServer.Config;
using GTGrimServer.Utils;
using GTGrimServer.Models;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly GameServerOptions _gsOptions;

        public CourseController(IOptions<GameServerOptions> options, ILogger<CourseController> logger)
        {
            _logger = logger;
            _gsOptions = options.Value;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (_gsOptions.GameType != GameType.GT6)
            {
                _logger.LogWarning("Got course getlist request on non GT6");
                return BadRequest();
            }

            GrimRequest requestReq = await GrimRequest.Deserialize(Request.Body);
            if (requestReq is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- Got course request: {command}", requestReq.Command);

            switch (requestReq.Command)
            {
                case "course.getlist":
                    return OnGetList(requestReq);
            }

            _logger.LogDebug("<- Got unknown course command: {command}", requestReq.Command);
            var badReqs = GrimResult.FromInt(-1);
            return BadRequest(badReqs);
        }

        public ActionResult OnGetList(GrimRequest request)
        {
            if (!request.TryGetParameterByKey("user_id", out var userIdParam))
            {
                _logger.LogWarning("Got course getlist without 'user_id'");
                return BadRequest();
            }

            _logger.LogDebug("<- course.getlist {userId}", userIdParam.Text);

            var courseList = new CourseList();
            return Ok(courseList);
        }
    }
}