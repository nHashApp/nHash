# ⚡ `nhash sys process` - Process telemetry manager

Lists active background processes sorted by RAM memory consumption.

---

> [!NOTE]
> **Subcommand Aliases**: `ps`, `p`

## 🛠️ Usage Syntax

```bash
nhash sys process [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--count` | `-n` | `int` | Count of processes to display. | `10` |

### 💡 Live Terminal Examples

**Show top 10 heavy background processes**:
```bash
nhash sys process
```

**Show top 25 processes**:
```bash
nhash sys process -n 25
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash sys process --output result.txt
> ```
