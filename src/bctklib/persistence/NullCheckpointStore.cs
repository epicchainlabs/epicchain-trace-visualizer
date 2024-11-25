// Copyright (C) 2015-2024 The EpicChain Project.
//
// NullCheckpointStore.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.BlockchainToolkit.Models;
using EpicChain.Persistence;

namespace EpicChain.BlockchainToolkit.Persistence
{
    public class NullCheckpointStore : ICheckpointStore
    {
        public ProtocolSettings Settings { get; }

        public NullCheckpointStore(ExpressChain? chain)
            : this(chain?.Network, chain?.AddressVersion)
        {
        }

        public NullCheckpointStore(uint? network = null, byte? addressVersion = null)
        {
            this.Settings = ProtocolSettings.Default with
            {
                Network = network ?? ProtocolSettings.Default.Network,
                AddressVersion = addressVersion ?? ProtocolSettings.Default.AddressVersion,
            };
        }

        public IEnumerable<(byte[] Key, byte[]? Value)> Seek(byte[] key, SeekDirection direction)
            => Enumerable.Empty<(byte[], byte[]?)>();
        public byte[]? TryGet(byte[] key) => null;
        public bool Contains(byte[] key) => false;
    }
}
