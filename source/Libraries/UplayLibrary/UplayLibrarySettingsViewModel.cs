using Playnite.SDK;

namespace UplayLibrary
{
    public class UplayLibrarySettings
    {
        public bool ImportInstalledGames { get; set; } = true;
        public bool ImportUninstalledGames { get; set; } = Uplay.IsInstalled;
    }

    public class UplayLibrarySettingsViewModel : PluginSettingsViewModel<UplayLibrarySettings, UplayLibrary>
    {
        public UplayLibrarySettingsViewModel(UplayLibrary library, IPlayniteAPI api) : base(library, api)
        {
            var savedSettings = LoadSavedSettings();
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new UplayLibrarySettings();
            }
        }
    }
}
