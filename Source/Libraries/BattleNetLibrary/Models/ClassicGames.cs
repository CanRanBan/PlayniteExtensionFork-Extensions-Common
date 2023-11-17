using System.Collections.Generic;

namespace BattleNetLibrary.Models
{
    public class ClassicGames
    {
        public class ClassicGame
        {
            public string localizedGameName;
            public string regionalGameFranchiseIconFilename;
            public List<string> cdKeys;
        }

        public List<ClassicGame> classicGames;
    }
}
