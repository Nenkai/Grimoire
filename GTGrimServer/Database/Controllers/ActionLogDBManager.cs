using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using System.Data;

using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;

using Microsoft.Extensions.DependencyInjection;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Database.Controllers
{
    public class ActionLogDBManager : IDBManager<ActionLogDTO>
    {
        private ILogger<ActionLogDBManager> _logger;
        protected IDbConnection _con;

        public ActionLogDBManager(ILogger<ActionLogDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<ActionLogDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<ActionLogDTO>(@"SELECT * FROM actionlogs WHERE id = @id", new { Id = id });

        public async Task UpdateAsync(ActionLogDTO pData)
            => await _con.ExecuteAsync(@"UPDATE actionlogs", pData);

        /// <summary>
        /// Gets all the actions of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns>Action object list.</returns>
        public async Task<IEnumerable<ActionLogDTO>> GetAllActionsOfUser(int userId)
            => await _con.QueryAsync<ActionLogDTO>(@"SELECT * FROM actionlogs WHERE user_id=@Id", new { Id = userId });

        public async Task<long> AddAsync(ActionLogDTO log)
        {
            var query =
@"INSERT INTO actionlogs (user_id, create_time, value1, value2, value3, value4, value5)
  VALUES(@UserId, @CreateTime, @Value1, @Value2, @Value3, @Value4, @Value5)
  returning id";
            
            return await _con.ExecuteScalarAsync<long>(query, new { log.UserId, log.CreateTime, log.Value1, log.Value2, log.Value3, log.Value4, log.Value5 });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM actionlogs WHERE id=@Id", new { Id = id });


        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS actionlogs (
                id SERIAL PRIMARY KEY,
                user_id INTEGER REFERENCES users(id),
                
                create_time TIMESTAMP WITHOUT TIME ZONE,
                value1 TEXT,
                value2 TEXT,
                value3 TEXT,
                value4 TEXT,
                value5 TEXT
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS actionlogs_user_id_idx ON actionlogs (user_id)";
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
