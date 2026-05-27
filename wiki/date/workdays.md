# ⚡ `nhash date workdays` - Business Workdays Calculator

Calculates the total business working days count (excluding standard weekends) between two dates.

---

## 🛠️ Usage Syntax

```bash
nhash date workdays [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<start>` | `string` | Start date string. | ✅ Yes |
| `<end>` | `string` | End date string. | ✅ Yes |

### 💡 Live Terminal Examples

**Count working days between dates**:
```bash
nhash date workdays "2026-05-01" "2026-05-27"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date workdays "2026-05-01" "2026-05-27" --output result.txt
> ```
