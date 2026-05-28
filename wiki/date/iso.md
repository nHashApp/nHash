# ⚡ `nhash date iso` - ISO 8601 String Parser

Parses standard ISO 8601 datetime strings to return discrete components, offsets, and indicators.

---

## 🛠️ Usage Syntax

```bash
nhash date iso [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<value>` | `string` | ISO 8601 date-time string to parse. | ✅ Yes |

### 💡 Live Terminal Examples

**Parse ISO 8601 datetime string**:
```bash
nhash date iso "2026-05-27T12:30:00.000Z"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date iso "2026-05-27T12:30:00.000Z" --output result.txt
> ```
