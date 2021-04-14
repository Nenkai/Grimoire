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
        public ConcurrentDictionary<SessionToken, Player> Players { get; set; } = new();

        public PlayerManager()
        {
            
        }

        public bool RemoveByToken(SessionToken token)
            => Players.TryRemove(token, out _);

        public bool AddByToken(SessionToken token, Player player)
            => Players.TryAdd(token, player);

        public bool RemovePlayer(Player player)
            => Players.TryRemove(player.Token, out _);

        public bool AddUser(Player player)
            => Players.TryAdd(player.Token, player);

        public Player GetPlayerByToken(SessionToken token)
        {
            if (Players.TryGetValue(token, out Player player))
                return player;

            return null; // TODO Handle
        }

        public void UpdatePlayerToken(Player player, SessionToken newToken)
        {
            if (Players.TryRemove(player.Token, out _))
            {
                player.Token = newToken;
                Players.TryAdd(player.Token, player);
            }
        }

        public Player GetPlayerByName(string name)
            => Players.FirstOrDefault(e => e.Value.Data.Nickname.Equals(name)).Value;
    }
}
