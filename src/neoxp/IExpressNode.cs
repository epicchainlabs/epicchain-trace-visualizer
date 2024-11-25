// Copyright (C) 2015-2024 The EpicChain Project.
//
// IExpressNode.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain;
using EpicChain.Cryptography.ECC;
using EpicChain.Network.P2P.Payloads;
using EpicChain.Network.RPC.Models;
using EpicChain.SmartContract;
using EpicChain.SmartContract.Manifest;
using EpicChain.SmartContract.Native;
using EpicChain.VM;
using EpicChain.Wallets;
using NeoExpress.Commands;
using NeoExpress.Models;
using System.Numerics;

namespace NeoExpress
{
    interface IExpressNode : IDisposable
    {
        ProtocolSettings ProtocolSettings { get; }

        enum CheckpointMode { Online, Offline }

        Task<CheckpointMode> CreateCheckpointAsync(string checkPointPath);

        Task<RpcInvokeResult> InvokeAsync(Script script, Signer? signer = null);
        Task<UInt256> ExecuteAsync(Wallet wallet, UInt160 accountHash, WitnessScope witnessScope, Script script, decimal additionalGas = 0);
        Task<UInt256> SubmitOracleResponseAsync(OracleResponse response, IReadOnlyList<ECPoint> oracleNodes);
        Task FastForwardAsync(uint blockCount, TimeSpan timestampDelta);

        Task<Block> GetBlockAsync(UInt256 blockHash);
        Task<Block> GetBlockAsync(uint blockIndex);
        Task<ContractManifest> GetContractAsync(UInt160 scriptHash);
        Task<Block> GetLatestBlockAsync();
        Task<(Transaction tx, RpcApplicationLog? appLog)> GetTransactionAsync(UInt256 txHash);
        Task<uint> GetTransactionHeightAsync(UInt256 txHash);

        Task<IReadOnlyList<(TokenContract contract, BigInteger balance)>> ListBalancesAsync(UInt160 address);
        Task<IReadOnlyList<(UInt160 hash, ContractManifest manifest)>> ListContractsAsync();
        Task<IReadOnlyList<(ulong requestId, OracleRequest request)>> ListOracleRequestsAsync();
        Task<IReadOnlyList<(string key, string value)>> ListStoragesAsync(UInt160 scriptHash);
        Task<IReadOnlyList<TokenContract>> ListTokenContractsAsync();

        Task<int> PersistContractAsync(ContractState state, IReadOnlyList<(string key, string value)> storagePairs, ContractCommand.OverwriteForce force);
        Task<int> PersistStorageKeyValueAsync(UInt160 scripthash, (string key, string value) storagePair);
        IAsyncEnumerable<(uint blockIndex, NotificationRecord notification)> EnumerateNotificationsAsync(IReadOnlySet<UInt160>? contractFilter, IReadOnlySet<string>? eventFilter);

        Task<bool> IsNep17CompliantAsync(UInt160 contractHash);
        Task<bool> IsNep11CompliantAsync(UInt160 contractHash);
    }
}
