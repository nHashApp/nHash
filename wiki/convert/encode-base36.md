# ⚡ `nhash convert encode base36` - Base36 Text Encoder / Decoder

Encodes or decodes text according to Base36 representation standards.

---

> [!NOTE]
> **Subcommand Aliases**: `b36`

## 🛠️ Usage Syntax

```bash
nhash convert encode base36 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base36 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base36 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base36**:
```bash
nhash convert encode base36 "hello"
```

**Decode Base36 back to plain text**:
```bash
nhash convert encode base36 4s02c3d -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base36 "hello" --output result.txt
> ```
