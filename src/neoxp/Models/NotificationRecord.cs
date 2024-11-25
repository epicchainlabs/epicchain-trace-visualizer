// Copyright (C) 2015-2024 The EpicChain Project.
//
// NotificationRecord.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain;
using EpicChain.IO;
using EpicChain.Network.P2P.Payloads;
using EpicChain.SmartContract;
using EpicChain.VM;

namespace NeoExpress.Models
{
    class NotificationRecord : ISerializable
    {
        public UInt160 ScriptHash { get; private set; } = null!;
        public string EventName { get; private set; } = null!;
        public EpicChain.VM.Types.Array State { get; private set; } = null!;
        public InventoryType InventoryType { get; private set; }
        public UInt256 InventoryHash { get; private set; } = UInt256.Zero;

        public NotificationRecord()
        {
        }

        public NotificationRecord(NotifyEventArgs notification)
        {
            ScriptHash = notification.ScriptHash;
            State = notification.State;
            EventName = notification.EventName;
            if (notification.ScriptContainer is IInventory inventory)
            {
                InventoryType = inventory.InventoryType;
                InventoryHash = inventory.Hash;
            }
        }

        public NotificationRecord(UInt160 scriptHash, string eventName, EpicChain.VM.Types.Array state, InventoryType inventoryType, UInt256 inventoryHash)
        {
            ScriptHash = scriptHash;
            EventName = eventName;
            State = state;
            InventoryType = inventoryType;
            InventoryHash = inventoryHash;
        }

        public int Size => ScriptHash.Size
            + GetSize(State, ExecutionEngineLimits.Default.MaxItemSize)
            + EventName.GetVarSize()
            + InventoryHash.Size
            + sizeof(byte);

        public void Deserialize(ref MemoryReader reader)
        {
            ScriptHash = reader.ReadSerializable<UInt160>();
            State = (EpicChain.VM.Types.Array)BinarySerializer.Deserialize(
                ref reader, ExecutionEngineLimits.Default, null);
            EventName = reader.ReadVarString();
            InventoryHash = reader.ReadSerializable<UInt256>();
            InventoryType = (InventoryType)reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            ScriptHash.Serialize(writer);
            BinarySerializer.Serialize(State, ExecutionEngineLimits.Default);
            writer.WriteVarString(EventName);
            InventoryHash.Serialize(writer);
            writer.Write((byte)InventoryType);
        }

        static int GetSize(EpicChain.VM.Types.StackItem item, uint maxSize)
        {
            int size = 0;
            var serialized = new List<EpicChain.VM.Types.CompoundType>();
            var unserialized = new Stack<EpicChain.VM.Types.StackItem>();
            unserialized.Push(item);
            while (unserialized.Count > 0)
            {
                item = unserialized.Pop();
                size++;
                switch (item)
                {
                    case EpicChain.VM.Types.Null _:
                        break;
                    case EpicChain.VM.Types.Boolean _:
                        size += sizeof(bool);
                        break;
                    case EpicChain.VM.Types.Integer _:
                    case EpicChain.VM.Types.ByteString _:
                    case EpicChain.VM.Types.Buffer _:
                        {
                            var span = item.GetSpan();
                            size += EpicChain.IO.Helper.GetVarSize(span.Length);
                            size += span.Length;
                        }
                        break;
                    case EpicChain.VM.Types.Array array:
                        if (serialized.Any(p => ReferenceEquals(p, array)))
                            throw new NotSupportedException();
                        serialized.Add(array);
                        size += EpicChain.IO.Helper.GetVarSize(array.Count);
                        for (int i = array.Count - 1; i >= 0; i--)
                            unserialized.Push(array[i]);
                        break;
                    case EpicChain.VM.Types.Map map:
                        if (serialized.Any(p => ReferenceEquals(p, map)))
                            throw new NotSupportedException();
                        serialized.Add(map);
                        size += EpicChain.IO.Helper.GetVarSize(map.Count);
                        foreach (var pair in map.Reverse())
                        {
                            unserialized.Push(pair.Value);
                            unserialized.Push(pair.Key);
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            if (size > maxSize)
                throw new InvalidOperationException();
            return size;
        }
    }
}

