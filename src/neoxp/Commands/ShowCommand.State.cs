// Copyright (C) 2015-2024 The EpicChain Project.
//
// ShowCommand.State.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using McMaster.Extensions.CommandLineUtils;

namespace NeoExpress.Commands
{
    partial class ShowCommand
    {
        [Command("state", Description = "Show state")]
        internal class State
        {
            readonly ExpressChainManagerFactory chainManagerFactory;

            public State(ExpressChainManagerFactory chainManagerFactory)
            {
                this.chainManagerFactory = chainManagerFactory;
            }

            [Argument(0, Description = "Optional block hash or index. Show most recent block if unspecified")]
            internal string BlockHash { get; init; } = string.Empty;

            [Option(Description = "Path toepicchain-express data file")]
            internal string Input { get; init; } = string.Empty;

            internal async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
            {
                try
                {
                    var (chainManager, config) = chainManagerFactory.LoadChain(Input);
                    using var expressNode = chainManager.GetExpressNode();
                    var blockHeight = (await expressNode.GetLatestBlockAsync().ConfigureAwait(false)).Index;
                    console.WriteLine($"Block height: {blockHeight}");
                    console.WriteLine($"IsRunning: {chainManager.IsRunning(null)}");
                    console.WriteLine($"Config file: {config}");
                    return 0;
                }
                catch (Exception ex)
                {
                    app.WriteException(ex);
                    return 1;
                }
            }
        }
    }
}
