using Playnite.SDK;

namespace ItchioLibrary
{
    public class ItchioClient : LibraryClient
    {
        public override string Icon => Itch.Icon;

        public override bool IsInstalled => Itch.IsInstalled;

        public override void Open()
        {
            Itch.StartClient();
        }
    }
}
