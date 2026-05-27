# ⚡ `nhash text xml` - XML Document Prettifier & Minifier

Parses, prettifies, minifies, or validates XML string documents.

---

## 🛠️ Usage Syntax

```bash
nhash text xml [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<input>` | `string` | XML string document to process. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--minify` | `-m` | `bool` | Minify the XML document. | `false` |
| `--validate` | `-v` | `bool` | Validate XML syntax. | `false` |

### 💡 Live Terminal Examples

**Prettify XML document**:
```bash
nhash text xml "<root><app>nHash</app></root>"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text xml "<root><app>nHash</app></root>" --output result.txt
> ```
