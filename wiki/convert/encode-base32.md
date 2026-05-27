# ⚡ `nhash convert encode base32` - Base32 Text Encoder / Decoder

Encodes or decodes text according to Base32 representation standards.

---

> [!NOTE]
> **Subcommand Aliases**: `b32`

## 🛠️ Usage Syntax

```bash
nhash convert encode base32 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base32 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base32 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base32**:
```bash
nhash convert encode base32 "hello"
```

**Decode Base32 back to plain text**:
```bash
nhash convert encode base32 NBSWY3DPEB====== -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base32 "hello" --output result.txt
> ```
