// Copyright (C) 2015-2024 The EpicChain Project.
//
// INotificationsProvider.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using EpicChain.Persistence;

namespace EpicChain.BlockchainToolkit.Plugins
{
    public readonly record struct NotificationInfo(
        uint BlockIndex,
        ushort TxIndex,
        ushort NotificationIndex,
        NotificationRecord Notification);

    public interface INotificationsProvider
    {
        IEnumerable<NotificationInfo> GetNotifications(
            SeekDirection direction = SeekDirection.Forward,
            IReadOnlySet<UInt160>? contracts = null,
            IReadOnlySet<string>? eventNames = null);
    }
}
