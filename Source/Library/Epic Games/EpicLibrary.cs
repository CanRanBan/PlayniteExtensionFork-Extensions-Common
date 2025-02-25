﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EpicLibrary.Services;
using Playnite.Common;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

namespace EpicLibrary
{
    [LoadPlugin]
    public class EpicLibrary : LibraryPluginBase<EpicLibrarySettingsViewModel>
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        internal readonly string TokensPath;

        public EpicLibrary(IPlayniteAPI api) : base(
            "Epic Games",
            Guid.Parse("00000002-DBD1-46C6-B5D0-B1BA559D10E4"),
            new LibraryPluginProperties { CanShutdownClient = true, HasSettings = true },
            new EpicClient(),
            EpicLauncher.Icon,
            (_) => new EpicLibrarySettingsView(),
            api)
        {
            SettingsViewModel = new EpicLibrarySettingsViewModel(this, api);
            TokensPath = Path.Combine(GetPluginUserDataPath(), "tokens.json");
        }

        internal Dictionary<string, GameMetadata> GetInstalledGames()
        {
            var games = new Dictionary<string, GameMetadata>();
            var appList = EpicLauncher.GetInstalledAppList();
            var manifests = EpicLauncher.GetInstalledManifests();

            foreach (var app in appList)
            {
                if (app.AppName.StartsWith("UE_"))
                {
                    continue;
                }

                var manifest = manifests.FirstOrDefault(a => a.AppName == app.AppName);
                if (manifest == null)
                {
                    continue;
                }

                // DLC
                if (manifest.AppName != manifest.MainGameAppName &&
                    (manifest.AppCategories?.Any(a => a == "addons/launchable") == false))
                {
                    continue;
                }

                // UE plugins
                if (manifest.AppCategories?.Any(a => a == "plugins" || a == "plugins/engine") == true ||
                    manifest.CompatibleApps?.Any(a => a.StartsWith("UE_")) == true ||
                    manifest.TechnicalType?.Contains("plugins/engine") == true)
                {
                    continue;
                }

                var gameName = manifest?.DisplayName ?? Path.GetFileName(app.InstallLocation);

                // Looks like manifest location can be not valid if a workaround to import existing game installation
                // is used and also the game was previously installed into different location.
                // App list seems to have correct location so it should be preffered value.
                var installLocation = app.InstallLocation;
                if (installLocation.IsNullOrEmpty() || !Directory.Exists(installLocation))
                {
                    installLocation = manifest?.InstallLocation;
                }

                if (installLocation.IsNullOrEmpty() || !Directory.Exists(installLocation))
                {
                    continue;
                }

                installLocation = Paths.FixSeparators(installLocation);
                if (!Directory.Exists(installLocation))
                {
                    logger.Error($"Epic game {gameName} installation directory {installLocation} not detected.");
                    continue;
                }

                var game = new GameMetadata()
                {
                    Source = new MetadataNameProperty("Epic"),
                    GameId = app.AppName,
                    Name = gameName,
                    InstallDirectory = installLocation,
                    IsInstalled = true,
                    Platforms = new HashSet<MetadataProperty> { new MetadataSpecProperty("pc_windows") }
                };

                game.Name = game.Name.RemoveTrademarks();
                games.Add(game.GameId, game);
            }

            return games;
        }

