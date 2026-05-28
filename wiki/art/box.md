# ⚡ `nhash art box` - Text Box Border Drawer

Draws beautiful Spectre Panel boxes around input strings with customizable border styles.

---

## 🛠️ Usage Syntax

```bash
nhash art box [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The text payload to display inside the terminal box. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--title` | `-t` | `string` | Optional box title display. | `N/A` |
| `--border` | `-b` | `string` | Border style: single, double, rounded, heavy, ascii. | `rounded` |

### 💡 Live Terminal Examples

**Draw box with a title and rounded border**:
```bash
nhash art box "System status is Normal" -t "STATUS"
```

**Draw a double-bordered box**:
```bash
nhash art box "Hello World!" -b double
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash art box "System status is Normal" -t "STATUS" --output result.txt
> ```
