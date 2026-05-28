# ⚡ `nhash date diff` - Date Interval & Duration Calculator

Evaluates precise elapsed time intervals, durations, and days count between two input dates.

---

## 🛠️ Usage Syntax

```bash
nhash date diff [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<start>` | `string` | Start date-time string. | ✅ Yes |
| `<end>` | `string` | End date-time string. | ✅ Yes |

### 💡 Live Terminal Examples

**Compute exact duration between two dates**:
```bash
nhash date diff "2026-05-01" "2026-05-27"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date diff "2026-05-01" "2026-05-27" --output result.txt
> ```
