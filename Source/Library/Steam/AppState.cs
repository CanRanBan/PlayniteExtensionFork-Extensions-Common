namespace SteamLibrary
{
    /// <summary>
    /// Represents overall game installation and running state.
    /// </summary>
    public class AppState
    {
        public bool Installed { get; set; }

        public bool Launching { get; set; }

        public bool Running { get; set; }

        public bool Installing { get; set; }

        public bool Uninstalling { get; set; }
    }
}
