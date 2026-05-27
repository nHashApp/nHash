# ⚡ `nhash convert encode url` - URL Percent Encoder / Decoder

Encodes text characters into standard Percent-encoded representations, or decodes them.

---

> [!NOTE]
> **Subcommand Aliases**: `url`

## 🛠️ Usage Syntax

```bash
nhash convert encode url [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Standard URL/Query string to encode or Percent-encoded string to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode Percent-encoded format instead of encoding. | `false` |

### 💡 Live Terminal Examples

**Percent encode URL parameters**:
```bash
nhash convert encode url "hello world!"
```

**Decode Percent-encoded URL**:
```bash
nhash convert encode url "hello%20world%21" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode url "hello world!" --output result.txt
> ```
