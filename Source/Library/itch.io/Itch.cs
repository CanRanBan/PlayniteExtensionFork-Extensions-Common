﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ItchioLibrary.Models;
using Playnite.Common;
using Playnite.SDK.Data;

namespace ItchioLibrary
{
    public class Itch
    {
        public static string UserPath
        {
            get
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, "itch");
            }
        }

        public static string PrereqsPaths
        {
            get { return Path.Combine(UserPath, "prereqs"); }
        }

        public static string InstallationPath
        {
            get
            {
                var prog = Programs.GetUnistallProgramsList().FirstOrDefault(a => a.DisplayName == "itch");
                if (prog == null) // Try default path
                {
                    var defPath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "itch");
                    if (File.Exists(Path.Combine(defPath, "state.json")))
                    {
                        return defPath;
                    }
                }

                return prog == null ? string.Empty : prog.InstallLocation;
            }
        }

        public static string ClientExecPath
        {
            get
            {
                var installDir = InstallationPath;
                if (string.IsNullOrEmpty(installDir))
                {
                    return string.Empty;
                }

                var statePath = Path.Combine(InstallationPath, "state.json");
                if (!File.Exists(statePath))
                {
                    return string.Empty;
                }

                var instState = Serialization.FromJson<ItchInstallState>(File.ReadAllText(statePath));
                var exePath = Path.Combine(InstallationPath, $"app-{instState.current}", "itch.exe");
                return File.Exists(exePath) ? exePath : string.Empty;
            }
        }

        public static bool IsInstalled
        {
            get { return InstalledVersion.Major >= 25; }
        }

        public static Version InstalledVersion
        {
            get
            {
                var statePath = Path.Combine(InstallationPath, "state.json");
                if (!File.Exists(statePath))
                {
                    return new Version();
                }

                var instState = Serialization.FromJson<ItchInstallState>(File.ReadAllText(statePath));
                return new Version(instState.current);
            }
        }

        public static string Icon => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"Resources", @"ItchIoLibraryIcon.ico");

        public static void StartClient()
        {
            ProcessStarter.StartProcess(ClientExecPath, string.Empty);
        }
    }
}
