# ⚡ `nhash dev number` - Number Inspector & Analyzer

Inspects integers and floats to output representations across Decimal, Hex, Octal, Binary, and Scientific layouts.

---

> [!NOTE]
> **Subcommand Aliases**: `num`, `n`

## 🛠️ Usage Syntax

```bash
nhash dev number [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<value>` | `string` | Number value string to inspect. | ✅ Yes |

### 💡 Live Terminal Examples

**Inspect integer representation**:
```bash
nhash dev number "65535"
```

**Inspect binary numeric string**:
```bash
nhash dev number "101010"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash dev number "65535" --output result.txt
> ```
