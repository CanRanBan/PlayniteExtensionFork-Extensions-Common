using Playnite.SDK.Data;

namespace SteamLibrary.SteamShared
{
    public class TagIdCategory
    {
        [SerializationPropertyName("Id")]
        public int Id { get; set; }

        [SerializationPropertyName("Category")]
        public TagCategory Category { get; set; }
    }

    public enum TagCategory
    {
        Assessments,
        Features,
        FundingEtc,
        Genres,
        HardwareInput,
        OtherTags,
        Players,
        RatingsEtc,
        Software,
        SubGenres,
        ThemesMoods,
        TopLevelGenres,
        VisualsViewpoint
    };
}
