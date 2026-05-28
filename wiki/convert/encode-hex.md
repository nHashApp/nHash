# ⚡ `nhash convert encode hex` - Hexadecimal (Base16) Encoder / Decoder

Encodes or decodes text to/from hexadecimal bytes format.

---

> [!NOTE]
> **Subcommand Aliases**: `b16`

## 🛠️ Usage Syntax

```bash
nhash convert encode hex [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Plain text to encode, or hex payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Hex text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Hex**:
```bash
nhash convert encode hex "hello"
```

**Decode Hex to plain text**:
```bash
nhash convert encode hex 68656c6c6f -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode hex "hello" --output result.txt
> ```
