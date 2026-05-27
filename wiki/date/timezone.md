# ⚡ `nhash date timezone` - Global Time Zone Converter

Converts date-time inputs dynamically from a source timezone ID to a target timezone ID.

---

## 🛠️ Usage Syntax

```bash
nhash date timezone [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<date>` | `string` | Date-time string to convert. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--from` | `-f` | `string` | Source timezone ID (e.g. UTC, 'Iran Standard Time', 'Eastern Standard Time'). | `UTC` |
| `--to` | `-t` | `string` | Target timezone ID (e.g. 'Iran Standard Time', 'Pacific Standard Time'). | `UTC` |

### 💡 Live Terminal Examples

**Convert UTC time to Iran Standard Time**:
```bash
nhash date timezone "2026-05-27 12:00:00" -f UTC -t "Iran Standard Time"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date timezone "2026-05-27 12:00:00" -f UTC -t "Iran Standard Time" --output result.txt
> ```
