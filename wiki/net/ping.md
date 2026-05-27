# ⚡ `nhash net ping` - HTTP Latency Ping

Performs raw HTTP request ping to measure response latency and returns HTTP response headers.

---

## 🛠️ Usage Syntax

```bash
nhash net ping [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<url>` | `string` | URL or host to ping. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--timeout` | `-t` | `int` | Connection timeout in seconds. | `10` |

### 💡 Live Terminal Examples

**HTTP ping google**:
```bash
nhash net ping https://www.google.com
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net ping https://www.google.com --output result.txt
> ```
