using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Logging;

using GTGrimServer.Database.Tables;
using GTGrimServer.Database.Controllers;

namespace GTGrimServer.Database
{
    public class DatabaseController
    {
        private UserDBManager _accDbManager;

        public IDbConnection Connection => new NpgsqlConnection(""); // Todo fill in conn string
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(ILogger<DatabaseController> logger)
        {
            _accDbManager = new UserDBManager(this);
            _logger = logger;
        }

        public bool CreateTablesIfNeeded()
        {
            try
            {
                GetAccountDB().CreateTableIfNeeded();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to create tables if needed");
            }

            return false;
        }

        public UserDBManager GetAccountDB()
            => _accDbManager;
    }
}
