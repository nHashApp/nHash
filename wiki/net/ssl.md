# ⚡ `nhash net ssl` - SSL Certificate Inspector

Inspects SSL/TLS security certificates for a domain and displays validation/expiry details.

---

## 🛠️ Usage Syntax

```bash
nhash net ssl [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<hostname>` | `string` | Hostname/Domain to inspect SSL cert. | ✅ Yes |

### 💡 Live Terminal Examples

**Inspect 'google.com' SSL cert**:
```bash
nhash net ssl google.com
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net ssl google.com --output result.txt
> ```
