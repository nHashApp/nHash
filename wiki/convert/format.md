# ⚡ `nhash convert format` - Structured Data Converter

Formats and converts direct structured config files/texts (JSON ↔ YAML ↔ XML).

---

## 🛠️ Usage Syntax

```bash
nhash convert format [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<input>` | `string` | The structured string content to convert. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--from` | `-f` | `string` | Source format (json, yaml, xml). | `json` |
| `--to` | `-t` | `string` | Target format (json, yaml, xml). | `yaml` |

### 💡 Live Terminal Examples

**Convert JSON to YAML representation**:
```bash
nhash convert format "{\"app\": \"nHash\", \"version\": 1.0}" -f json -t yaml
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert format "{\"app\": \"nHash\", \"version\": 1.0}" -f json -t yaml --output result.txt
> ```
