# ⚡ `nhash dev color` - Color Format & Swatch Translator

Translates colors instantly between formats (HEX, RGB, HSL, CMYK) with terminal truecolor swatch previews.

---

> [!NOTE]
> **Subcommand Aliases**: `c`

## 🛠️ Usage Syntax

```bash
nhash dev color [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<value>` | `string` | Color representation (hex like '#FF5733' or RGB like '255,87,51'). | ✅ Yes |

### 💡 Live Terminal Examples

**Convert Hex color and preview**:
```bash
nhash dev color "#FF5733"
```

**Convert RGB components to other colorspaces**:
```bash
nhash dev color "128,0,255"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash dev color "#FF5733" --output result.txt
> ```
