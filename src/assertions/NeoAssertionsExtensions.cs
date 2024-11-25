// Copyright (C) 2015-2024 The EpicChain Project.
//
// NeoAssertionsExtensions.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.SmartContract;
using EpicChain.VM.Types;

namespace EpicChain.Assertions
{
    public static class NeoAssertionsExtensions
    {
        public static StackItemAssertions Should(this StackItem item) => new StackItemAssertions(item);

        public static NotifyEventArgsAssertions Should(this NotifyEventArgs args) => new NotifyEventArgsAssertions(args);

        public static StorageItemAssertions Should(this StorageItem item) => new StorageItemAssertions(item);
    }
}
