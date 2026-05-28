# ⚡ `nhash art gradient` - Horizontal Gradient Text Renderer

Displays strings shaded with custom horizontal hex-color transitions in the terminal.

---

## 🛠️ Usage Syntax

```bash
nhash art gradient [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The text to render with color gradient. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--from` | `-f` | `string` | Start hex-color code (e.g. #FF5733) or standard console color name. | `#FF6B6B` |
| `--to` | `-t` | `string` | End hex-color code (e.g. #33FF57) or standard console color name. | `#6BCB77` |

### 💡 Live Terminal Examples

**Render text with default pink-to-green gradient**:
```bash
nhash art gradient "Beautiful Gradient Text"
```

**Render with blue to red hex color transitions**:
```bash
nhash art gradient "WARNING ALERT" -f "#0000FF" -t "#FF0000"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash art gradient "Beautiful Gradient Text" --output result.txt
> ```
