# ⚡ `nhash file type` - MIME Type & Header Inspector

Scans binary file headers (magic bytes) to verify actual MIME/file types regardless of extension.

---

## 🛠️ Usage Syntax

```bash
nhash file type [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<file>` | `string` | Path to the target file. | ✅ Yes |

### 💡 Live Terminal Examples

**Verify real file type of a document**:
```bash
nhash file type image.png
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file type image.png --output result.txt
> ```
