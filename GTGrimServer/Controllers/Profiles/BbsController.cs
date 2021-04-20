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
using GTGrimServer.Services;
using GTGrimServer.Models;
using GTGrimServer.Models.Xml;
using GTGrimServer.Filters;
using GTGrimServer.Results;
using GTGrimServer.Database.Controllers;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Controllers
{
    /// <summary>
    /// GT5 handler for community message boards.
    /// </summary>
    [ApiController]
    [PDIClient]
    [Authorize]
    [Route("/ap/[controller]")]
    [Produces("application/xml")]
    public class BbsController : GrimControllerBase
    {
        private readonly ILogger<BbsController> _logger;
        private readonly GameServerOptions _gsOptions;

        private readonly UserDBManager _userDb;
        private readonly BbsBoardDBManager _bbsDb;

        public BbsController(PlayerManager players, 
            UserDBManager userDb,
            BbsBoardDBManager bbsDb,
            IOptions<GameServerOptions> options, ILogger<BbsController> logger)
            : base(players)
        {
            _logger = logger;
            _gsOptions = options.Value;

            _userDb = userDb;
            _bbsDb = bbsDb;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var player = Player;
            if (player is null)
            {
                _logger.LogWarning("Could not get current player for host {host}", Request.Host);
                return Unauthorized();
            }

            if (_gsOptions.GameType != GameType.GT5)
            {
                _logger.LogWarning("Got a bbs request on a non GT5 server from host: {host}", Request.Host);
                return Unauthorized();
            }

            GrimRequest gRequest = await GrimRequest.Deserialize(Request.Body);
            if (gRequest is null)
            {
                // Handle
                var badReq = GrimResult.FromInt(-1);
                return BadRequest(badReq);
            }

            _logger.LogDebug("<- {command}", gRequest.Command);

            switch (gRequest.Command)
            {
                case "bbs.getCommentList":
                    return await OnRequestGetCommentList(player, gRequest);
                case "bbs.updateComment":
                    return await OnRequestUpdateComment(player, gRequest);
                case "bbs.deleteComment":
                    return await OnRequestDeleteComment(player, gRequest);
            }

            _logger.LogDebug("Received unimplemented bbs call: {command}", gRequest.Command);
            var res = GrimResult.FromInt(-1);
            return BadRequest(res);
        }

        /// <summary>
        /// Fired when the player requests a message board.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ActionResult> OnRequestGetCommentList(Player player, GrimRequest request)
        {
            if (!request.TryGetParameterByKey("bbs_board_id", out var bbsBoardIdParam) || !int.TryParse(bbsBoardIdParam.Text, out int bbs_board_id))
            {
                _logger.LogWarning($"Got bbs.getCommentList request with missing 'bbs_board_id' parameter");
                return BadRequest();
            }

            if (!request.TryGetParameterByKey("bbs_comment_id", out var commentIdParam) || !int.TryParse(commentIdParam.Text, out int bbs_comment_id))
            {
                _logger.LogWarning($"Got bbs.getCommentList request with missing 'bbs_comment_id' parameter");
                return BadRequest();
            }

            // Bbs Board ids is just the user number - get the user using it
            var user = await _userDb.GetByIDAsync(bbs_board_id);
            if (user is null)
                BadRequest();

            var entries = await _bbsDb.GetAllCommentsOfBoard(bbs_board_id);
            var commentList = new BbsCommentList();

            foreach (var entry in entries)
            {
                if (entry.Id <= bbs_comment_id)
                    continue;

                string creatorUserId = player.Data.PSNUserId;
                if (entry.AuthorId != player.Data.Id)
                {
                    creatorUserId = await _userDb.GetPSNNameByIdAsync(entry.AuthorId);
                    if (creatorUserId is null)
                        break;
                }

                BbsComment comment = new BbsComment();
                comment.BoardId = bbs_board_id;
                comment.Comment = entry.Comment;
                comment.CreateTime = entry.CreateTime;
                comment.Nickname = creatorUserId;
                comment.UserId = creatorUserId;
                comment.CommentId = entry.Id;
                commentList.Comments.Add(comment);
            }

            return Ok(commentList);
        }

        /// <summary>
        /// Fired when the player posts a new comment.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ActionResult> OnRequestUpdateComment(Player player, GrimRequest request)
        {
            if (!request.TryGetParameterByKey("bbs_board_id", out var bbsBoardIdParam) || !int.TryParse(bbsBoardIdParam.Text, out int bbs_board_id))
            {
                _logger.LogWarning($"Got bbs.updateComment request with missing 'bbs_board_id' parameter");
                return BadRequest();
            }

            if (!request.TryGetParameterByKey("comment", out var comment))
            {
                _logger.LogWarning($"Got bbs.updateComment request with missing 'bbs_comment_id' parameter");
                return BadRequest();
            }

            if (comment.Text.Length == 0 || comment.Text.Length > 140)
            {
                _logger.LogWarning("Got bbs.updateComment with empty or too long comment? In: {length}, Max: 30", comment.Text.Length);
                return BadRequest();
            }

            if (comment.Text == "cock and ball torture on wikipedia, the free encyclopedia that anyone can edit")
                return new ConsoleBanResult();

            // Bbs Board ids is just the user number - get the user using it
            var user = await _userDb.GetByIDAsync(bbs_board_id);
            if (user is null)
                BadRequest();

            var newComment = new BbsDTO(bbs_board_id, comment.Text, DateTime.Now);
            await _bbsDb.AddAsync(newComment);
            
            return Ok(GrimResult.FromBool(true));
        }

        /// <summary>
        /// Fired when the player deletes a comment from their board.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ActionResult> OnRequestDeleteComment(Player player, GrimRequest request)
        {
            if (!request.TryGetParameterByKey("bbs_comment_id", out var commentIdParam) || !int.TryParse(commentIdParam.Text, out int bbs_comment_id))
            {
                _logger.LogWarning($"Got bbs.deleteComment request with missing 'bbs_comment_id' parameter");
                return BadRequest();
            }

            // Get current user's board
            var comment = await _bbsDb.GetByIDAsync(bbs_comment_id);
            if (comment is null || comment.BbsBoardId != player.Data.Id)
                return Forbid();

            await _bbsDb.RemoveAsync(bbs_comment_id);
            return Ok(GrimResult.FromBool(true));
        }
    }
}