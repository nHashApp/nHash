# ⚡ `nhash file search` - Regex Grep File Scanner

Executes a fast regex pattern grep across all file contents in a target directory.

---

## 🛠️ Usage Syntax

```bash
nhash file search [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<directory>` | `string` | Directory path to scan (default: current directory). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--regex` | `-r` | `string` | Regular expression grep search pattern. | `N/A` |
| `--ext` | `-e` | `string` | Comma-separated extensions filter (e.g. cs,json). | `N/A` |

### 💡 Live Terminal Examples

**Grep all TODOs in C# files**:
```bash
nhash file search . -r "TODO.*" -e cs
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file search . -r "TODO.*" -e cs --output result.txt
> ```
