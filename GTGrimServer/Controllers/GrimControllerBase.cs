using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Models;
using GTGrimServer.Results;
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

        private BanObjectResult Ban([ActionResultObjectValue] object value)
            => new BanObjectResult(value);

        private ConsoleBanObjectResult ConsoleBan([ActionResultObjectValue] object value)
            => new ConsoleBanObjectResult(value);

        private SuspendedObjectResult Suspended([ActionResultObjectValue] object value)
            => new SuspendedObjectResult(value);
    }
}
