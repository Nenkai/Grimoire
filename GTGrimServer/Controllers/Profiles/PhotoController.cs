using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using GTGrimServer.Config;
using GTGrimServer.Services;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Filters;
using GTGrimServer.Utils;
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
    public class PhotoController : GrimControllerBase
    {
        private readonly ILogger<PhotoController> _logger;
        private readonly GameServerOptions _gsOptions;
        private readonly UserDBManager _userDb;
        private readonly FriendDBManager _friendsDb;
        private readonly PhotoDBManager _photoDb;

        public PhotoController(PlayerManager players,
            IOptions<GameServerOptions> options, 
            ILogger<PhotoController> logger,
            UserDBManager userDb,
            FriendDBManager friendsDb,
            PhotoDBManager photoDb)
            : base(players)
        {
            _logger = logger;
            _gsOptions = options.Value;
            _userDb = userDb;
            _friendsDb = friendsDb;
            _photoDb = photoDb;
        }

        [HttpPost]
        [Route("/ap/photo")]
        public async Task<ActionResult> Post(string server)
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            GrimRequest gRequest = await GrimRequest.Deserialize(Request.Body);
            if (gRequest is null)
                return BadRequest();

            _logger.LogDebug("<- {command}", gRequest.Command);

            switch (gRequest.Command)
            {
                case "photo.deleteimage":
                    return await OnRequestDeleteImage(player, gRequest);
            }

            _logger.LogDebug("Got unimplemented photo call: {command}", gRequest.Command);


            return BadRequest();
        }

        [HttpGet]
        [Route("/photo/list/{userId}.xml")]
        public async Task<ActionResult> GetUserPhotoList(string userId)
        {
            Player player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            var targetId = await _userDb.GetInternalIdOfUserAsync(userId);
            if (targetId is null)
                return NotFound();

            if (!await _friendsDb.IsFriendedToUser(player.Data.Id, targetId.Value))
                return Forbid();

            var photos = await _photoDb.GetAllPhotosOfUser(targetId.Value);

            var list = new PhotoList();
            foreach (var photo in photos)
            {
                var photoModel = new Photo();
                photoModel.PhotoId = photo.Id;
                photoModel.CarName = photo.CarName;
                photoModel.Comment = photo.Comment;
                photoModel.CreateTime = photo.CreateTime;
                photoModel.Place = photo.Place;
                photoModel.UserId = userId;
                list.Photos.Add(photoModel);
            }

            return Ok(list);
        }

        [HttpPost]
        [Route("/ap/[controller]/upload")]
        public async Task<ActionResult> OnRequestUploadImage()
        {
            if (Player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            var value = Request.Headers["X-gt-xml"];
            if (value.Count != 1)
                return BadRequest();

            if (Request.ContentLength > 1_024_000)
                return StatusCode(StatusCodes.Status413PayloadTooLarge);

            string xml = value[0];
            if (string.IsNullOrEmpty(xml))
                return BadRequest();

            GrimRequest gRequest = GrimRequest.Deserialize(xml);
            if (gRequest is null)
                return BadRequest();

            if (gRequest.Command != "photo.upload")
                return BadRequest();

            if (!gRequest.TryGetParameterByKey("place", out var place))
            {
                _logger.LogWarning($"Got photo.upload request with missing 'place' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("car_name", out var car_name))
            {
                _logger.LogWarning($"Got photo.upload request with missing 'car_name' parameter");
                return BadRequest();
            }
            if (!gRequest.TryGetParameterByKey("comment", out var comment))
            {
                _logger.LogWarning($"Got photo.upload request with missing 'comment' parameter");
                return BadRequest();
            }

            if (comment.Text.Length > 140)
            {
                _logger.LogWarning("Received photo.upload request with too long 'comment' parameter (len {comment.Text.Length} > 140)", comment.Text.Length);
                return BadRequest();
            }

            // type 3 is regular photo upload, 1 is avatar
            if (!gRequest.TryGetParameterIntByKey("type", out int type))
            {
                _logger.LogWarning($"Got photo.upload request with missing 'type' parameter");
                return BadRequest();
            }

            if (type != 3)
                return BadRequest();

            PhotoDTO photo = new PhotoDTO(Player.Data.Id, DateTime.Now, comment.Text, car_name.Text, place.Text);
            long newId = await _photoDb.AddAsync(photo);

            using (var fs = new FileStream($"Resources/photo/image/{newId}.jpg", FileMode.Create))
                await Response.Body.CopyToAsync(fs);

            // To let the client aware of the photo's online id
            var result = GrimResult.FromLong(newId);
            return Ok(result);
        }

        private async Task<ActionResult> OnRequestDeleteImage(Player player, GrimRequest gRequest)
        {
            if (Player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            if (!gRequest.TryGetParameterLongByKey("photo_id", out long photo_id))
            {
                _logger.LogWarning($"Got photo.upload request with missing 'photo_id' parameter");
                return BadRequest();
            }

            var photo = await _photoDb.GetByIDAsync(photo_id);
            if (photo is null)
                return NotFound();

            if (photo.UserId != player.Data.Id)
                return Forbid();

            await _photoDb.RemoveAsync(photo_id);

            return Ok(GrimResult.FromBool(true));
        }
    }
}
