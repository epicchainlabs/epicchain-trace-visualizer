// Copyright (C) 2015-2024 The EpicChain Project.
//
// EpicChainDebugInfo.Parameter.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace EpicChain.Collector.Models
{
    public partial class EpicChainDebugInfo
    {
        public struct Parameter
        {
            public readonly string Name;
            public readonly string Type;
            public readonly int Index;

            public Parameter(string name, string type, int index)
            {
                Name = name;
                Type = type;
                Index = index;
            }
        }
    }
}
