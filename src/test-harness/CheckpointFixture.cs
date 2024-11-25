// Copyright (C) 2015-2024 The EpicChain Project.
//
// CheckpointFixture.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain;
using EpicChain.BlockchainToolkit;
using EpicChain.BlockchainToolkit.Models;
using EpicChain.BlockchainToolkit.Persistence;
using EpicChain.Persistence;
using System.IO.Abstractions;

namespace NeoTestHarness
{
    public abstract class CheckpointFixture : IDisposable
    {
        readonly static Lazy<IFileSystem> defaultFileSystem = new Lazy<IFileSystem>(() => new FileSystem());
        readonly CheckpointStore checkpointStore;

        public IReadOnlyStore CheckpointStore => checkpointStore;
        public ProtocolSettings ProtocolSettings => checkpointStore.Settings;

        public CheckpointFixture(string checkpointPath)
        {
            if (Path.IsPathFullyQualified(checkpointPath))
            {
                if (!File.Exists(checkpointPath))
                    throw new FileNotFoundException("couldn't find checkpoint", checkpointPath);
            }
            else
            {
                var directory = Path.GetFullPath(".");
                var tempPath = Path.GetFullPath(checkpointPath, directory);
                while (!File.Exists(tempPath))
                {
                    directory = Path.GetDirectoryName(directory);
                    tempPath = Path.GetFullPath(checkpointPath, directory!);
                }
                checkpointPath = tempPath;
            }

            checkpointStore = new CheckpointStore(checkpointPath);
        }

        public void Dispose()
        {
            checkpointStore.Dispose();
        }

        public ExpressChain FindChain(string fileName = Constants.DEFAULT_EXPRESS_FILENAME, IFileSystem? fileSystem = null, string? searchFolder = null)
            => (fileSystem ?? defaultFileSystem.Value).FindChain(fileName, searchFolder);
    }
}

