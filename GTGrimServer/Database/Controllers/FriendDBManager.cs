using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            => await _con.QueryAsync<FriendDTO>(@"SELECT * FROM friends WHERE id=@Id", new { Id = id });

        public async Task UpdateAsync(FriendDTO pData)
            => await _con.ExecuteAsync(@"UPDATE friends WHERE id=@Id AND friend_id=@FriendId", pData);

        public async Task<long> AddAsync(FriendDTO friendData)
        {
            var query =
@"INSERT INTO friends (user_id, friend_id)
  VALUES(@UserId, @FriendId)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { friendData.UserId, friendData.FriendId });
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
           => await _con.ExecuteAsync(@"DELETE FROM friends WHERE user_id=@UserId AND friend_id=@FriendId", new { UserId = userId, FriendId = friendId });

        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS friends (
                id SERIAL PRIMARY KEY,
				user_id BIGINT REFERENCES users(id),
                friend_id BIGINT REFERENCES users(id)
			);";
            _con.Execute(query);
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
