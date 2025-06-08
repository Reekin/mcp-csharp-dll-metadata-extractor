[English](README.md)

# DllMetadataExtractor

`DllMetadataExtractor` 是一个 .NET 工具，旨在通过分析 .NET 程序集文件 (`.dll`) 来提取其完整的公共 API 信息。

它可以解析出包括类、方法、属性、签名以及 XML 文档注释在内的详细元数据，并将结果以 JSON 字符串的形式返回。

这个工具的主要应用场景是为 AI 或其他开发工具提供一个准确、实时的库 API 定义，从而帮助分析和修正代码，尤其是在 API 调用失败或用法不明确的情况下。

## ✨ 功能特性

- **完整的 API 提取**：提取类、方法、属性、事件、字段及其签名。
- **XML 文档支持**：如果 .dll 文件旁边存在对应的 .xml 文档文件，工具会自动解析并包含 XML 注释。
- **JSON 输出**：以结构化的 JSON 格式输出元数据，便于程序解析。
- **MCP 服务器**：作为 MCP (Model Context Protocol) 服务器运行，可以轻松地与支持该协议的工具集成。

## 🚀 如何构建

你可以使用项目根目录下的 `build.bat` 脚本来构建此工具。

```shell
.\build.bat
```

构建成功后，所有必要的文件都会被输出到 `publish` 目录下。

## 🔧 如何使用

该工具被设计为作为 MCP 服务器通过标准输入/输出 (stdio) 运行。它会监听请求，并根据请求调用 `ExtractMetadata` 方法。

### 方法: `ExtractMetadata`

分析一个 .NET 程序集文件 (`.dll`) 来提取其完整的公共 API 信息。

**参数:**

- `dllPath` (string, 必选): 目标库 DLL 文件的绝对路径。

**返回:**

一个包含详细 API 定义的 JSON 字符串。

### 调用示例 (通过 MCP)

这是一个通过 MCP 协议调用该工具的 JSON-RPC 示例：

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

## 🤝 贡献

欢迎对这个项目做出贡献！如果你有任何想法、建议或发现了 bug，请随时提交 Pull Request 或创建 Issue。

## 📄 许可证

该项目采用 [MIT](LICENSE) 许可证。 