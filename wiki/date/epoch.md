# ⚡ `nhash date epoch` - Unix Epoch Datetime Converter

Converts Unix epoch timestamps to human-readable dates, or human datetimes to Unix epoch.

---

## 🛠️ Usage Syntax

```bash
nhash date epoch [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<value>` | `string` | Unix epoch value or date-time string (if omitted, returns current epoch). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--ms` | `-m` | `bool` | Treat epoch value as milliseconds instead of seconds. | `false` |

### 💡 Live Terminal Examples

**Retrieve current Unix epoch timestamp**:
```bash
nhash date epoch
```

**Translate Unix epoch value to date**:
```bash
nhash date epoch 1716800000
```

**Convert standard datetime to epoch**:
```bash
nhash date epoch "2026-05-27 12:00:00"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date epoch --output result.txt
> ```
