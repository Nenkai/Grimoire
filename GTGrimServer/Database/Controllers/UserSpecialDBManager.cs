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
    public class UserSpecialDBManager : IDBManager<UserSpecialDTO>
    {
        private ILogger<UserSpecialDBManager> _logger;
        protected IDbConnection _con;

        public UserSpecialDBManager(ILogger<UserSpecialDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<UserSpecialDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<UserSpecialDTO>(@"SELECT * FROM user_specials WHERE id = @Id", new { Id = id });

        /// <summary>
        /// Gets all the specials of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <param name="type">Type of the special, GT6 always uses 3. -1 will retrieve all specials.</param>
        /// <returns>Special object list.</returns>
        public async Task<IEnumerable<UserSpecialDTO>> GetAllPresentsOfUserAsync(long dbUserId, int type = -1)
        {
            if (type == -1)
                return await _con.QueryAsync<UserSpecialDTO>(@"SELECT * FROM user_specials WHERE userid=@UserId AND type=@Type", new { UserId = dbUserId, Type = type });
            else
                return await _con.QueryAsync<UserSpecialDTO>(@"SELECT * FROM user_specials WHERE userid=@UserId", new { UserId = dbUserId });
        }

        public async Task UpdateAsync(UserSpecialDTO uSpecialData)
            => await _con.ExecuteAsync(@"UPDATE user_specials WHERE id=@Id", uSpecialData);

        public async Task<long> AddAsync(UserSpecialDTO uSpecialData)
        {
            var query =
@"INSERT INTO user_specials (userid, type, key, value)
  VALUES(@UserId, @Type, @Key, @Value)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { uSpecialData.UserId, uSpecialData.Type, uSpecialData.Key, uSpecialData.Value });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM user_specials WHERE id=@id", new { Id = id });

        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS user_specials (
                id SERIAL PRIMARY KEY,
				userid BIGINT REFERENCES users(id),
                type INT,
                key TEXT,
                value TEXT
			);";
            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS user_specials_userid_idx ON user_specials (userid)";
            _con.Execute(query2);
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
