// Copyright (C) 2015-2024 The EpicChain Project.
//
// Extensions.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.BuildTasks;
using System;
using System.IO;
using System.Linq;

namespace build_tasks
{
    static class TestFiles
    {
        public static Stream GetStream(string name)
        {
            var assembly = typeof(TestFiles).Assembly;
            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new FileNotFoundException();
            return assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException();
        }

        public static void CopyTo(string name, string destinationPath)
        {
            var destinationDir = Path.GetDirectoryName(destinationPath) ?? throw new Exception();
            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);
            using (var destination = File.OpenWrite(destinationPath))
            using (var resource = GetStream(name))
            {
                resource.CopyTo(destination);
            }
        }

        public static string GetString(string name)
        {
            using (var resource = GetStream(name))
            using (var streamReader = new System.IO.StreamReader(resource))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    static class Extensions
    {
#if NETFRAMEWORK
        public static string[] Split(this string @this, string separator)
            => @this.Split(new string[] { separator }, StringSplitOptions.None);
#endif
        public static void RunThrow(this IProcessRunner @this, string command, string arguments, string workingDirectory = null)
        {
            var result = @this.Run(command, arguments, workingDirectory);
            if (result.ExitCode != 0)
            {
                if (result.Error.Count == 1)
                {
                    throw new Exception(result.Error.Single());
                }
                else
                {
                    throw new AggregateException(result.Error.Select(e => new Exception(e)));
                }
            }
        }

        // public static ProjectCreator AssertBuild(this ProjectCreator @this, ITestOutputHelper? output = null)
        // {
        //     @this.TryBuild(restore: true, out bool result, out BuildOutput buildOutput);
        //     if (!result)
        //     {
        //         if (output != null)
        //         {
        //             foreach (var m in buildOutput.Messages)
        //             {
        //                 output.WriteLine(m);
        //             }
        //         }

        //         switch (buildOutput.Errors.Count)
        //         {
        //             case 0: throw new Exception("TryBuild failed");
        //             case 1: throw new Exception(buildOutput.Errors.First());
        //             default: throw new AggregateException(buildOutput.Errors.Select(e => new Exception(e)));
        //         }
        //     }
        //     return @this;
        // }

        // public static ProjectCreator ImportNeoBuildTools(this ProjectCreator @this)
        // {
        //     var buildTasksPath = typeof(NeoCsc).Assembly.Location;
        //     var testBuildAssmblyDirectory = Path.GetDirectoryName(typeof(TestBuild).Assembly.Location)
        //         ?? throw new Exception("Couldn't get directory name of TestBuild assembly");
        //     var targetsPath = Path.Combine(testBuildAssmblyDirectory, "build", "EpicChain.BuildTasks.targets");

        //     return @this
        //         .Property("NeoBuildTasksAssembly", buildTasksPath)
        //         .Import(targetsPath);
        // }

        // public static ProjectCreator ReferenceNeo(this ProjectCreator @this, string version)
        // {
        //     return @this.ItemPackageReference("Neo", version: version);
        // }

        // public static ProjectCreator ReferenceNeoScFx(this ProjectCreator @this, string version)
        // {
        //     return @this.ItemPackageReference("EpicChain.SmartContract.Framework", version: version);
        // }
    }
}
