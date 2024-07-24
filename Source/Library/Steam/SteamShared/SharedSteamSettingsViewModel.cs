using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

namespace SteamLibrary.SteamShared
{
    public abstract class SharedSteamSettingsViewModel<TSettings, TPlugin> : PluginSettingsViewModel<TSettings, TPlugin>
        where TSettings : SharedSteamSettings, new()
        where TPlugin : Plugin
    {
        private ObservableCollection<TagInfo> okayTags;
        private ObservableCollection<TagInfo> blacklistedTags;
        private string fixedTagCountString;
        internal readonly string ApiKeysPath;

        protected SharedSteamSettingsViewModel(TPlugin plugin, IPlayniteAPI playniteApi) : base(plugin, playniteApi)
        {
            ApiKeysPath = Path.Combine(plugin.GetPluginUserDataPath(), "keys.dat");
            var savedSettings = LoadSavedSettings();
            if (savedSettings != null)
            {
                Settings = savedSettings;
                OnLoadSettings();
            }
            else
            {
                Settings = new TSettings() { LanguageKey = GetSteamLanguageForCurrentPlayniteLanguage() };
                OnInitSettings();
            }

            InitializeSteamDeckCompatibilitySettings();
            InitializeTagNames();
            Settings.PropertyChanged += (sender, ev) =>
            {
                if (ev.PropertyName == nameof(Settings.LanguageKey)
                    || ev.PropertyName == nameof(Settings.UseTagPrefix)
                    || ev.PropertyName == nameof(Settings.TagPrefix)
                    || ev.PropertyName == nameof(Settings.SetTagCategoryAsPrefix))
                {
                    InitializeTagNames();
                }
            };
            FixedTagCountString = Settings.FixedTagCount.ToString();
        }

        private string GetSteamLanguageForCurrentPlayniteLanguage()
        {
            switch (PlayniteApi.ApplicationSettings.Language)
            {
                case "de_DE": return "german";
                case "en_US":
                default: return "english";
            }
        }

        /// <summary>
        /// Called after settings are successfully loaded
        /// </summary>
        protected virtual void OnLoadSettings()
        {
        }

        /// <summary>
        /// Called after settings are newly created (on first run, or after settings have been deleted)
        /// </summary>
        protected virtual void OnInitSettings()
        {
        }

        private void InitializeTagNames()
        {
            var tagNamer = new SteamTagNamer(Plugin, Settings, new Playnite.Common.Web.Downloader());
            var tags = tagNamer.GetTagNames()
                .Select(t => new TagInfo(t.Key, tagNamer.GetFinalTagName(t.Value, t.Key)))
                .OrderBy(t => t.Name).ToList();
            OkayTags = tags.Where(t => !Settings.BlacklistedTags.Contains(t.Id)).ToObservable();
            BlacklistedTags = tags.Where(t => Settings.BlacklistedTags.Contains(t.Id)).ToObservable();
        }

        public Dictionary<string, string> Languages { get; } = new Dictionary<string, string>
        {
            {"german","Deutsch (German)"},
            {"english","English"},
        };

        public class TagInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public TagInfo(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public ObservableCollection<TagInfo> OkayTags { get => okayTags; set => SetValue(ref okayTags, value); }
        public ObservableCollection<TagInfo> BlacklistedTags { get => blacklistedTags; set => SetValue(ref blacklistedTags, value); }
        public string FixedTagCountString { get => fixedTagCountString; set => SetValue(ref fixedTagCountString, value); }

        public RelayCommand<IList<object>> WhitelistCommand
        {
            get => new RelayCommand<IList<object>>((selectedItems) =>
            {
                var selectedKeyValuePairs = selectedItems.Cast<TagInfo>().ToList();
                foreach (var sel in selectedKeyValuePairs)
                {
                    BlacklistedTags.Remove(sel);
                    OkayTags.Add(sel);
                    Settings.BlacklistedTags.Remove(sel.Id);
                }
            }, (a) => a?.Count > 0);
        }

        public RelayCommand<IList<object>> BlacklistCommand
        {
            get => new RelayCommand<IList<object>>((selectedItems) =>
            {
                var selectedKeyValuePairs = selectedItems.Cast<TagInfo>().ToList();
                foreach (var sel in selectedKeyValuePairs)
                {
                    OkayTags.Remove(sel);
                    BlacklistedTags.Add(sel);
                    Settings.BlacklistedTags.Add(sel.Id);
                }
            }, (a) => a?.Count > 0);
        }

        public override bool VerifySettings(out List<string> errors)
        {
            if (Settings.LimitTagsToFixedAmount)
            {
                if (!int.TryParse(FixedTagCountString, out int fixedTagCount) || fixedTagCount < 0)
                {
                    errors = new List<string> { PlayniteApi.Resources.GetString(LOC.SteamValidationFixedTagCount) };
                    return false;
                }
            }

            return base.VerifySettings(out errors);
        }

        public override void EndEdit()
        {
            if (int.TryParse(FixedTagCountString, out int fixedTagCount) && fixedTagCount >= 0)
            {
                Settings.FixedTagCount = fixedTagCount;
            }

            base.EndEdit();
        }

        public class NamedField
        {
            public string Name { get; set; }
            public GameField Field { get; set; }
            public NamedField(string name, GameField field)
            {
                Name = name;
                Field = field;
            }
        }

        public List<NamedField> SteamDeckCompatibilityFieldOptions { get; set; }

        public void InitializeSteamDeckCompatibilitySettings()
        {
            SteamDeckCompatibilityFieldOptions = new List<NamedField>
            {
                new NamedField(PlayniteApi.Resources.GetString("LOCNone"), GameField.None),
                new NamedField(PlayniteApi.Resources.GetString("LOCFeatureLabel"), GameField.Features),
                new NamedField(PlayniteApi.Resources.GetString("LOCTagLabel"), GameField.Tags)
            };
        }
    }
}
