// Copyright (C) 2015-2024 The EpicChain Project.
//
// Constants.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace EpicChain.BlockchainToolkit
{
    public static class Constants
    {
        public const string JSON_EXTENSION = ".json";

        public const string EXPRESS_EXTENSION = ".epicchain-express";
        public const string DEFAULT_EXPRESS_FILENAME = "default" + EXPRESS_EXTENSION;

        public const string EXPRESS_BATCH_EXTENSION = ".batch";
        public const string DEFAULT_BATCH_FILENAME = "default" + EXPRESS_BATCH_EXTENSION;
        public const string DEFAULT_SETUP_BATCH_FILENAME = "setup" + EXPRESS_BATCH_EXTENSION;

        public const string DEAULT_POLICY_FILENAME = "default-policy" + JSON_EXTENSION;

        public const string WORKNET_EXTENSION = ".neo-worknet";
        public const string DEFAULT_WORKNET_FILENAME = "default" + WORKNET_EXTENSION;


        public static readonly IReadOnlyList<string> MAINNET_RPC_ENDPOINTS = new[]
        {
            "http://mainnet1-seed.epic-chain.org:10111",
            "http://mainnet2-seed.epic-chain.org:10111",
            "http://mainnet3-seed.epic-chain.org:10111",
            "http://mainnet4-seed.epic-chain.org:10111",
            "http://mainnet5-seed.epic-chain.org:10111"
        };

        public static readonly IReadOnlyList<string> TESTNET_RPC_ENDPOINTS = new[]
        {
            "http://testnet1-seed.epic-chain.org:20111",
            "http://testnet2-seed.epic-chain.org:20111",
            "http://testnet3-seed.epic-chain.org:20111",
            "http://testnet4-seed.epic-chain.org:20111",
            "http://testnet5-seed.epic-chain.org:20111"
        };
    }
}
