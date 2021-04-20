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
using GTGrimServer.Utils;
using GTGrimServer.Database.Controllers;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Filters;
using GTGrimServer.Services;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// Provides news to the player.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Produces("application/xml")]
    [Route("")]
    public class CourseController : GrimControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly GameServerOptions _gsOptions;
        private readonly CourseDBManager _courses;
        private readonly UserDBManager _users;

        public CourseController(PlayerManager players, 
            CourseDBManager courses,
            UserDBManager users,
            IOptions<GameServerOptions> options, 
            ILogger<CourseController> logger)
            : base(players)
        {
            _logger = logger;
            _gsOptions = options.Value;
            _users = users;
            _courses = courses;
        }

        [HttpPost]
        [Route("/ap/[controller]")]
        public async Task<ActionResult> Post()
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

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
                    return await OnGetList(requestReq, player);
                case "course.update":
                    return await OnUpdateCourse(requestReq, player);
            }

            _logger.LogDebug("<- Got unknown course command: {command}", requestReq.Command);
            var badReqs = GrimResult.FromInt(-1);
            return BadRequest(badReqs);
        }

        [HttpGet]
        [Route("/[controller]/data/{courseId:long}.dat")]
        public async Task GetTrack(long courseId)
        {
            string tedFile = $"course/data/{courseId}.dat";
            await this.SendFile(_gsOptions.XmlResourcePath, tedFile);
        }

        private async Task<ActionResult> OnGetList(GrimRequest request, Player player)
        {
            if (!request.TryGetParameterByKey("user_id", out var userIdParam) || userIdParam.Text.Length > 32)
            {
                _logger.LogWarning("Got course getlist without 'user_id'");
                return BadRequest();
            }

            var user = await _users.GetByPSNUserIdAsync(player.Data.PSNUserId);
            var courses = await _courses.GetAllCoursesOfUser(user.Id);

            var courseList = new CourseList();
            courseList.Courses = new List<Course>();

            var course = new Course()
            {
                Comment = "Track Comment",
                CourseId = 1001000,
                Height = 200,
                OneWay = 0,
                Status = 2,
                Straight = 200,
                Title = "Track Title",
                Theme = "scenery_andalusia",
                Length = 7878,
                OwnerId = player.Data.PSNUserId,
                Corners = 69,
                OriginalCreator = player.Data.PSNUserId,
                PhotoId = 12345678,
                UpdateTime = DateTime.Now,
                PhotoHidden = 0,
            };

            courseList.Courses.Add(course);
            return Ok(courseList);
        }

        private async Task<ActionResult> OnUpdateCourse(GrimRequest request, Player player)
        {
            if (!request.TryGetParameterByKey("course_id", out var courseIdParam))
            {
                _logger.LogWarning("Got course getlist without 'course_id'");
                return BadRequest();
            }

            if (!request.TryGetParameterByKey("status", out var statusParam))
            {
                _logger.LogWarning("Got course getlist without 'status'");
                return BadRequest();
            }

            return Ok();
        }
    }
}