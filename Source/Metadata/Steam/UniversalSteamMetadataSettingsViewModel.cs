using Playnite.SDK;
using SteamLibrary.SteamShared;

namespace UniversalSteamMetadata
{
    public class UniversalSteamMetadataSettings : SharedSteamSettings
    {
    }


    public class
        UniversalSteamMetadataSettingsViewModel : SharedSteamSettingsViewModel<UniversalSteamMetadataSettings,
        UniversalSteamMetadata>
    {
        public UniversalSteamMetadataSettingsViewModel(UniversalSteamMetadata plugin, IPlayniteAPI api) : base(plugin,
            api)
        {
        }
    }
}
