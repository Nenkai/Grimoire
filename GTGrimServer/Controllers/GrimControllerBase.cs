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

using GTGrimServer.Sony;
using GTGrimServer.Filters;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Config;
using GTGrimServer.Controllers;
using GTGrimServer.Services;

namespace GTGrimServer.Controllers
{
    public class GrimControllerBase : ControllerBase
    {
        protected readonly PlayerManager Players;

        public GrimControllerBase(PlayerManager players)
        {
            Players = players;
        }

        /// <summary>
        /// Current player for this request.
        /// </summary>
        public Player Player
        {
            get
            {
                string token = Request.Cookies["X-gt-token"];
                if (token is null)
                    return null;

                Player player = Players.GetPlayerByToken(token);

                // TODO: Check if expired

                return player;
            }
        }
    }
}
