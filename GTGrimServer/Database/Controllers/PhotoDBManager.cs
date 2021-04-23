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
    public class PhotoDBManager : IDBManager<PhotoDTO>
    {
        private ILogger<PhotoDBManager> _logger;
        protected IDbConnection _con;

        public PhotoDBManager(ILogger<PhotoDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<PhotoDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<PhotoDTO>(@"SELECT * FROM photos WHERE id = @Id", new { Id = id });

        /// <summary>
        /// Gets the database id of the user that created an image by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int?> GetAuthorIdOfPhotoAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<int?>(@"SELECT user_id FROM photos WHERE id = @Id", new { Id = id });

        /// <summary>
        /// Gets the count of photos that an user has.
        /// </summary>
        /// <param name="id">Database id of the user's id.</param>
        /// <returns></returns>
        public async Task<int?> GetPhotoCountOfUserAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<int?>(@"SELECT COUNT(id) FROM photos WHERE user_id = @UserId", new { UserId = id });

        /// <summary>
        /// Gets all the photos of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns>Course object list.</returns>
        public async Task<IEnumerable<PhotoDTO>> GetAllPhotosOfUser(long userId)
            => await _con.QueryAsync<PhotoDTO>(@"SELECT * FROM photos WHERE user_id=@UserId", new { UserId = userId });

        public async Task UpdateAsync(PhotoDTO pData)
            => await _con.ExecuteAsync(@"UPDATE photos WHERE id=@Id", pData);

        public async Task<long> AddAsync(PhotoDTO pData)
        {
            var query =
@"INSERT INTO photos (user_id, create_time, comment, car_name, place)
  VALUES
(@UserId, @CreateTime, @Comment, @CarName, @Place)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { pData.UserId, pData.CreateTime, pData.Comment, pData.CarName, pData.Place });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM photos WHERE id=@Id", new { Id = id });

        /// <summary>
        /// Removes a photo of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <param name="photoId">Database Id of the photo.</param>
        /// <returns></returns>
        public async Task RemovePhotoAsync(long userId, long photoId)
           => await _con.ExecuteAsync(@"DELETE FROM photos WHERE user_id=@UserId AND id=@Id", new { UserId = userId, Id = photoId });

        public void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS photos (
                id BIGSERIAL PRIMARY KEY,
				user_id BIGINT REFERENCES users(id),
                create_time TIMESTAMP WITHOUT TIME ZONE,
                comment TEXT,
                car_name TEXT,
                place TEXT
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS photos_user_id_idx ON photos (user_id)";
            _con.Execute(query2);
        }
    }
}
