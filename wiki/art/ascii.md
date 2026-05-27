# ⚡ `nhash art ascii` - Ascii Art Banner Generator

Generates large, highly-stylized ASCII text banners using built-in or custom FIGlet fonts.

---

## 🛠️ Usage Syntax

```bash
nhash art ascii [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The text to convert to ASCII art banner. | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--font` | `-f` | `string` | Optional name of a font in the 'fonts' folder, or path to a custom .flf font file. | `N/A` |
| `--color` | `-c` | `string` | Color of the ASCII art (red, green, blue, yellow, magenta, cyan, white). | `N/A` |
| `--list` | `-l` | `bool` | List all available Figlet fonts in the 'fonts' folder. | `false` |

### 💡 Live Terminal Examples

**List all available FIGlet fonts in the project**:
```bash
nhash art ascii --list
```

**Generate 'nHash' banner in cyan**:
```bash
nhash art ascii "nHash" --color cyan
```

**Generate banner with 'slant' font**:
```bash
nhash art ascii "nHash" --font slant
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash art ascii --list --output result.txt
> ```
