﻿using System;

namespace Papper.Extensions.Metadata
{
    public class BlockMetaData
    {
        public string Name { get; internal set; }
        public string Selector { get; internal set; }
        public string? Version { get; set; }
        public DateTime LastCodeChange { get; set; }
        public DateTime LastInterfaceChange { get; set; }
        public uint Checksum { get; set; }
        public uint CodeSize { get; set; }
        public BlockType BlockType { get; set; }

        public BlockMetaData(string name, string selector)
        {
            Name = name ?? ExceptionThrowHelper.ThrowArgumentNullException<string>(nameof(name));
            Selector = selector ?? string.Empty;
        }
    }
}
