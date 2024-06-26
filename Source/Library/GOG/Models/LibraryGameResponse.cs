﻿using System;

namespace GogLibrary.Models
{
    public class LibraryGameResponse
    {
        public class Game
        {
            public string id;
            public bool achievementSupport;
            public string image;
            public string title;
            public string url;
        }

        public class Stats
        {
            public DateTime? lastSession;
            public int playtime;
        }

        public Game game;

        public object stats;
    }
}
