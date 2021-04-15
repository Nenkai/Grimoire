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
    public class UserDBManager : IDBManager<UserDTO>
    {
        private ILogger<UserDBManager> _logger;
        protected IDbConnection _con;

        public UserDBManager(ILogger<UserDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<UserDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<UserDTO>(@"SELECT * FROM users WHERE id = @id", new { Id = id });

        public async Task<UserDTO> GetByPSNIdAsync(long psnId)
            => await _con.QueryFirstOrDefaultAsync<UserDTO>(@"SELECT * FROM users WHERE psnid = @psnid", new { PsnId = psnId });

        public async Task UpdateAsync(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users", pData);

        public async Task<long> AddAsync(UserDTO pData)
        {
            var query =
@"INSERT INTO users (psnid, nickname, ipaddress, mac)
  VALUES(@PsnId, @Nickname, @IPAddress, @MacAddress)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { pData.PsnId, pData.Nickname, pData.IPAddress, pData.MacAddress });
        }

        public async Task RemoveAsync(ulong id)
            => await _con.ExecuteAsync(@"DELETE FROM users WHERE id=@Id", new { Id = id });

        public async Task RemoveByPSNIdAsync(ulong psnId)
            => await _con.ExecuteAsync(@"DELETE FROM users WHERE psnid=@Id", new { PsnId = psnId });

        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
				psnid BIGINT UNIQUE,
                nickname TEXT,
                ipaddress TEXT,
                mac TEXT,

                aspec_level INTEGER DEFAULT 0,
                aspec_exp INTEGER DEFAULT 0,
                bspec_level INTEGER DEFAULT 0,
                bspec_exp INTEGER DEFAULT 0,
                achievement INTEGER DEFAULT 0,
                credit BIGINT DEFAULT 0,
                win_count INTEGER DEFAULT 0,
                car_count INTEGER DEFAULT 0,
                trophy INTEGER DEFAULT 0,
                odometer INTEGER DEFAULT 0,
                license_level INTEGER DEFAULT 0,
                license_gold INTEGER DEFAULT 0,
                license_silver INTEGER DEFAULT 0,
                license_bronze INTEGER DEFAULT 0,

                helmet INTEGER DEFAULT 0,
                helmet_color INTEGER DEFAULT 0,
            
                wear INTEGER DEFAULT 0,
                wear_color INTEGER DEFAULT 0
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS users_psnid_idx ON users (psnid)";
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
