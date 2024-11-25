// Copyright (C) 2015-2024 The EpicChain Project.
//
// RunCommand.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using EpicChain;
using EpicChain.BlockchainToolkit.Persistence;
using EpicChain.BlockchainToolkit.Plugins;
using EpicChain.Cryptography.ECC;
using EpicChain.Persistence;
using EpicChain.Plugins;
using NeoWorkNet.Models;
using System.IO.Abstractions;
using System.Net;

namespace NeoWorkNet.Commands;

[Command("run", Description = "Run Neo-WorkNet instance node")]
partial class RunCommand
{
    readonly IFileSystem fs;

    public RunCommand(IFileSystem fs)
    {
        this.fs = fs;
    }

    [Option(Description = "Time between blocks")]
    internal uint? SecondsPerBlock { get; }

    internal async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console, CancellationToken token)
    {
        try
        {
            var (filename, worknet) = await fs.LoadWorknetAsync(app).ConfigureAwait(false);
            var dataDir = fs.GetWorknetDataDirectory(filename);
            if (!fs.Directory.Exists(dataDir))
                throw new Exception($"Cannot locate data directory {dataDir}");

            var secondsPerBlock = SecondsPerBlock ?? 0;
            await RunAsync(worknet, dataDir, secondsPerBlock, console, token).ConfigureAwait(false);
            return 0;
        }
        catch (Exception ex)
        {
            app.WriteException(ex);
            return 1;
        }
    }

    static ProtocolSettings GetProtocolSettings(WorknetFile worknetFile, uint secondsPerBlock = 0)
    {
        var account = worknetFile.ConsensusWallet.GetAccounts().Single();
        var key = account.GetKey() ?? throw new Exception();
        return ProtocolSettings.Default with
        {
            Network = worknetFile.BranchInfo.Network,
            AddressVersion = worknetFile.BranchInfo.AddressVersion,
            MillisecondsPerBlock = secondsPerBlock == 0 ? 15000 : secondsPerBlock * 1000,
            ValidatorsCount = 1,
            StandbyCommittee = new ECPoint[] { key.PublicKey },
            SeedList = new string[] { $"{System.Net.IPAddress.Loopback}:{30333}" }
        };
    }

    static async Task RunAsync(WorknetFile worknet, string dataDir, uint secondsPerBlock, IConsole console, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<bool>();
        _ = Task.Run(() =>
        {
            try
            {
                using var db = RocksDbUtility.OpenDb(dataDir);
                using var stateStore = new StateServiceStore(worknet.Uri, worknet.BranchInfo, db, true);
                using var trackStore = new PersistentTrackingStore(db, stateStore, true);

                var protocolSettings = GetProtocolSettings(worknet, secondsPerBlock);

                var storeProvider = new WorknetStorageProvider(trackStore);
                StoreFactory.RegisterProvider(storeProvider);

                using var persistencePlugin = new ToolkitPersistencePlugin(db);
                using var logPlugin = new WorkNetLogPlugin(console, Utility.GetDiagnosticWriter(console));
                using var dbftPlugin = new EpicChain.Consensus.DBFTPlugin(GetConsensusSettings(worknet));
                using var rpcServerPlugin = new WorknetRpcServerPlugin(GetRpcServerSettings(worknet), persistencePlugin, worknet.Uri);
                using var neoSystem = new NeoSystem(protocolSettings, storeProvider.Name);

                neoSystem.StartNode(new EpicChain.Network.P2P.ChannelsConfig
                {
                    Tcp = new IPEndPoint(IPAddress.Loopback, 30333)
                });
                dbftPlugin.Start(worknet.ConsensusWallet);

                // DevTracker looks for a string that starts with "EpicChain express is running" to confirm that the instance has started
                // Do not remove or re-word this console output:
                console.Out.WriteLine($"EpicChain worknet is running");

                var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(token, rpcServerPlugin.CancellationToken);
                linkedToken.Token.WaitHandle.WaitOne();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            finally
            {
                tcs.TrySetResult(true);
            }
        }, CancellationToken.None);
        await tcs.Task.ConfigureAwait(false);

        static EpicChain.Consensus.Settings GetConsensusSettings(WorknetFile worknet)
        {
            var settings = new Dictionary<string, string>()
            {
                { "PluginConfiguration:Network", $"{worknet.BranchInfo.Network}" },
                { "IgnoreRecoveryLogs", "true" },
                { "RecoveryLogs", "ConsensusState" }
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
            return new EpicChain.Consensus.Settings(config.GetSection("PluginConfiguration"));
        }

        static RpcServerSettings GetRpcServerSettings(WorknetFile worknet)
        {
            // var ipAddress = IPAddress.TryParse("0.0.0.0", out var _address) ? _address : IPAddress.Loopback;
            // chain.TryReadSetting<IPAddress>("rpc.BindAddress", IPAddress.TryParse, out var bindAddress)
            //     ? bindAddress : IPAddress.Loopback;

            var settings = new Dictionary<string, string>()
                {
                    { "PluginConfiguration:Network", $"{worknet.BranchInfo.Network}" },
                    { "PluginConfiguration:BindAddress", $"{IPAddress.Any}" },
                    { "PluginConfiguration:Port", $"{30332}" },
                    { "PluginConfiguration:SessionEnabled", $"{true}"}
                };

            var config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
            return RpcServerSettings.Load(config.GetSection("PluginConfiguration"));
        }
    }
}
