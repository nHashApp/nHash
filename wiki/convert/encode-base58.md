# ⚡ `nhash convert encode base58` - Base58 Text Encoder / Decoder

Encodes or decodes text according to Base58 representation standards (frequently used in Bitcoin address formats).

---

> [!NOTE]
> **Subcommand Aliases**: `b58`

## 🛠️ Usage Syntax

```bash
nhash convert encode base58 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base58 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base58 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base58**:
```bash
nhash convert encode base58 "hello"
```

**Decode Base58 back to plain text**:
```bash
nhash convert encode base58 "Cn8eb" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base58 "hello" --output result.txt
> ```
