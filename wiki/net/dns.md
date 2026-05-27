# ⚡ `nhash net dns` - DNS Record Lookup

Resolves DNS registry records (A, AAAA, MX, TXT, CNAME, ANY) for a host domain.

---

## 🛠️ Usage Syntax

```bash
nhash net dns [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<hostname>` | `string` | Hostname/Domain to resolve. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--type` | `-t` | `string` | DNS query record type (A, AAAA, MX, TXT, ANY). | `A` |

### 💡 Live Terminal Examples

**Resolve 'google.com' A records**:
```bash
nhash net dns google.com
```

**Query TXT records for a domain**:
```bash
nhash net dns google.com -t TXT
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net dns google.com --output result.txt
> ```
