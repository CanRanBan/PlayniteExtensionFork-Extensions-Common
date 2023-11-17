namespace ItchioLibrary.Models
{
    /// <summary>
    /// Ask the client to perform an URL launch, ie. open an address with the system browser or appropriate.
    /// Sent during Launch.
    /// </summary>
    public class URLLaunch
    {
        /// <summary>
        /// URL to open, e.g. https://itch.io/community
        /// </summary>
        public string url;
    }
}
