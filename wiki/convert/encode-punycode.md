# ⚡ `nhash convert encode punycode` - Punycode (IDN Domain) Encoder / Decoder

Translates internationalized domain names (containing non-ASCII chars) to punycode format (RFC 3492).

---

> [!NOTE]
> **Subcommand Aliases**: `pny`

## 🛠️ Usage Syntax

```bash
nhash convert encode punycode [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Internationalized domain to encode, or punycode label to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode punycode string instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Punycode encode a Persian/Arabic domain name**:
```bash
nhash convert encode punycode "مأرب.com"
```

**Decode punycode back to Unicode**:
```bash
nhash convert encode punycode "xn--mgb9a8a.com" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode punycode "مأرب.com" --output result.txt
> ```
