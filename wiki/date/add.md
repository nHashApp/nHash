# ⚡ `nhash date add` - Complex Date Algebra Calculator

Performs complex duration additions/subtractions dynamically on top of standard datetimes.

---

## 🛠️ Usage Syntax

```bash
nhash date add [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<datetime>` | `string` | Base starting date-time string. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--duration` | `-d` | `string` | Algebraic duration expression (e.g. +30d, -2h, +1y, +2w). | `N/A` |

### 💡 Live Terminal Examples

**Add 30 days to date-time**:
```bash
nhash date add "2026-05-27" -d "+30d"
```

**Subtract 3 hours and add 2 weeks**:
```bash
nhash date add "2026-05-27 12:00:00" -d "-3h+2w"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date add "2026-05-27" -d "+30d" --output result.txt
> ```
