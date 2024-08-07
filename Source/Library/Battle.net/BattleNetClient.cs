﻿using System.Diagnostics;
using System.Linq;
using Playnite.Common;
using Playnite.SDK;

namespace BattleNetLibrary
{
    public class BattleNetClient : LibraryClient
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public override string Icon => BattleNet.Icon;

        public override bool IsInstalled => BattleNet.IsInstalled;

        public override void Open()
        {
            BattleNet.StartClient();
        }

        public override void Shutdown()
        {
            var mainProc = Process.GetProcessesByName("Battle.net").FirstOrDefault();
            if (mainProc == null)
            {
                logger.Info("Battle.net client is no longer running, no need to shut it down.");
                return;
            }

            ProcessStarter.StartProcessWait(BattleNet.ClientExecPath, "--exec=shutdown", null);
        }
    }
}
