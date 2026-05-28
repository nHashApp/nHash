# ⚡ `nhash dev regex` - Regex Match Tester

Tests regular expressions in real-time against custom strings and outputs matching indices and capturing groups.

---

> [!NOTE]
> **Subcommand Aliases**: `rg`

## 🛠️ Usage Syntax

```bash
nhash dev regex [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<input>` | `string` | Input text to run regex matches against. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--pattern` | `-p` | `string` | Regular expression pattern string. | `N/A` |

### 💡 Live Terminal Examples

**Test email matching regex**:
```bash
nhash dev regex "naser@example.com" -p "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash dev regex "naser@example.com" -p "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" --output result.txt
> ```
