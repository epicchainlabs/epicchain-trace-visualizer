// Copyright (C) 2015-2024 The EpicChain Project.
//
// EventWrapper.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Akka.Actor;

namespace NeoExpress
{
    // EventWrapper class taken from neo-gui project: https://github.com/neo-project/neo-node/blob/master/neo-gui/IO/Actors/EventWrapper.cs
    class EventWrapper<T> : UntypedActor
    {
        readonly Action<T> callback;

        public EventWrapper(Action<T> callback)
        {
            this.callback = callback;
            Context.System.EventStream.Subscribe(Self, typeof(T));
        }

        protected override void OnReceive(object message)
        {
            if (message is T obj)
                callback(obj);
        }

        protected override void PostStop()
        {
            Context.System.EventStream.Unsubscribe(Self);
            base.PostStop();
        }

        public static Props Props(Action<T> callback)
        {
            return Akka.Actor.Props.Create(() => new EventWrapper<T>(callback));
        }
    }
}
