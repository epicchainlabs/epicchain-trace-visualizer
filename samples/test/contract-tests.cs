using FluentAssertions;
using EpicChain.Assertions;
using EpicChain.BlockchainToolkit;
using EpicChain.BlockchainToolkit.Models;
using EpicChain.BlockchainToolkit.SmartContract;
using EpicChain.VM;
using NeoTestHarness;
using Xunit;
using Xunit.Abstractions;

namespace ContractTests
{
    [CheckpointPath("checkpoints/contract-deployed.epicchain-checkpoint")]
    public class ContractDeployedTests : IClassFixture<CheckpointFixture<ContractDeployedTests>>
    {
        readonly CheckpointFixture fixture;
        readonly ExpressChain chain;
        readonly ITestOutputHelper output;

        public ContractDeployedTests(CheckpointFixture<ContractDeployedTests> fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.chain = fixture.FindChain();
            this.output = output;
        }

        [Fact]
        public void test_symbol()
        {
            var settings = chain.GetProtocolSettings();
            using var snapshot = fixture.GetSnapshot();
            using var engine = new TestApplicationEngine(snapshot, settings);

            var state = engine.ExecuteScript<contract>(c => c.symbol());

            engine.State.Should().Be(VMState.HALT);
            engine.ResultStack.Should().HaveCount(1);
            engine.ResultStack.Peek(0).Should().BeEquivalentTo("TEST");
        }

        [Fact]
        public void test_decimals()
        {
            var settings = chain.GetProtocolSettings();
            using var snapshot = fixture.GetSnapshot();
            using var engine = new TestApplicationEngine(snapshot, settings);

            var state = engine.ExecuteScript<contract>(c => c.decimals());

            engine.State.Should().Be(VMState.HALT);
            engine.ResultStack.Should().HaveCount(1);
            engine.ResultStack.Peek(0).Should().BeEquivalentTo(0);
        }
    }
}
