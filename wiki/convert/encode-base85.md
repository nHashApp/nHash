# ⚡ `nhash convert encode base85` - Base85 Text Encoder / Decoder

Encodes or decodes text according to Base85 (Ascii85) representation standards.

---

> [!NOTE]
> **Subcommand Aliases**: `b85`

## 🛠️ Usage Syntax

```bash
nhash convert encode base85 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base85 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base85 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base85**:
```bash
nhash convert encode base85 "hello"
```

**Decode Base85 back to plain text**:
```bash
nhash convert encode base85 "BOu!r" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base85 "hello" --output result.txt
> ```
