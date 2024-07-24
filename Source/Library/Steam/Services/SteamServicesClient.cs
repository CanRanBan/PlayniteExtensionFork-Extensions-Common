using System;
using System.Collections.Generic;
using Playnite.SDK;
using Playnite.Services;
using SteamLibrary.Models;

namespace SteamLibrary.Services
{
    public class SteamServicesClient : BaseServicesClient
    {
        private readonly ILogger logger = LogManager.GetLogger();

        public SteamServicesClient(string endpoint, Version playniteVersion) : base(endpoint, playniteVersion)
        {
        }

        public List<GetOwnedGamesResult.Game> GetSteamLibrary(ulong userName, bool freeSub = false)
        {
            var url = "/steam/library/" + userName;
            if (freeSub)
            {
                url += "?freeSub=true";
            }

            return ExecuteGetRequest<List<GetOwnedGamesResult.Game>>(url);
        }
    }
}
