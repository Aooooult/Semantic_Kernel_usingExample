# Semantic Kernel Console App

This is a simple console application demonstrating how to use [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel) to:

- Send basic prompts to OpenAI using the Semantic Kernel;
- Perform simple file system operations on the local machine through plugins.

## Features

- ✅ Integration with Azure OpenAI or OpenAI API;
- 📁 Basic plugin for file system manipulation (e.g. listing files, reading/writing files);
- ⏰ Additional plugin for returning the current system time;
- 🧠 Simple use of Semantic Kernel's planner and prompt execution.

## Prerequisites

- .NET 9.0 SDK
- OpenAI API Key or Azure OpenAI Key
- Internet connection (for API access)

## How to Run

1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/SemanticKernelConsoleApp.git
   cd SemanticKernelConsoleApp
   ```

2. Restore packages:
   ```bash
   dotnet restore
   ```

3. Update your API key in `Program.cs`.

4. Run the app:
   ```bash
   dotnet run
   ```

## Project Structure

```
ConsoleApp1/
├── Program.cs             # Entry point with kernel setup
├── AutoInvocationFilter.cs
├── Plugins/
│   ├── FilePlugin.cs      # File system operations
│   └── TimePlugin.cs      # System time responses
├── ConsoleApp1.csproj     # Project definition
└── README.md
```

## License

MIT — free to use and modify.

