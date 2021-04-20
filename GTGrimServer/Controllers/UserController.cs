using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Database.Controllers;
using GTGrimServer.Filters;
using GTGrimServer.Controllers;
using GTGrimServer.Services;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Utils;

namespace GTGrimServer.Helpers
{
    /// <summary>
    /// Handles profile related requests.
    /// </summary>
    [ApiController]
    [Authorize]
    [PDIClient]
    [Route("[controller]")]
    [Produces("application/xml")]
    public class UserController : GrimControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserDBManager _userDb;

        public UserController(PlayerManager players, ILogger<UserController> logger, UserDBManager userDb)
            : base(players)
        {
            _logger = logger;
            _userDb = userDb;
        }

        /// <summary>
        /// General user fetching.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}.xml")]
        public ActionResult Get(string userId)
        {
            var currentPlayer = Player;
            if (currentPlayer is null)
            {
                _logger.LogWarning("Unable to get current player for host '{host}'", Request.Host);
                return Unauthorized();
            }

            if (currentPlayer?.Data?.PSNUserId?.Equals(userId) is true)
            {
                var user = UserProfile.FromDatabaseObject(currentPlayer.Data);
                user.ProfileLevel = unchecked((int)0b_11111111_11111111_11111111_11111111);
                user.BandUp = 1024;
                user.BandDown = 1024;
                user.BandUpdateTime = DateTime.Now.ToRfc3339String();
                user.BandTest = 1024;
                return Ok(user);
            }
            else
            {
                // TODO: Get other user
                _logger.LogWarning("Got unexpected profile request for '{userId}'", userId);
                return BadRequest();
            }
        }
    }
}
