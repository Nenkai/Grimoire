﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using GTGrimServer.Filters;
using GTGrimServer.Sony;
using GTGrimServer.Models;
using GTGrimServer.Utils;
using GTGrimServer.Database.Tables;

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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId}.xml")]
        public ActionResult Get(long userId)
        {
            var user = new UserProfile();
            return Ok(user);
        }

    }
}
