// Copyright (C) 2015-2024 The EpicChain Project.
//
// NeoToken.cs file belongs toepicchain-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace NeoTestHarness.NativeContractInterfaces
{
    public interface NeoToken : Nep17Token
    {
        EpicChain.VM.Types.Array getAccountState(EpicChain.UInt160 account);
        EpicChain.VM.Types.Array getCandidates();
        EpicChain.VM.Types.Array getCommittee();
        System.Numerics.BigInteger getGasPerBlock();
        EpicChain.VM.Types.Array getNextBlockValidators();
        System.Numerics.BigInteger getRegisterPrice();
        bool registerCandidate(EpicChain.Cryptography.ECC.ECPoint pubkey);
        void setGasPerBlock(System.Numerics.BigInteger gasPerBlock);
        void setRegisterPrice(System.Numerics.BigInteger registerPrice);
        System.Numerics.BigInteger unclaimedGas(EpicChain.UInt160 account, System.Numerics.BigInteger end);
        bool unregisterCandidate(EpicChain.Cryptography.ECC.ECPoint pubkey);
        bool vote(EpicChain.UInt160 account, EpicChain.Cryptography.ECC.ECPoint voteTo);
    }
}
