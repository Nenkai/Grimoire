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
    public class BbsBoardDBManager : IDBManager<BbsDTO>
    {
        private ILogger<BbsBoardDBManager> _logger;
        protected IDbConnection _con;

        public BbsBoardDBManager(ILogger<BbsBoardDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<BbsDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<BbsDTO>(@"SELECT * FROM bbs WHERE id = @id", new { Id = id });

        public async Task UpdateAsync(BbsDTO pData)
            => await _con.ExecuteAsync(@"UPDATE bbs", pData);

        /// <summary>
        /// Gets all the comments in a board.
        /// </summary>
        /// <param name="id">Database Id of the board.</param>
        /// <returns>Bbs object list.</returns>
        public async Task<IEnumerable<BbsDTO>> GetAllCommentsOfBoard(int boardId)
            => await _con.QueryAsync<BbsDTO>(@"SELECT * FROM bbs WHERE bbs_board_id=@Id", new { Id = boardId });

        public async Task<long> AddAsync(BbsDTO bbs)
        {
            var query =
@"INSERT INTO bbs (bbs_board_id, author_id, comment, create_time)
  VALUES(@BbsBoardId, @AuthorId, @Comment, @CreateTime)
  returning id";
            
            return await _con.ExecuteScalarAsync<long>(query, new { bbs.BbsBoardId, bbs.AuthorId, bbs.Comment, bbs.CreateTime });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM bbs WHERE id=@Id", new { Id = id });

        public async Task RemoveByBoardIdAsync(int boardId)
            => await _con.ExecuteAsync(@"DELETE FROM bbs WHERE bbs_board_id=@Id", new { Id = boardId });

        public void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS bbs (
                id SERIAL PRIMARY KEY,
                bbs_board_id INTEGER REFERENCES users(id),
                
                author_id INTEGER REFERENCES users(id),
                comment TEXT,
                create_time TIMESTAMP WITHOUT TIME ZONE
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS bbs_bbsboardid_idx ON bbs (bbs_board_id)";
            _con.Execute(query2);
        }
    }
}
