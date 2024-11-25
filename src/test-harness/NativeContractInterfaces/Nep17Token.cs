// Copyright (C) 2015-2024 The EpicChain Project.
//
// Nep17Token.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace NeoTestHarness.NativeContractInterfaces
{
    public interface Nep17Token
    {
        System.Numerics.BigInteger balanceOf(EpicChain.UInt160 account);
        System.Numerics.BigInteger decimals();
        string symbol();
        System.Numerics.BigInteger totalSupply();
        bool transfer(EpicChain.UInt160 @from, EpicChain.UInt160 to, System.Numerics.BigInteger amount, object data);

        interface Events
        {
            void Transfer(EpicChain.UInt160 @from, EpicChain.UInt160 to, System.Numerics.BigInteger amount);
        }
    }
}
