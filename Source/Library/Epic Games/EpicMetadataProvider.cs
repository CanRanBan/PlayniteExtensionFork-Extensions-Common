﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Playnite.SDK;
using Playnite.SDK.Models;

namespace EpicLibrary
{
    public class EpicMetadataProvider : LibraryMetadataProvider
    {
        private readonly IPlayniteAPI api;

        public EpicMetadataProvider(IPlayniteAPI api)
        {
            this.api = api;
        }

        public override GameMetadata GetMetadata(Game game)
        {
            var gameInfo = new GameMetadata() { Links = new List<Link>() };

            // The old search api for metadata we use is completely broken and returns terrible results.
            // The new API actually checks if a browser makes requests so we would need to switch to a web view.
            // So this is TODO to update this in future. It's temporarily disabled to make sure we don't apply messed up results.

            //using (var client = new WebStoreClient())
            //{
            //    var catalogs = client.QuerySearch(game.Name).GetAwaiter().GetResult();
            //    if (catalogs.HasItems())
            //    {
            //        var catalog = catalogs.FirstOrDefault(a => a.title.Equals(game.Name, StringComparison.InvariantCultureIgnoreCase));
            //        if (catalog == null)
            //        {
            //            return null;
            //        }

            //        // Some games use completely different way in which the store gets game related data.
            //        // I don't understand why Epic doesn't have this unified, but the other way totally sucks,
            //        // so we are not going to support those games right now.
            //        if (catalog.productSlug.IsNullOrEmpty())
            //        {
            //            return null;
            //        }

            //        var product = client.GetProductInfo(catalog.productSlug).GetAwaiter().GetResult();
            //        if (product.pages.HasItems())
            //        {
            //            var page = product.pages.FirstOrDefault(a => a.type is string type && type == "productHome");
            //            if (page == null)
            //            {
            //                page = product.pages[0];
            //            }

            //            gameInfo.Developers = page.data.about.developerAttribution?.
            //                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
            //                Select(a => new MetadataNameProperty(a.Trim())).Cast<MetadataProperty>().ToHashSet();
            //            gameInfo.Publishers = page.data.about.publisherAttribution?.
            //                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
            //                Select(a => new MetadataNameProperty(a.Trim())).Cast<MetadataProperty>().ToHashSet();
            //            gameInfo.BackgroundImage = new MetadataFile(page.data.hero.backgroundImageUrl);
            //            gameInfo.Links.Add(new Link(
            //                api.Resources.GetString("LOCCommonLinksStorePage"),
            //                "https://www.epicgames.com/store/en-US/product/" + catalogs[0].productSlug));

            //            if (page.data.socialLinks.HasItems())
            //            {
            //                var links = page.data.socialLinks.
            //                    Where(a => a.Key.StartsWith("link") && !a.Value.IsNullOrEmpty()).
            //                    Select(a => new Link(a.Key.Replace("link", ""), a.Value)).ToList();
            //                if (links.HasItems())
            //                {
            //                    gameInfo.Links.AddRange(links);
            //                }
            //            }

            //            if (!page.data.about.description.IsNullOrEmpty())
            //            {
            //                gameInfo.Description = Markup.MarkdownToHtml(page.data.about.description);
            //            }
            //        }
            //    }
            //}

            //gameInfo.Links.Add(new Link("PCGamingWiki", @"http://pcgamingwiki.com/w/index.php?search=" + game.Name));

            // There's not icon available on Epic servers so we will load one from EXE
            if (game.IsInstalled && string.IsNullOrEmpty(game.Icon))
            {
                var manifest = EpicLauncher.GetInstalledManifests().FirstOrDefault(a => a.AppName == game.GameId);
                if (manifest != null)
                {
                    var exePath = Path.Combine(manifest.InstallLocation, manifest.LaunchExecutable);
                    if (File.Exists(exePath))
                    {
                        gameInfo.Icon = new MetadataFile(exePath);
                    }
                }
            }

            return gameInfo;
        }
    }
}