        internal List<GameMetadata> GetLibraryGames(CancellationToken cancelToken)
        {
            var cacheDir = GetCachePath("catalogcache");
            var games = new List<GameMetadata>();
            var accountApi = new EpicAccountClient(PlayniteApi, TokensPath);
            var assets = accountApi.GetAssets();
            if (!assets?.Any() == true)
            {
                Logger.Warn("Found no assets on Epic accounts.");
            }

            var playtimeItems = accountApi.GetPlaytimeItems();
            foreach (var gameAsset in assets.Where(a => a.@namespace != "ue"))
            {
                if (cancelToken.IsCancellationRequested)
                {
                    break;
                }

                var cacheFile =
                    Paths.GetSafePathName(
                        $"{gameAsset.@namespace}_{gameAsset.catalogItemId}_{gameAsset.buildVersion}.json");
                cacheFile = Path.Combine(cacheDir, cacheFile);
                var catalogItem = accountApi.GetCatalogItem(gameAsset.@namespace, gameAsset.catalogItemId, cacheFile);
                if (catalogItem?.categories?.Any(a => a.path == "applications") != true)
                {
                    continue;
                }

                if ((catalogItem?.mainGameItem != null) &&
                    (catalogItem.categories?.Any(a => a.path == "addons/launchable") == false))
                {
                    continue;
                }

                if (catalogItem?.categories?.Any(a => a.path == "digitalextras" || a.path == "plugins" || a.path == "plugins/engine") == true)
                {
                    continue;
                }

                if ((catalogItem?.customAttributes?.ContainsKey("ThirdPartyManagedApp") == true) && (catalogItem?.customAttributes["ThirdPartyManagedApp"].value.ToLower() == "the ea app"))
                {
                    if (!SettingsViewModel.Settings.ImportEAGames)
                    {
                        continue;
                    }
                }

                if ((catalogItem?.customAttributes?.ContainsKey("partnerLinkType") == true) &&
                    (catalogItem.customAttributes["partnerLinkType"].value == "ubisoft"))
                {
                    if (!SettingsViewModel.Settings.ImportUbisoftGames)
                    {
                        continue;
                    }
                }

                var newGame = new GameMetadata
                {
                    Source = new MetadataNameProperty("Epic"),
                    GameId = gameAsset.appName,
                    Name = catalogItem.title.RemoveTrademarks(),
                    Platforms = new HashSet<MetadataProperty> { new MetadataSpecProperty("pc_windows") }
                };

                var playtimeItem = playtimeItems?.FirstOrDefault(x => x.artifactId == gameAsset.appName);
                if (playtimeItem != null)
                {
                    newGame.Playtime = playtimeItem.totalTime;
                }

                games.Add(newGame);
            }

            return games;
        }

        public override IEnumerable<GameMetadata> GetGames(LibraryGetGamesArgs args)
        {
            var allGames = new List<GameMetadata>();
            var installedGames = new Dictionary<string, GameMetadata>();
            Exception importError = null;

            if (SettingsViewModel.Settings.ImportInstalledGames)
            {
                try
                {
                    installedGames = GetInstalledGames();
                    Logger.Debug($"Found {installedGames.Count} installed Epic games.");
                    allGames.AddRange(installedGames.Values.ToList());
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to import installed Epic games.");
                    importError = e;
                }
            }

            if (SettingsViewModel.Settings.ConnectAccount)
            {
                try
                {
                    var libraryGames = GetLibraryGames(args.CancelToken);
                    Logger.Debug($"Found {libraryGames.Count} library Epic games.");

                    if (!SettingsViewModel.Settings.ImportUninstalledGames)
                    {
                        libraryGames = libraryGames.Where(lg => installedGames.ContainsKey(lg.GameId)).ToList();
                    }

                    foreach (var game in libraryGames)
                    {
                        if (installedGames.TryGetValue(game.GameId, out var installed))
                        {
                            installed.Playtime = game.Playtime;
                            installed.LastActivity = game.LastActivity;
                            installed.Name = game.Name;
                        }
                        else
                        {
                            allGames.Add(game);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to import linked account Epic games details.");
                    importError = e;
                }
            }

            if (importError != null)
            {
                PlayniteApi.Notifications.Add(new NotificationMessage(
                    ImportErrorMessageId,
                    string.Format(PlayniteApi.Resources.GetString("LOCLibraryImportError"), Name) +
                    System.Environment.NewLine + importError.Message,
                    NotificationType.Error,
                    () => OpenSettingsView()));
            }
            else
            {
                PlayniteApi.Notifications.Remove(ImportErrorMessageId);
            }

            return allGames;
        }

        public string GetCachePath(string dirName)
        {
            return Path.Combine(GetPluginUserDataPath(), dirName);
        }

        public override IEnumerable<InstallController> GetInstallActions(GetInstallActionsArgs args)
        {
            if (args.Game.PluginId != Id)
            {
                yield break;
            }

            yield return new EpicInstallController(args.Game);
        }

        public override IEnumerable<UninstallController> GetUninstallActions(GetUninstallActionsArgs args)
        {
            if (args.Game.PluginId != Id)
            {
                yield break;
            }

            yield return new EpicUninstallController(args.Game);
        }

        public override IEnumerable<PlayController> GetPlayActions(GetPlayActionsArgs args)
        {
            if (args.Game.PluginId != Id)
            {
                yield break;
            }

            yield return new AutomaticPlayController(args.Game)
            {
                Type = AutomaticPlayActionType.Url,
                TrackingMode = TrackingMode.Directory,
                TrackingPath = args.Game.InstallDirectory,
                Path = string.Format(EpicLauncher.GameLaunchUrlMask, args.Game.GameId),
                Name = ResourceProvider.GetString(LOC.EpicStartUsingClient).Format("Epic")
            };
        }

        public override LibraryMetadataProvider GetMetadataDownloader()
        {
            return new EpicMetadataProvider(PlayniteApi);
        }
    }
}
