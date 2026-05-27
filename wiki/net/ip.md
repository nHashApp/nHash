# ⚡ `nhash net ip` - IP Address Resolver

Queries internal network IPs or external public IP address along with geographic locator info.

---

## 🛠️ Usage Syntax

```bash
nhash net ip [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--external` | `-e` | `bool` | Query external IP address and geo location instead of local interface. | `false` |

### 💡 Live Terminal Examples

**Query internal interface IP**:
```bash
nhash net ip
```

**Query external public IP and geolocation**:
```bash
nhash net ip -e
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net ip --output result.txt
> ```
