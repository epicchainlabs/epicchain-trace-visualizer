// Copyright (C) 2015-2024 The EpicChain Project.
//
// WorkNetLogPlugin.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using McMaster.Extensions.CommandLineUtils;
using EpicChain;
using EpicChain.BlockchainToolkit.Persistence;
using EpicChain.Ledger;
using EpicChain.Network.P2P.Payloads;
using EpicChain.Persistence;
using EpicChain.Plugins;
using EpicChain.SmartContract;
using EpicChain.SmartContract.Native;
using System.Diagnostics;
using static Crayon.Output;

namespace NeoWorkNet.Commands;

class WorkNetLogPlugin : Plugin
{
    NeoSystem? neoSystem;
    readonly IConsole console;

    public WorkNetLogPlugin(IConsole console, Action<string, object?>? diagnosticWriter = null)
    {
        if (diagnosticWriter is not null)
        {
            var stateServiceObserver = new KeyValuePairObserver(diagnosticWriter);
            var diagnosticObserver = new DiagnosticObserver(StateServiceStore.LoggerCategory, stateServiceObserver);
            DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
        }

        this.console = console;
        Blockchain.Committing += OnCommitting;
        ApplicationEngine.Log += OnAppEngineLog!;
        EpicChain.Utility.Logging += OnNeoUtilityLog;
    }

    public override void Dispose()
    {
        EpicChain.Utility.Logging -= OnNeoUtilityLog;
        ApplicationEngine.Log -= OnAppEngineLog!;
        Blockchain.Committing -= OnCommitting;
        GC.SuppressFinalize(this);
    }

    protected override void OnSystemLoaded(NeoSystem system)
    {
        if (neoSystem is not null)
            throw new Exception($"{nameof(OnSystemLoaded)} already called");
        neoSystem = system;
        base.OnSystemLoaded(system);
    }

    protected string GetContractName(UInt160 scriptHash)
    {
        if (neoSystem is not null)
        {
            var contract = NativeContract.ContractManagement.GetContract(neoSystem.StoreView, scriptHash);
            if (contract is not null)
            {
                return contract.Manifest.Name;
            }
        }

        return scriptHash.ToString();
    }

    void OnAppEngineLog(object sender, LogEventArgs args)
    {
        var container = args.ScriptContainer is null
            ? string.Empty
            : $" [{args.ScriptContainer.GetType().Name}]";


        console.WriteLine($"{Magenta(GetContractName(args.ScriptHash))} Log: \"{Cyan(args.Message)}\" {container}");
    }

    void OnNeoUtilityLog(string source, LogLevel level, object message)
    {
        console.WriteLine($"{DateTimeOffset.Now:HH:mm:ss.ff} {source} {level} {message}");
    }

    void OnCommitting(NeoSystem system, Block block, DataCache snapshot, IReadOnlyList<Blockchain.ApplicationExecuted> applicationExecutedList)
    {
        foreach (var appExec in applicationExecutedList)
        {
            OnApplicationExecuted(appExec);
        }
    }

    void OnApplicationExecuted(Blockchain.ApplicationExecuted applicationExecuted)
    {
        if (applicationExecuted.VMState == EpicChain.VM.VMState.FAULT)
        {
            var logMessage = $"Tx FAULT: hash={applicationExecuted.Transaction.Hash}";
            if (!string.IsNullOrEmpty(applicationExecuted.Exception.Message))
            {
                logMessage += $" exception=\"{applicationExecuted.Exception.Message}\"";
            }
            console.Error.WriteLine(Red(logMessage));
        }
    }
}
