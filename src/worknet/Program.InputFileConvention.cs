// Copyright (C) 2015-2024 The EpicChain Project.
//
// Program.InputFileConvention.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Conventions;
using System.Reflection;
using static EpicChain.BlockchainToolkit.Constants;

namespace NeoWorkNet;

partial class Program
{
    class InputFileConvention : IConvention
    {
        public void Apply(ConventionContext context)
        {
            if (context.ModelType is null)
                return;
            if (context.ModelType.Equals(typeof(Commands.CreateCommand)))
                return;

            var subCmdAttrib = context.ModelType.GetCustomAttribute<SubcommandAttribute>();
            if (subCmdAttrib is not null)
                return;

            var parser = context.Application.ValueParsers.GetParser<string>()
                ?? throw new InvalidOperationException("Can't get string value parser");

            var option = new CommandOption<string>(parser, "--input", CommandOptionType.SingleValue)
            {
                Description = $"Path to {WORKNET_EXTENSION} data file"
            };
            context.Application.AddOption(option);
        }
    }
}
