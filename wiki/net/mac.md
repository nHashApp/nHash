# ⚡ `nhash net mac` - MAC Vendor Lookup

Resolves the hardware OUI vendor/manufacturer of a MAC address.

---

## 🛠️ Usage Syntax

```bash
nhash net mac [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<address>` | `string` | MAC address to lookup. | ✅ Yes |

### 💡 Live Terminal Examples

**Look up manufacturer for MAC address**:
```bash
nhash net mac "00:1A:2B:3C:4D:5E"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash net mac "00:1A:2B:3C:4D:5E" --output result.txt
> ```
