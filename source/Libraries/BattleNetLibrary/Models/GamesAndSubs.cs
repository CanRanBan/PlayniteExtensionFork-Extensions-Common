using System.Collections.Generic;

namespace BattleNetLibrary.Models
{
    public class GamesAndSubs
    {
        public class GameAccount
        {
            public class AccountUniqueId
            {
                public long gameAccountId;
                public int gameServiceRegionId;
                public long programId;
            }

            public long titleId;
            public string localizedGameName;
            public string gameAccountName;
            public AccountUniqueId gameAccountUniqueId;
            public string gameAccountRegion;
            public string regionalGameFranchiseIconFilename;
            public string gameAccountStatus;
            public bool titleHasSubscriptions;
            public bool titleHasGameTime;
        }

        public List<GameAccount> gameAccounts;
    }
}
