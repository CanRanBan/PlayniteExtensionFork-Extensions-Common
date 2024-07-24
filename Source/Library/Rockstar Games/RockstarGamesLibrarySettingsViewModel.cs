using Playnite.SDK;

namespace RockstarGamesLibrary
{
    public class RockstarGamesLibrarySettings
    {
    }

    public class RockstarGamesLibrarySettingsViewModel : PluginSettingsViewModel<RockstarGamesLibrarySettings, RockstarGamesLibrary>
    {
        public RockstarGamesLibrarySettingsViewModel(RockstarGamesLibrary library, IPlayniteAPI api) : base(library, api)
        {
        }
    }
}
