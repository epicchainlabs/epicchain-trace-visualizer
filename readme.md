# EpicChain-Trace-Visualizer

**EpicChain-Trace-Visualizer** is a specialized tool designed for the **EpicChain Private Net**, focusing on enhancing the debugging and analysis of smart contracts. As the complexity of blockchain applications increases, having robust tools to visualize and understand transaction flows becomes essential. This tool provides developers with an intuitive interface to visualize execution traces, making it easier to analyze complex transactions, troubleshoot issues, and optimize smart contract performance during development.

## Key Features

### Trace Visualization
The core feature of **EpicChain-Trace-Visualizer** is its ability to convert raw execution traces into clear and meaningful graphical representations. Developers can see how transactions flow through smart contracts, understand the interaction between different components, and identify the paths taken by transactions.

### Interactive Debugging
**EpicChain-Trace-Visualizer** offers an interactive debugging experience. Developers can step through the execution of transactions, observing real-time changes in state and variable values. This functionality is crucial for pinpointing issues that may arise during the execution of smart contracts.

### Detailed Insights
The tool provides comprehensive insights into each step of smart contract execution. Users can view input values, outputs, and any errors that occur during processing. This level of detail helps developers understand how their contracts behave under different conditions and identify potential vulnerabilities.

### Block and Transaction Selection
**EpicChain-Trace-Visualizer** supports specifying blocks by index or hash and transactions by hash. This feature allows users to focus on specific events of interest, making it easier to analyze targeted execution scenarios.

### User-Friendly Interface
The intuitive design of **EpicChain-Trace-Visualizer** ensures that developers of all experience levels can navigate through complex trace data. The user-friendly interface minimizes the learning curve and allows developers to concentrate on building and improving their smart contracts.

### Export and Share Functionality
Collaboration is vital in development teams, and **EpicChain-Trace-Visualizer** facilitates this by providing options to export visualizations. Developers can create reports or share insights with their colleagues, enhancing teamwork and collective problem-solving.

### Customizable Views
Every developer has unique needs, and **EpicChain-Trace-Visualizer** addresses this with customizable visualization parameters. Users can tailor the display to meet their specific requirements, ensuring that they can work in an environment that suits their workflow.

## Installation

### Install via .NET Tool

To effectively use **EpicChain-Trace-Visualizer**, you'll need to install it via the .NET Tool. The installation process is straightforward but does require some prerequisites.

#### Requirements

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or higher is required for proper functionality of the tool. Ensure you have this installed on your system before proceeding with the installation steps below.

### Installation Steps

To install the latest version of **EpicChain-Trace-Visualizer** as a global tool, follow these steps:

1. Open a terminal window on your system.
2. Run the following command to install the tool:

   ```shell
   dotnet tool install EpicChain.Trace.Visualizer -g
   ```

   This command will download and install the latest version of **EpicChain-Trace-Visualizer** globally, making it accessible from any terminal session.

3. To keep your installation up to date, you can run the following command:

   ```shell
   dotnet tool update EpicChain.Trace.Visualizer -g
   ```

   This command checks for the latest version and updates your installation accordingly.

The installation and update process for **EpicChain-Trace** follows the same commands:

```shell
dotnet tool install EpicChain.Trace -g
dotnet tool update EpicChain.Trace -g
```

### Additional EpicChain-Trace-Visualizer Requirements

#### Ubuntu Installation

> **Note**: While Microsoft provides instructions for [installing .NET via Snap](https://docs.microsoft.com/en-us/dotnet/core/install/linux-snap), there is a [known issue](https://github.com/dotnet/runtime/issues/3775#issuecomment-534263315) with this approach that leads to a segmentation fault in **EpicChain Express**. Unfortunately, this issue has been closed and will not be addressed. Therefore, we recommend using APT for installing .NET on Ubuntu instead.

Before proceeding with the installation, you need to install some required dependencies on Ubuntu. Use the following command:

```shell
sudo apt install libsnappy-dev libc6-dev librocksdb-dev -y
```

This command installs the necessary libraries required for **EpicChain-Trace-Visualizer** to function correctly.

#### macOS Installation

If you're using macOS, you'll need to install RocksDB via [Homebrew](https://brew.sh/):

1. Open a terminal window.
2. Run the following command:

   ```shell
   brew install rocksdb
   ```

> **Note**: .NET 6 Arm64 has [full support for Apple Silicon](https://devblogs.microsoft.com/dotnet/announcing-net-6/#arm64). Homebrew also supports Apple Silicon. If you encounter any issues running **EpicChain-Trace-Visualizer** on Apple Silicon hardware, please [open an issue](https://github.com/epicchainlabs/epicchain-trace-visualizer/issues) in the EpicChain-Trace-Visualizer repository.

## Usage Guide

### Getting Started with EpicChain-Trace-Visualizer

Once you've installed **EpicChain-Trace-Visualizer**, you're ready to start using it. Here are some fundamental commands to get you started:

#### Creating a New Local EpicChain Network

To create a new local EpicChain network, you can use the following command:

```shell
epicchain create
```

This command initializes a new network instance on your local machine, providing you with a sandbox environment to develop and test your smart contracts.

#### Listing All Wallets

To view all wallets associated with your EpicChain network, use the command:

```shell
epicchain wallet list
```

This command retrieves a list of all existing wallets, allowing you to manage and interact with them as needed.

#### Showing Genesis Account Balance

To check the balance of the genesis account, which is crucial for understanding the initial state of your network, you can use the command:

```shell
epicchain show balances genesis
```

The `genesis` account represents the consensus node multi-sig account that holds the genesis **EpicChain** and **EpicPulse** assets. Knowing the balance of this account is essential for initial transactions.

#### Sending Transactions

To transfer assets between accounts, such as sending 1 EpicPulse from the genesis account to another account (e.g., node1), you can run the following command:

```shell
epicchain transfer 1 epicpulse genesis node1
```

This command initiates a transfer of assets, allowing you to simulate real-world transactions on your private network.

### Command Reference

For a complete overview of all commands and features available in **EpicChain-Trace-Visualizer**, please refer to the [Command Reference](docs/command-reference.md). This documentation will provide detailed explanations of each command and its usage, helping you to fully leverage the capabilities of **EpicChain-Trace-Visualizer**.
