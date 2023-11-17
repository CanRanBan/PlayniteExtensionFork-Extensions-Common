using Playnite.SDK.Models;

namespace OriginLibrary.Models
{
    public class OriginGameMetadata : GameMetadata
    {
        public GameStoreDataResponse StoreDetails { get; set; }
        public StorePageMetadata StoreMetadata { get; set; }
    }
}
