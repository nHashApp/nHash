# ⚡ `nhash convert encode base64` - Base64 Text Encoder / Decoder

Encodes or decodes text according to standard RFC 4648 Base64 representation.

---

> [!NOTE]
> **Subcommand Aliases**: `b64`

## 🛠️ Usage Syntax

```bash
nhash convert encode base64 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base64 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base64 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base64**:
```bash
nhash convert encode base64 "Hello, World!"
```

**Decode Base64 using short aliases**:
```bash
nhash conv e b64 SGVsbG8sIFdvcmxkIQ== -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base64 "Hello, World!" --output result.txt
> ```
