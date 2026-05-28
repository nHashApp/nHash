# ⚡ `nhash text diff` - Text Diffing Tool

Highlights exact differences (line-by-line) between two files or strings.

---

## 🛠️ Usage Syntax

```bash
nhash text diff [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<start>` | `string` | Path to the first file (or raw string). | ✅ Yes |
| `<end>` | `string` | Path to the second file (or raw string). | ✅ Yes |

### 💡 Live Terminal Examples

**Diff two configuration files**:
```bash
nhash text diff old.json new.json
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text diff old.json new.json --output result.txt
> ```
