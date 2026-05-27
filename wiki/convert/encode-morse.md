# ⚡ `nhash convert encode morse` - Morse Code Encoder / Decoder

Encodes standard text characters to Morse code dots and dashes, or decodes Morse code back.

---

> [!NOTE]
> **Subcommand Aliases**: `ms`

## 🛠️ Usage Syntax

```bash
nhash convert encode morse [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text to encode, or dots/dashes separated by spaces/slashes to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Morse code payload to standard text. | `false` |

### 💡 Live Terminal Examples

**Encode 'SOS' to Morse**:
```bash
nhash convert encode morse "SOS"
```

**Decode Morse back to text**:
```bash
nhash convert encode morse "... --- ..." -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode morse "SOS" --output result.txt
> ```
