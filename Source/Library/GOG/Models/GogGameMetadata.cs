using Playnite.SDK.Models;

namespace GogLibrary.Models
{
    public class GogGameMetadata : GameMetadata
    {
        public ProductApiDetail GameDetails { get; set; }
        public StorePageResult.ProductDetails StoreDetails { get; set; }
    }
}
