// Copyright (C) 2015-2024 The EpicChain Project.
//
// ContractCoverage.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Collections.Generic;

namespace EpicChain.Collector.Models
{
    class ContractCoverage
    {
        public readonly string Name;
        public readonly EpicChainDebugInfo DebugInfo;
        public readonly IReadOnlyDictionary<int, Instruction> InstructionMap;
        public readonly IReadOnlyDictionary<int, uint> HitMap;
        public readonly IReadOnlyDictionary<int, (uint BranchCount, uint ContinueCount)> BranchHitMap;

        public ContractCoverage(string name, EpicChainDebugInfo debugInfo, IReadOnlyDictionary<int, Instruction> instructionMap, IReadOnlyDictionary<int, uint> hitMap, IReadOnlyDictionary<int, (uint, uint)> branchHitMap)
        {
            Name = name;
            InstructionMap = instructionMap;
            DebugInfo = debugInfo;
            HitMap = hitMap;
            BranchHitMap = branchHitMap;
        }
    }
}
