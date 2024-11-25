// Copyright (C) 2015-2024 The EpicChain Project.
//
// TestableRpcClient.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace test.bctklib
{
    class TestableRpcClient : EpicChain.Network.RPC.RpcClient
    {
        Queue<Func<JToken>> responseQueue = new();

        public TestableRpcClient(params Func<JToken>[] functions) : base(null)
        {
            foreach (var func in functions.Reverse())
            {
                responseQueue.Enqueue(func);
            }
        }

        public void QueueResource(string resourceName)
        {
            responseQueue.Enqueue(() => JToken.Parse(Utility.GetResource(resourceName)) ?? throw new NullReferenceException());
        }

        public override JToken RpcSend(string method, params JToken[] paraArgs)
        {
            return responseQueue.Dequeue()();
        }
    }
}
