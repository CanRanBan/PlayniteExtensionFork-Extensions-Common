﻿using System.Collections.Generic;
using ProtoBuf;

namespace UplayLibrary.Models
{
    [ProtoContract]
    public class UplayCacheGame
    {
        [ProtoMember(1)] public uint UplayId { get; set; }
        [ProtoMember(2)] public uint InstallId { get; set; }
        [ProtoMember(3)] public string GameInfo { get; set; }
    }

    [ProtoContract]
    public class UplayCacheGameCollection
    {
        [ProtoMember(1)] public List<UplayCacheGame> Games { get; set; }
    }
}
