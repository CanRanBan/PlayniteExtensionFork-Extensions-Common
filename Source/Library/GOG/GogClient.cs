﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Playnite.Common;
using Playnite.SDK;

namespace GogLibrary
{
    public class GogClient : LibraryClient
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public override string Icon => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"Resources", @"GOGLibraryIcon.ico");

        public override bool IsInstalled => Gog.IsInstalled;

        public override void Open()
        {
            ProcessStarter.StartProcess(Gog.ClientExecPath, string.Empty);
        }

        public override void Shutdown()
        {
            var mainProc = Process.GetProcessesByName("GalaxyClient").FirstOrDefault();
            if (mainProc == null)
            {
                logger.Info("Galaxy client is no longer running, no need to shut it down.");
                return;
            }

            ProcessStarter.StartProcessWait(Gog.ClientExecPath, "/command=shutdown", null);
        }
    }
}
