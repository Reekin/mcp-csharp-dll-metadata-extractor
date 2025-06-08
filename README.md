[简体中文](README.zh-CN.md)

# DllMetadataExtractor

`DllMetadataExtractor` is a .NET tool designed to extract complete public API information from .NET assembly files (`.dll`).

It can parse detailed metadata including classes, methods, properties, signatures, and XML documentation comments, returning the result as a JSON string.

The primary use case for this tool is to provide an accurate, real-time library API definition for AI or other development tools, thereby helping to analyze and correct code, especially when API calls fail or usage is unclear.

## ✨ Features

- **Complete API Extraction**: Extracts classes, methods, properties, events, fields, and their signatures.
- **XML Documentation Support**: If a corresponding `.xml` documentation file exists alongside the `.dll` file, the tool will automatically parse and include the XML comments.
- **JSON Output**: Outputs metadata in a structured JSON format for easy programmatic parsing.
- **MCP Server**: Runs as an MCP (Model Context Protocol) server, allowing for easy integration with tools that support this protocol.

## 🚀 How to Build

You can use the `build.bat` script located in the project root to build this tool.

```shell
.\build.bat
```

After a successful build, all necessary files will be output to the `publish` directory.

## 🔧 How to Use

The tool is designed to run as an MCP server over standard input/output (stdio). It listens for requests and invokes the `ExtractMetadata` method accordingly.

### Method: `ExtractMetadata`

Analyzes a .NET assembly file (`.dll`) to extract its complete public API information.

**Parameters:**

- `dllPath` (string, required): The absolute path to the target library's DLL file.

**Returns:**

A JSON string containing the detailed API definition.

### Example Invocation (via MCP)

Here is a JSON-RPC example of how to call the tool via the MCP protocol:

```json
{
  "jsonrpc": "2.0",
  "method": "ExtractMetadata",
  "params": {
    "dllPath": "C:\\Path\\To\\Your\\Library.dll"
  },
  "id": 1
}
```

## 🤝 Contributing

Contributions to this project are welcome! If you have any ideas, suggestions, or have found a bug, please feel free to submit a Pull Request or create an Issue.

## 📄 License

This project is licensed under the [MIT](LICENSE) License. 