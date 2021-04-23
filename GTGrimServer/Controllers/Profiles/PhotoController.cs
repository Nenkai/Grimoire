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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

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
    /// Handles sharing photos between friends.
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

        [HttpGet]
        [Route("/photo/image/{args}.jpg")]
        public async Task<ActionResult> GetPhoto(string args)
        {
            Player player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            string[] spl = args.Split('_');
            if (spl.Length != 2)
                return BadRequest();

            if (!long.TryParse(spl[0], out long photoId) || !int.TryParse(spl[1], out int type)) // 0 is actual image, 1 is thumbnail - for now we just send the same for both
                return BadRequest();

            if (type is not 0 or 1)
                return BadRequest();

            int? authorId = await _photoDb.GetAuthorIdOfPhotoAsync(photoId);
            if (authorId is null)
                return NotFound();

            if (!await _friendsDb.IsFriendedToUser(player.Data.Id, authorId.Value))
                return Forbid();

            if (!System.IO.File.Exists($"{_gsOptions.XmlResourcePath}/photo/image/{photoId}_0.jpg"))
                return NotFound();

            using var fs = new FileStream($"{_gsOptions.XmlResourcePath}/photo/image/{photoId}_0.jpg", FileMode.Open);
            return File(fs, "image/jpeg");
        }

        [HttpPost]
        [RequestSizeLimit(GTConstants.MaxPhotoSize)]
        [Route("/ap/[controller]/upload")]
        public async Task<ActionResult> OnRequestUploadImage()
        {
            Player player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            var value = Request.Headers["X-gt-xml"];
            if (value.Count != 1)
                return BadRequest();

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

            // Make sure they don't already have more photos that we allow, gets big quick
            if (await _photoDb.GetPhotoCountOfUserAsync(player.Data.Id) >= GTConstants.MaxPhotos)
                return Forbid();

            using var ms = Program.StreamManager.GetStream();
            await Request.Body.CopyToAsync(ms);
            ms.Position = 0;

            // Check the image itself
            if (!await VerifyImage(ms, GTConstants.MaxPhotoWidth, GTConstants.MaxPhotoHeight))
                return BadRequest();

            if (type != 3)
                return BadRequest();

            PhotoDTO photo = new PhotoDTO(Player.Data.Id, DateTime.Now, comment.Text, car_name.Text, place.Text);
            long newId = await _photoDb.AddAsync(photo);

            Directory.CreateDirectory($"{_gsOptions.XmlResourcePath}/photo/image");

            ms.Position = 0;
            using (var fs = new FileStream($"{_gsOptions.XmlResourcePath}/photo/image/{newId}_0.jpg", FileMode.Create))
                await ms.CopyToAsync(fs);

            // To let the client aware of the photo's online id
            var result = GrimResult.FromLong(newId);
            return Ok(result);
        }

        /// <summary>
        /// Fired when the player deletes an image, or can also be used for the player 
        /// to sync their local images to the server and removing the ones that the player no longer has.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Verifies that the image being uploaded is suitable to be.
        /// </summary>
        /// <param name="stream">Stream containing the image.</param>
        /// <param name="maxWidth">Max photo width.</param>
        /// <param name="maxHeight">Max photo height.</param>
        /// <returns></returns>
        private async Task<bool> VerifyImage(Stream stream, int maxWidth, int maxHeight)
        {
            var image = await Image.IdentifyWithFormatAsync(stream);
            if (image.Format is null)
                return false;

            if (image.Format is not JpegFormat)
                return false;

            if (image.ImageInfo.Width > maxWidth || image.ImageInfo.Height > maxHeight)
                return false;

            return true;
        }
    }
}
