# ⚡ `nhash net port` - TCP Port Scanner

Tests TCP port connectivity and network socket response from a remote host.

---

## 🛠️ Usage Syntax

```bash
nhash net port [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<host>` | `string` | Host domain address or IP to scan. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--port` | `-p` | `int` | Target port to test. | `N/A` |

### 💡 Live Terminal Examples

**Scan port 443 on google.com**:
```bash
nhash net port google.com -p 443
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net port google.com -p 443 --output result.txt
> ```
