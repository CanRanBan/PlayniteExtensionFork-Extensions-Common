using System.Collections.Generic;
using Playnite.SDK.Models;

namespace BattleNetLibrary.Models
{
    public enum BNetAppType
    {
        Default,
        Classic
    }
    public class BNetApp
    {
        public string ProductId;
        public string InternalId;
        public string IconUrl;
        public string BackgroundUrl;
        public string CoverUrl;
        public string Name;
        public BNetAppType Type;
        public string ClassicExecutable;
        public List<Link> Links;
        public long ApiId;
    }
}
