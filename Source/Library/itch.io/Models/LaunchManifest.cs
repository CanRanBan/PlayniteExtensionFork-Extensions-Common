﻿using System.Collections.Generic;

namespace ItchioLibrary.Models
{
    public class LaunchManifest
    {
        public class Action
        {
            public string name { get; set; }
            public string path { get; set; }
            public string platform { get; set; }
            public List<string> args { get; set; }
        }

        public class Prerequisite
        {
            public string name { get; set; }
        }

        public List<Action> actions { get; set; }
        public List<Prerequisite> prereqs { get; set; }
    }
}
