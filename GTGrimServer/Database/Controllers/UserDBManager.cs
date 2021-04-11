using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
    public class UserDBManager : IDBManager<User>
    {
        private ILogger<UserDBManager> _logger;
        protected IDbConnection _con;

        public UserDBManager(ILogger<UserDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public User GetByID(long id)
        {
            return _con.QueryFirstOrDefault<User>(@"SELECT * FROM users WHERE psnid = @psnid", new { Id = id });
        }

        public void Update(User pData)
        {
            _con.Execute(@"UPDATE users", pData);
        }

        public long Add(User pData)
        {
            var query =
@"INSERT INTO users (psnid, nickname, ipaddress, mac)
  VALUES(@PsnId, @Nickname, @IPAddress, @MacAddress)
  returning id";

            return _con.ExecuteScalar<long>(query, new { pData.PsnId, pData.Nickname, pData.IPAddress, pData.MacAddress });
        }

        public void Remove(ulong id)
        {
            _con.Execute(@"DELETE FROM users WHERE psnid=@Id", new { Id = id });
        }

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
