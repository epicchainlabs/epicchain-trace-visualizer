// Copyright (C) 2015-2024 The EpicChain Project.
//
// DotNetToolTask.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = Microsoft.Build.Utilities.Task;

namespace EpicChain.BuildTasks
{
    internal enum DotNetToolType { Local, Global }

    public abstract class DotNetToolTask : Task
    {
        protected abstract string Command { get; }
        protected abstract string PackageId { get; }
        protected abstract string GetArguments();
        protected virtual bool ValidateVersion(NugetPackageVersion version) => true;

        readonly IProcessRunner processRunner;
        DotNetToolType toolType;

        [Output]
        public string ToolType => $"{toolType}";

        [Output]
        public string CommandLine { get; set; } = string.Empty;

        [Output]
        public string ToolVersion { get; set; } = string.Empty;

        public ITaskItem? WorkingDirectory { get; set; }

        protected DotNetToolTask(IProcessRunner? processRunner = null)
        {
            this.processRunner = processRunner ?? new ProcessRunner();
        }

        protected virtual void ExecutionSuccess(IReadOnlyCollection<string> output)
        {
            foreach (var @out in output)
            {
                Log.LogMessage(MessageImportance.High, "{0}: {1}",
                    Command, @out);
            }
        }

        public override bool Execute()
        {
            var packageId = PackageId;
            var directory = WorkingDirectory;

            Log.LogMessage(MessageImportance.High, "Searching for tool package {0} in {1}",
                packageId, directory?.ItemSpec ?? "<no working directory>");

            if (FindTool(packageId, directory, out var toolType, out var version))
            {
                this.toolType = toolType;
                this.ToolVersion = version.ToString();
                Log.LogMessage(MessageImportance.High, "Found {0} tool package {1} version {2}",
                    toolType, packageId, version);

                var command = toolType == EpicChain.BuildTasks.DotNetToolType.Global ? Command : "dotnet";
                var arguments = toolType == EpicChain.BuildTasks.DotNetToolType.Global
                        ? GetArguments()
                        : Command + " " + GetArguments();

                CommandLine = $"{command} {arguments}";
                Log.LogMessage(MessageImportance.High, "Running {0} {1}",
                    command, arguments);

                if (TryExecute(command, arguments, directory, out var output))
                {
                    ExecutionSuccess(output);
                    return true;
                }

                return false;
            }

            Log.LogError("tool package {0} not found", packageId);
            return false;
        }

        internal bool FindTool(string package, ITaskItem? directory, out DotNetToolType toolType, out NugetPackageVersion version)
        {
            if (directory is not null)
            {
                if (TryExecute("dotnet", "tool list --local", directory, out var output)
                    && ContainsPackage(output, package, out version)
                    && ValidateVersion(version))
                {
                    toolType = DotNetToolType.Local;
                    return true;
                }
            }

            {
                if (TryExecute("dotnet", "tool list --global", directory, out var output)
                    && ContainsPackage(output, package, out version)
                    && ValidateVersion(version))
                {
                    toolType = DotNetToolType.Global;
                    return true;
                }
            }

            toolType = DotNetToolType.Local;
            version = default;
            return false;
        }

        internal bool TryExecute(string command, string arguments, ITaskItem? directory, out IReadOnlyCollection<string> output)
        {
            var results = processRunner.Run(command, arguments, directory?.ItemSpec ?? "");

            if (results.ExitCode != 0 || results.Error.Any())
            {
                if (results.ExitCode != 0)
                    Log.LogError("{0} returned {1}", Command, results.ExitCode);
                else
                    Log.LogWarning("{0} returned {1}", Command, results.ExitCode);

                foreach (var err in results.Error)
                {
                    Log.LogError(err);
                }
                foreach (var @out in results.Output)
                {
                    Log.LogWarning(@out);
                }

                output = Array.Empty<string>();
                return false;
            }

            output = results.Output;
            return true;
        }

        internal static bool ContainsPackage(IReadOnlyCollection<string> output, string package, out NugetPackageVersion version)
        {
            foreach (var o in output.Skip(2))
            {
                var row = ParseTableRow(o);
                if (row.Count < 2)
                    continue;
                if (row[0].Equals(package, StringComparison.InvariantCultureIgnoreCase)
                    && NugetPackageVersion.TryParse(row[1], out version))
                {
                    return true;
                }
            }

            version = default;
            return false;
        }

        internal static IReadOnlyList<string> ParseTableRow(string row)
        {
            const string ColumnDelimiter = "      ";

            var columns = new List<string>();
            while (true)
            {
                row = row.TrimStart();
                if (row.Length == 0)
                    break;
                var index = row.IndexOf(ColumnDelimiter);
                if (index == -1)
                {
                    columns.Add(row);
                    break;
                }
                else
                {
                    columns.Add(row.Substring(0, index).Trim());
                    row = row.Substring(index);
                }
            }
            return columns;
        }
    }
}
