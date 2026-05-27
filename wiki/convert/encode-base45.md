# ⚡ `nhash convert encode base45` - Base45 Text Encoder / Decoder

Encodes or decodes text using RFC 9285 Base45 standard (commonly used in QR Codes and health certificates).

---

> [!NOTE]
> **Subcommand Aliases**: `b45`

## 🛠️ Usage Syntax

```bash
nhash convert encode base45 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base45 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base45 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base45**:
```bash
nhash convert encode base45 "hello"
```

**Decode Base45 back to plain text**:
```bash
nhash convert encode base45 "X.C6" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base45 "hello" --output result.txt
> ```
