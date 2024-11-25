// Copyright (C) 2015-2024 The EpicChain Project.
//
// ITraceDebugSink.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.SmartContract;
using EpicChain.VM;
using ExecutionContext = EpicChain.VM.ExecutionContext;

namespace EpicChain.BlockchainToolkit.TraceDebug
{
    public interface ITraceDebugSink : IDisposable
    {
        void Trace(VMState vmState, long gasConsumed, IReadOnlyCollection<ExecutionContext> executionContexts);
        void Log(LogEventArgs args, string scriptName);
        void Notify(NotifyEventArgs args, string scriptName);
        void Results(VMState vmState, long gasConsumed, IReadOnlyCollection<EpicChain.VM.Types.StackItem> results);
        void Fault(Exception exception);
        void Script(Script script);
        void Storages(UInt160 scriptHash, IEnumerable<(StorageKey key, StorageItem item)> storages);
        void ProtocolSettings(uint network, byte addressVersion);
    }
}
