using System;
using Playnite.SDK;

namespace HumbleLibrary
{
    public class HumbleLibraryClient : LibraryClient
    {
        public override bool IsInstalled => true;

        public override void Open()
        {
            throw new NotImplementedException();
        }
    }
}
