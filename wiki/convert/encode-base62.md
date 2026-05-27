# ⚡ `nhash convert encode base62` - Base62 Text Encoder / Decoder

Encodes or decodes text according to Base62 representation standards (commonly used for clean URL shorteners).

---

> [!NOTE]
> **Subcommand Aliases**: `b62`

## 🛠️ Usage Syntax

```bash
nhash convert encode base62 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text input to encode or Base62 payload to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Base62 text instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Encode text to Base62**:
```bash
nhash convert encode base62 "hello"
```

**Decode Base62 back to plain text**:
```bash
nhash convert encode base62 2tq5Zp -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode base62 "hello" --output result.txt
> ```
