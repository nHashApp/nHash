# ⚡ `nhash art table` - CSV to Terminal Grid Renderer

Translates raw comma-separated values (CSV) into truecolor styled terminal grids and tables.

---

## 🛠️ Usage Syntax

```bash
nhash art table [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<data>` | `string` | CSV formatted string (the first row is treated as headers, subsequent rows are data). | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--border` | `-b` | `string` | Border style: rounded, double, heavy, ascii. | `rounded` |

### 💡 Live Terminal Examples

**Render simple CSV as a beautiful table**:
```bash
nhash art table "Name,Role,Status\nNaser,Author,Active\nAI,Agent,Processing"
```

**Render CSV with double edge border**:
```bash
nhash art table "Metric,Value\nCPU,12%\nRAM,45%" -b double
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash art table "Name,Role,Status\nNaser,Author,Active\nAI,Agent,Processing" --output result.txt
> ```
