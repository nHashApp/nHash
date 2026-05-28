# ⚡ `nhash text yaml` - YAML Formatting & Minification Suite

Parses, prettifies, minifies, or validates YAML string documents.

---

## 🛠️ Usage Syntax

```bash
nhash text yaml [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<input>` | `string` | YAML string document to process. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--minify` | `-m` | `bool` | Minify the YAML document. | `false` |
| `--validate` | `-v` | `bool` | Validate YAML syntax. | `false` |

### 💡 Live Terminal Examples

**Prettify YAML document**:
```bash
nhash text yaml "app: nHash\nversion: 1.0"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text yaml "app: nHash\nversion: 1.0" --output result.txt
> ```
