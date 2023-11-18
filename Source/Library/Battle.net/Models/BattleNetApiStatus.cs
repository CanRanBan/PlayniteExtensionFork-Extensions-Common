namespace BattleNetLibrary.Models
{
    public class BattleNetApiStatus
    {
        public class AccountCompletion
        {
            public bool requiresHealup;
            public string accountCountry;
            public string completionUrl;
        }

        public AccountCompletion accountCompletion;
        public string logoutUri;
        public string loginUri;
        public bool authenticated;
    }
}
