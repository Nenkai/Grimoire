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
using GTGrimServer.Database.Tables;
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
        private readonly FriendDBManager _friendsDb;

        public UserController(PlayerManager players, 
            ILogger<UserController> logger, 
            UserDBManager userDb,
            FriendDBManager friendsDb)
            : base(players)
        {
            _logger = logger;
            _userDb = userDb;
            _friendsDb = friendsDb;
        }

        /// <summary>
        /// General user fetching.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}.xml")]
        public async Task<ActionResult> Get(string userId)
        {
            var currentPlayer = Player;
            if (currentPlayer is null)
            {
                _logger.LogWarning("Unable to get current player for host '{host}'", Request.Host);
                return Unauthorized();
            }

            UserProfile userProfile;

            // Is it self?
            if (currentPlayer?.Data?.PSNUserId?.Equals(userId) is true)
            {
                userProfile = UserProfile.FromDatabaseObject(currentPlayer.Data);
            }
            else
            {
                // Possible friend - Check if they're a friend before allowing to get their profile
                UserDTO userData = await _userDb.GetByPSNUserIdAsync(userId);
                if (!await _friendsDb.IsFriendedToUser(currentPlayer.Data.Id, userData.Id))
                    return Forbid();

                userProfile = UserProfile.FromDatabaseObject(userData);
            }

            userProfile.BandUp = 1024;
            userProfile.BandDown = 1024;
            userProfile.BandUpdateTime = DateTime.Now.ToRfc3339String();
            userProfile.BandTest = 1024;
            return Ok(userProfile);
        }
    }
}
