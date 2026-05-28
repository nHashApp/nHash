# ⚡ `nhash file dedup` - Duplicate File Scanner

Scans directories for identical files using fast size-grouping followed by SHA-256 checks.

---

## 🛠️ Usage Syntax

```bash
nhash file dedup [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<directory>` | `string` | Directory path to scan (default: current directory). | ❌ No |

### 💡 Live Terminal Examples

**Find duplicates in workspace**:
```bash
nhash file dedup .
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file dedup . --output result.txt
> ```
