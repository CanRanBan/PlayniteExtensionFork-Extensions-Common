using Playnite.SDK.Models;
using SteamKit2;

namespace Steam.Models
{
    public class SteamGameMetadata : GameMetadata
    {
        public KeyValue ProductDetails { get; set; }
        public StoreAppDetailsResult.AppDetails StoreDetails { get; set; }
        public AppReviewsResult.QuerySummary UserReviewDetails { get; set; }
    }
}
