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
    public class CourseDBManager : IDBManager<CourseDTO>
    {
        private ILogger<CourseDBManager> _logger;
        protected IDbConnection _con;

        public CourseDBManager(ILogger<CourseDBManager> logger, IDbConnection con)
        {
            _logger = logger;
            _con = con;
        }

        public async Task<CourseDTO> GetByIDAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<CourseDTO>(@"SELECT * FROM courses WHERE id = @Id", new { Id = id });

        /// <summary>
        /// Gets all the courses of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns>Course object list.</returns>
        public async Task<IEnumerable<CourseDTO>> GetAllCoursesOfUser(long id)
            => await _con.QueryAsync<CourseDTO>(@"SELECT * FROM courses WHERE id=@Id", new { Id = id });

        public async Task UpdateAsync(CourseDTO pData)
            => await _con.ExecuteAsync(@"UPDATE courses WHERE id=@Id AND friendid=@FriendId", pData);

        public async Task<long> AddAsync(CourseDTO friendData)
        {
            var query =
@"INSERT INTO courses (user_id, create_time, update_time, status, photo_id, 
title, comment, title_hidden, comment_hidden, photo_hidden, theme, length, one_way, source_user_id)
  VALUES
(@UserId, @CreateTime, @UpdateTime, @Status, @PhotoId, @Title, @Comment, @TitleHidden, 
@CommentHidden, @PhotoHidden, @Theme, @Length, @OneWay, @SourceUserId, @Straight, @Height)
  returning id";

            return await _con.ExecuteScalarAsync<long>(query, new { friendData.UserId, friendData.FriendId });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM courses WHERE id=@id", new { Id = id });

        /// <summary>
        /// Removes a course of an user.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <param name="courseId">Database Id of the course.</param>
        /// <returns></returns>
        public async Task RemoveCourseAsync(long userId, long courseId)
           => await _con.ExecuteAsync(@"DELETE FROM courses WHERE userid=@UserId AND id=@Id", new { UserId = userId, Id = courseId });

        public void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS courses (
                id SERIAL PRIMARY KEY,
				user_id BIGINT REFERENCES users(id),
                create_time TIMESTAMP WITHOUT TIME ZONE,
                update_time TIMESTAMP WITHOUT TIME ZONE,
                status INTEGER,
                photo_id BIGINT,
                title TEXT,
                comment TEXT,
                title_hidden INTEGER,
                comment_hidden INTEGER,
                photo_hidden INTEGER,
                theme TEXT,
                one_way INTEGER,
                source_user_id BIGINT REFERENCES users(id),
                straight INTEGER,
                height INTEGER
			);";

            _con.Execute(query);

            string query2 = @"CREATE INDEX IF NOT EXISTS courses_user_id_idx ON courses (user_id)";
            _con.Execute(query2);
        }
    }
}
