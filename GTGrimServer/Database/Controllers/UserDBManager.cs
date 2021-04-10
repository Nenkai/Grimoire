using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using Dapper;
using Npgsql;

using Microsoft.Extensions.DependencyInjection;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Database.Controllers
{
    public class UserDBManager : IDBManager<UserProfile>
    {
        private DatabaseController _db;

        public UserDBManager(DatabaseController db)
        {
            _db = db;
        }

        public UserProfile GetByID(long id)
        {
            using IDbConnection con = _db.Connection;
            con.Open();
            var cmd = new NpgsqlCommand();
            return con.QueryFirstOrDefault<UserProfile>(@"SELECT * FROM users WHERE psnid = @psnid", new { Id = id });
        }

        public void Update(UserProfile pData)
        {
            using IDbConnection con = _db.Connection;
            con.Open();
            con.Execute(@"UPDATE users", pData);
        }

        public void Add(UserProfile pData)
        {

        }

        public void Remove(ulong id)
        {
            using IDbConnection con = _db.Connection;
            con.Open();
            con.Execute(@"DELETE FROM users WHERE psnid=@Id", new { Id = id });
        }

        public void CreateTableIfNeeded()
        {
            string query = @"CREATE TABLE IF NOT EXISTS users (
						psnid SERIAL PRIMARY KEY NOT NULL,
					);";

            using IDbConnection con = _db.Connection;
            con.Open();
            con.Execute(query);
        }
    }
}
