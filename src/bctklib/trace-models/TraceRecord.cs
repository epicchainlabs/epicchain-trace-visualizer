// Copyright (C) 2015-2024 The EpicChain Project.
//
// TraceRecord.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using MessagePack;
using EpicChain.VM;
using System.Buffers;
using ExecutionContext = EpicChain.VM.ExecutionContext;

namespace EpicChain.BlockchainToolkit.TraceDebug
{
    [MessagePackObject]
    public partial class TraceRecord : ITraceDebugRecord
    {
        public const int RecordKey = 0;

        [Key(0)]
        public readonly VMState State;
        [Key(1)]
        public readonly long GasConsumed;
        [Key(2)]
        public readonly IReadOnlyList<StackFrame> StackFrames;

        public TraceRecord(VMState state, long gasConsumed, IReadOnlyList<StackFrame> stackFrames)
        {
            State = state;
            GasConsumed = gasConsumed;
            StackFrames = stackFrames;
        }

        public static void Write(IBufferWriter<byte> writer,
                                 MessagePackSerializerOptions options,
                                 VMState vmState,
                                 long gasConsumed,
                                 IReadOnlyCollection<ExecutionContext> contexts,
                                 Func<ExecutionContext, UInt160> getScriptIdentifier)
        {
            var mpWriter = new MessagePackWriter(writer);
            Write(ref mpWriter, options, vmState, gasConsumed, contexts, getScriptIdentifier);
            mpWriter.Flush();
        }

        public static void Write(ref MessagePackWriter writer,
                                 MessagePackSerializerOptions options,
                                 VMState vmState,
                                 long gasConsumed,
                                 IReadOnlyCollection<ExecutionContext> contexts,
                                 Func<ExecutionContext, UInt160> getScriptIdentifier)
        {
            writer.WriteArrayHeader(2);
            writer.WriteInt32(RecordKey);
            writer.WriteArrayHeader(3);
            options.Resolver.GetFormatterWithVerify<VMState>().Serialize(ref writer, vmState, options);
            writer.WriteInt64(gasConsumed);
            writer.WriteArrayHeader(contexts.Count);
            foreach (var context in contexts)
            {
                StackFrame.Write(ref writer, options, context, getScriptIdentifier(context));
            }
        }
    }
}
