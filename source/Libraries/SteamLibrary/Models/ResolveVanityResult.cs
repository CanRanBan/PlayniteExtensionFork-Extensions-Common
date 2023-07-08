namespace SteamLibrary.Models
{
    public class ResolveVanityResult
    {
        public class Response
        {
            public int success;
            public string steamid;
            public string message;
        }

        public Response response;
    }
}
