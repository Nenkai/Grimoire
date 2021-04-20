using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using GTGrimServer.Models;
using GTGrimServer.Database.Tables;

namespace GTGrimServer.Services
{
    /// <summary>
    /// Handles all the currently connected players to the server.
    /// </summary>
    public class PlayerManager
    {
        public ConcurrentDictionary<string, Player> Players { get; set; } = new();

        public PlayerManager()
        {
            
        }

        public bool RemoveByToken(SessionToken sToken)
            => Players.TryRemove(sToken.Token, out _);

        public bool AddByToken(SessionToken sToken, Player player)
            => Players.TryAdd(sToken.Token, player);

        public bool RemovePlayer(Player player)
            => Players.TryRemove(player.Token.Token, out _);

        public bool AddUser(Player player)
            => Players.TryAdd(player.Token.Token, player);

        public Player GetPlayerByToken(string sToken)
        {
            if (Players.TryGetValue(sToken, out Player player))
                return player;

            return null; // TODO Handle
        }

        public void UpdatePlayerToken(Player player, SessionToken newToken)
        {
            if (Players.TryRemove(player.Token.Token, out _))
            {
                player.Token = newToken;
                Players.TryAdd(player.Token.Token, player);
            }
        }

        public Player GetPlayerByName(string name)
            => Players.FirstOrDefault(e => e.Value.Data.PSNUserId.Equals(name)).Value;
    }
}
