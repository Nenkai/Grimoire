using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;


using GTGrimServer.Controllers;
using GTGrimServer.Database.Controllers;
using GTGrimServer.Database.Tables;
using GTGrimServer.Filters;
using GTGrimServer.Config;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Sony;

namespace GTGrimServer.Controllers.Profiles
{
    [ApiController]
    [PDIClient]
    [Route("/ap/ticket/login/{pfsVersion}/")]
    [Produces("application/xml")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _config;
        private readonly GameServerOptions _gsOptions;
        private readonly UserDBManager _users;

        public AuthenticationController(IConfiguration config, 
            ILogger<AuthenticationController> logger,
            UserDBManager users)
        {
            _logger = logger;
            _config = config;
            _gsOptions = _config.GetSection(GameServerOptions.GameServer).Get<GameServerOptions>();
            _users = users;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ulong pfsVersion)
        {
            PFSType pfsType = (PFSType)pfsVersion;
            _logger.LogDebug("Login request from {host} with PFS: {pfsType} ({pfsNum})", Request.Host, pfsType, (long)pfsType);

            if (!EnsureVersion(pfsType))
                return Forbid();

            if (Request.ContentLength >= 0x300)
            {
                _logger.LogWarning("Received a ticket too big - {size} from {host}", Request.ContentLength, Request.Host);
                return BadRequest();
            }

            NPTicket ticket;
            try
            {
                byte[] buf = new byte[(int)Request.ContentLength];
                await Request.Body.ReadAsync(buf.AsMemory(0, buf.Length));
                ticket = NPTicket.FromBuffer(buf);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Could not read NP ticket provided by client");
                return BadRequest();
            }

            if (!VerifyTicket(ticket))
                return BadRequest();

            _logger.LogDebug("Auth Request: NP_Ticket -> PFS: {pfsVersion} | OnlineID: {OnlineId} | Region: {Region}", pfsVersion, ticket.OnlineId, ticket.Region);
            User user = _users.GetByID((long)ticket.UserId) ?? CreateUser(ticket);
            if (user is null)
            {
                _logger.LogError("Failed to get or create user from db: Name: {name}, PSN Id {psnId}", ticket.OnlineId, ticket.UserId);

                return Problem();    
            }

            var resp = new TicketResult()
            {
                Result = "0", // Doesn't seem to do much.
                Nickname = ticket.OnlineId,
                UserId = ticket.UserId,
                UserNumber = user.Id.ToString(),
                ServerTime = DateTime.Now.ToRfc3339String(),
            };

            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddHours(1) };
            Response.Cookies.Append("X-gt-token", GenerateToken(), cookieOptions);

            return Ok(resp);
        }

        /// <summary>
        /// Checks if the game version provided is suitable for the server.
        /// </summary>
        /// <param name="pfs"></param>
        /// <returns></returns>
        [NonAction]
        private bool EnsureVersion(PFSType pfs)
        {
            if (_gsOptions.GameType == GameType.GT6)
            {
                if (pfs < PFSType.GT6_V1_22)
                {
                    _logger.LogInformation("Client Fail: Received PFS type {type} (num) - expected GT6 1.22.", pfs, (long)pfs);
                    return false;
                }
            }

            return true;
        }

        [NonAction]
        private bool VerifyTicket(NPTicket ticket)
        {
            return true;
        }

        [NonAction]
        public User CreateUser(NPTicket ticket)
        {
            var user = new User()
            {
                PsnId = (long)ticket.UserId,
                IPAddress = Request.Host.Host,
                Nickname = ticket.OnlineId,
            };

            long id;
            try
            {
                id = _users.Add(user);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to create new user {userName} - PSNId: {psnId}", ticket.OnlineId, ticket.UserId);
                return null;
            }

            _logger.LogInformation("Created user {userName} - PSNId: {psnId} - ServId: {id}", ticket.OnlineId, ticket.UserId, id);
            return user;
        }

        [NonAction]
        public string GenerateToken()
        {
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, 
                expires: DateTime.Now.AddHours(1), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
