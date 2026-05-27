# ⚡ `nhash file tree` - Directory Structure Tree Builder

Renders an elegant, terminal-based visual directory tree with depth limits and file sizes.

---

## 🛠️ Usage Syntax

```bash
nhash file tree [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<directory>` | `string` | Directory path to render (default: current directory). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--depth` | `-d` | `int` | Maximum recursion depth levels. | `3` |
| `--sizes` | `-s` | `bool` | Show formatted file sizes in tree rendering. | `false` |

### 💡 Live Terminal Examples

**Render current directory tree**:
```bash
nhash file tree .
```

**Render tree with recursion depth 2 showing sizes**:
```bash
nhash file tree . -d 2 -s
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file tree . --output result.txt
> ```
