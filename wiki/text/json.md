# ⚡ `nhash text json` - JSON Schema Check & Format Suite

Parses, prettifies, minifies, or validates JSON string documents.

---

## 🛠️ Usage Syntax

```bash
nhash text json [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<input>` | `string` | JSON string document to process. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--minify` | `-m` | `bool` | Minify the JSON string instead of prettifying. | `false` |
| `--validate` | `-v` | `bool` | Validate JSON string syntax. | `false` |

### 💡 Live Terminal Examples

**Prettify JSON**:
```bash
nhash text json "{\"a\":1,\"b\":2}"
```

**Minify JSON and validate**:
```bash
nhash text json "{ \"a\": 1 }" --minify --validate
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text json "{\"a\":1,\"b\":2}" --output result.txt
> ```
