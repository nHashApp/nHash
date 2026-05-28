# ⚡ `nhash dev cron` - Cron Expression Explainer

Explains standard Cron syntax in plain English and calculates the next N estimated execution schedules.

---

> [!NOTE]
> **Subcommand Aliases**: `cr`

## 🛠️ Usage Syntax

```bash
nhash dev cron [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<expression>` | `string` | Cron expression to parse (e.g. '*/5 12 * * 1-5'). | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--count` | `-c` | `int` | Number of next execution times to calculate. | `5` |

### 💡 Live Terminal Examples

**Translate cron expression and print next 5 matches**:
```bash
nhash dev cron "*/5 12 * * 1-5"
```

**Print next 10 executions for a standard cron expression**:
```bash
nhash dev cron "0 0 1 1 *" -c 10
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash dev cron "*/5 12 * * 1-5" --output result.txt
> ```
