// Copyright (C) 2015-2024 The EpicChain Project.
//
// WorknetFile.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.BlockchainToolkit.Models;
using EpicChain.Wallets;

namespace NeoWorkNet.Models;

public record WorknetFile(
    Uri Uri,
    BranchInfo BranchInfo,
    Wallet ConsensusWallet);
