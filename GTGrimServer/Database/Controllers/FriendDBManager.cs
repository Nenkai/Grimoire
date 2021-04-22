using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;

using Microsoft.Extensions.DependencyInjection;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Database.Controllers
{
    public class FriendDBManager : IDBManager<FriendDTO>
    {
        private ILogger<FriendDBManager> _logger;
        protected IDbConnection _con;

        public FriendDBManager(ILogger<FriendDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<FriendDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<FriendDTO>(@"SELECT * FROM friends WHERE id = @Id", new { Id = id });

        /// <summary>
        /// Gets all the friends of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns>Friend object list.</returns>
        public async Task<IEnumerable<FriendDTO>> GetAllFriendsOfUser(long id)
            => await _con.QueryAsync<FriendDTO>(@"SELECT * FROM friends WHERE userid=@Id", new { Id = id });

        /// <summary>
        /// Returns whether an user is friended to another user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns>Friend object list.</returns>
        public async Task<bool> IsFriendedToUser(long userId, long targetId)
        {
            return await _con.ExecuteScalarAsync<bool>(@"SELECT 1 FROM friends WHERE userid = @UserId AND friendid = @FriendId", 
                new { UserId = userId, TargetId = targetId });
        }

        public async Task UpdateAsync(FriendDTO pData)
            => await _con.ExecuteAsync(@"UPDATE friends WHERE id=@Id AND friendid=@FriendId", pData);

        public async Task<long> AddAsync(FriendDTO friendData)
        {
            var query =
@"INSERT INTO friends (userid, friendid)
  VALUES(@UserId, @FriendId)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { friendData.UserId, friendData.FriendId });
        }

        /// <summary>
        /// Updates an user's PSN friend list.
        /// </summary>
        /// <param name="userid">Database Id of the user.</param>
        /// <param name="currentFriendList">List of user ids that are friends of the current user.</param>
        /// <returns></returns>
        public async Task UpdateFriendList(int userid, string[] currentFriendList)
        {
            var sb = new StringBuilder("DELETE FROM friends WHERE userid="); sb.Append(userid); sb.AppendLine(" AND friendid IN (");
            sb.Append("SELECT id FROM users WHERE psn_user_id NOT IN (");
            foreach (var friend in currentFriendList)
            {
                sb.Append('\''); sb.Append(friend.Replace("'", "''")); sb.Append('\'');
            }
            sb.Append(')'); sb.AppendLine();
            sb.Append(')');

            await _con.ExecuteAsync(sb.ToString());

            // Add all friends that aren't in
            var addQuery = @$"INSERT INTO friends (userid, friendid)
SELECT @UserId, id FROM users WHERE psn_user_id = ANY(@FriendListUserIds) AND id NOT IN (
	 SELECT friendid FROM friends WHERE userid=@UserId AND friendid != id
)";

            await _con.ExecuteAsync(addQuery, new { UserId = userid, FriendListUserIds = currentFriendList });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM friends WHERE id=@id", new { Id = id });

        /// <summary>
        /// Removes a friend of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <param name="friendId">Database Id of the friend.</param>
        /// <returns></returns>
        public async Task RemoveFriendAsync(long userId, long friendId)
           => await _con.ExecuteAsync(@"DELETE FROM friends WHERE userid=@UserId AND friendid=@FriendId", new { UserId = userId, FriendId = friendId });

        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS friends (
                id SERIAL PRIMARY KEY,
				userid BIGINT REFERENCES users(id),
                friendid BIGINT REFERENCES users(id)
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS friends_userid_idx ON friends (userid)";
            _con.Execute(query2);

            string query3 = @"CREATE INDEX IF NOT EXISTS friends_friendid_idx ON friends (friendid)";
            _con.Execute(query3);
        }

        public bool CreateTableIfNeeded()
        {
            try
            {
                CreateTable();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to create table if needed");
            }

            return false;
        }
    }
}
