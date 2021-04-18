using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Config
{
    public class GameServerOptions
    {
        public const string GameServer = "GameServer";

        public GameType GameType { get; set; }
        public string XmlResourcePath { get; set; }
        public bool EnforceGameVersion { get; set; }
    }
}
