# ⚡ `nhash net cidr` - CIDR Subnet Calculator

Calculates IP boundaries, subnets, masks, broadcasts, and hosts counts from a CIDR notation.

---

## 🛠️ Usage Syntax

```bash
nhash net cidr [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<cidr>` | `string` | CIDR notation (e.g. 192.168.1.0/24). | ✅ Yes |

### 💡 Live Terminal Examples

**Calculate details for 192.168.1.0/24**:
```bash
nhash net cidr "192.168.1.0/24"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net cidr "192.168.1.0/24" --output result.txt
> ```
