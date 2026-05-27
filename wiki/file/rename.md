# ⚡ `nhash file rename` - Batch Regex File Renamer

Performs fast batch file renames in a directory using regex replacements with visual safe previews.

---

## 🛠️ Usage Syntax

```bash
nhash file rename [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<directory>` | `string` | Directory path to operate in (default: current). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--pattern` | `-p` | `string` | Regex matching pattern for filenames. | `N/A` |
| `--replace` | `-r` | `string` | Replacement text string (supports capture groups like $1). | `N/A` |
| `--preview` | `-v` | `bool` | Preview changes without actually renaming files. | `true` |
| `--ext` | `-e` | `string` | Extensions filter. | `N/A` |

### 💡 Live Terminal Examples

**Preview renaming v1 files to v2**:
```bash
nhash file rename . -p "v1_(.*)" -r "v2_$1" --preview
```

**Perform batch rename in current folder**:
```bash
nhash file rename . -p "doc_(.*)" -r "paper_$1" -v false
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file rename . -p "v1_(.*)" -r "v2_$1" --preview --output result.txt
> ```
