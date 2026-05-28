# ⚡ `nhash net whois` - Domain WHOIS Lookup

Retrieves registration and ownership metadata from WHOIS registry servers.

---

## 🛠️ Usage Syntax

```bash
nhash net whois [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<domain>` | `string` | Domain name to query WHOIS registry info. | ✅ Yes |

### 💡 Live Terminal Examples

**Lookup WHOIS details for google.com**:
```bash
nhash net whois google.com
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net whois google.com --output result.txt
> ```
