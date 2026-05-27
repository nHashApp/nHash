# ⚡ `nhash convert encode base91` - Base91 Text Encoder / Decoder

Encodes or decodes text using standard basE91 algorithm.

---

> [!NOTE]
> **Subcommand Aliases**: `b91`

## 🛠️ Usage Syntax

```bash
nhash convert encode base91 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base91 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base91 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base91**:
```bash
nhash convert encode base91 "hello"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base91 "hello" --output result.txt
> ```
