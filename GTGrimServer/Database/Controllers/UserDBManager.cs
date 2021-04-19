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

        /// <summary>
        /// Gets an user by PSN Id.
        /// </summary>
        /// <param name="psnId">PSN Id of the user.</param>
        /// <returns></returns>
        public async Task<UserDTO> GetByPSNIdAsync(long psnId)
            => await _con.QueryFirstOrDefaultAsync<UserDTO>(@"SELECT * FROM users WHERE psnid = @psnid", new { PsnId = psnId });

        /// <summary>
        /// Gets the PSN Name of an user by Id.
        /// </summary>
        /// <param name="id">Database Id of the user.</param>
        /// <returns></returns>
        public async Task<string> GetPSNNameByIdAsync(long id)
            => await _con.QueryFirstOrDefaultAsync<string>(@"SELECT psnname FROM users WHERE id = @id", new { Id = id });

        public async Task UpdateAsync(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users", pData);

        /// <summary>
        /// Updates the user with the current nickname and available nickname changes.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateNewNickname(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users SET nickname=@Nickname, nickname_changes=@NicknameChanges WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user with a new welcome message.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateWelcomeMessage(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users SET welcomemessage=@WelcomeMessage WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user with helmet data.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateHelmet(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users SET helmet=@HelmetId, helmet_color=@HelmetColorId WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user with wear data.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateWear(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users SET wear=@WearId, wear_color=@WearColorId WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user with menu data (GT5).
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateHomeDesign(UserDTO pData)
            => await _con.ExecuteAsync(@"UPDATE users SET menu_color=@MenuColor, menu_matiere=@MenuMatiere WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user's game stats.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateGameStats(UserDTO pData)
            => await _con.ExecuteAsync("UPDATE users SET license_level=@LicenseLevel, achievement=@AchievementCount, trophy=@TrophyCount, " +
                "car_count=@CarCount, license_gold=@LicenseGoldCount, odometer=@Odometer WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user's profile details.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateOnlineProfile(UserDTO pData)
            => await _con.ExecuteAsync("UPDATE users SET profile_level=@ProfileLevel, playtime_level=@PlaytimeLevel, comment_level=@CommentLevel, " +
                "playtime=@Playtime, comment=@Comment WHERE id = @Id", pData);

        /// <summary>
        /// Updates the user's home profile.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public async Task UpdateMyHomeProfile(UserDTO pData)
            => await _con.ExecuteAsync("UPDATE users SET license_level=@LicenseLevel, achievement=@AchievementCount, trophy=@TrophyCount, " +
                "car_count=@CarCount, license_gold=@LicenseGoldCount, odometer=@Odometer, win_count=@WinCount, license_silver=@LicenseSilverCount, " +
                "license_bronze=@LicenseBronzeCount, aspec_level=@ASpecLevel, bspec_level=@BSpecLevel, aspec_exp=@ASpecExp, bspec_exp=@BSpecExp, " +
                "credit=@Credit WHERE id = @Id", pData);

        public async Task<long> AddAsync(UserDTO pData)
        {
            var query =
@"INSERT INTO users (psnid, psnname, ipaddress, mac, country, nickname)
  VALUES(@PsnId, @PSNName, @IPAddress, @MacAddress, @Country, @Nickname)
  returning id";
            
            return await _con.ExecuteScalarAsync<long>(query, new { pData.PsnId, pData.PSNName, pData.IPAddress, 
                pData.MacAddress, pData.Country, pData.Nickname });
        }

        public async Task RemoveAsync(long id)
            => await _con.ExecuteAsync(@"DELETE FROM users WHERE id=@Id", new { Id = id });

        public async Task RemoveByPSNIdAsync(long psnId)
            => await _con.ExecuteAsync(@"DELETE FROM users WHERE psnid=@Id", new { PsnId = psnId });

        private void CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
				psnid BIGINT UNIQUE,
                psnname TEXT,
                ipaddress TEXT,
                country TEXT,
                mac TEXT,
                nickname_changes INTEGER DEFAULT 3,

                aspec_level INTEGER DEFAULT 0,
                aspec_exp INTEGER DEFAULT 0,
                bspec_level INTEGER DEFAULT 0,
                bspec_exp INTEGER DEFAULT 0,
                achievement INTEGER DEFAULT 0,
                credit BIGINT DEFAULT 0,
                win_count INTEGER DEFAULT 0,
                car_count INTEGER DEFAULT 0,
                trophy INTEGER DEFAULT 0,
                odometer REAL DEFAULT 0,
                license_level INTEGER DEFAULT 0,
                license_gold INTEGER DEFAULT 0,
                license_silver INTEGER DEFAULT 0,
                license_bronze INTEGER DEFAULT 0,

                helmet INTEGER DEFAULT 0,
                helmet_color INTEGER DEFAULT 0,
            
                wear INTEGER DEFAULT 0,
                wear_color INTEGER DEFAULT 0,

                comment TEXT,
                nickname TEXT,
                photo_id_avatar TEXT,
                photo_bg TEXT,
                band_test INTEGER DEFAULT 0,
                band_up INTEGER DEFAULT 0,
                band_down INTEGER DEFAULT 0,
                band_update_time TIMESTAMP WITHOUT TIME ZONE,
                
                menu_color INTEGER DEFAULT 0,
                menu_matiere INTEGER DEFAULT 0,

                profile_level INTEGER DEFAULT 0,

                playtime TEXT,
                playtime_level INTEGER DEFAULT 0,

                comment TEXT,
                comment_level INTEGER DEFAULT 0,

                welcomemessage TEXT,
                tag INTEGER DEFAULT 0
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
