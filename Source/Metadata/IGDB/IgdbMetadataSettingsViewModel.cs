using System.ComponentModel;
using Playnite.SDK;

namespace IGDBMetadata
{
    public enum MultiImagePriority
    {
        [Description("LOCFirst")]
        First,
        [Description("LOCRandom")]
        Random,
        [Description("LOCUserSelect")]
        Select
    }

    public class IgdbMetadataSettings
    {
        public bool UseScreenshotsIfNecessary { get; set; } = false;
        public MultiImagePriority ImageSelectionPriority { get; set; } = MultiImagePriority.First;
        public bool UseCoverAsIcon { get; set; } = false;
    }

    public class IgdbMetadataSettingsViewModel : PluginSettingsViewModel<IgdbMetadataSettings, IgdbMetadataPlugin>
    {
        public IgdbMetadataSettingsViewModel(IgdbMetadataPlugin plugin, IPlayniteAPI api) : base(plugin, api)
        {
            var savedSettings = LoadSavedSettings();
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new IgdbMetadataSettings();
            }
        }
    }
}
