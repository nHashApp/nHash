# ⚡ `nhash math evaluate` - Arithmetic Expression Evaluator

Evaluates complex algebraic/arithmetic expressions (+, -, *, /, %, ^, sin, cos, tan, log) in the terminal.

---

> [!NOTE]
> **Subcommand Aliases**: `eval`, `calc`, `c`

## 🛠️ Usage Syntax

```bash
nhash math evaluate [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<expression>` | `string` | The arithmetic expression to evaluate (e.g. '2^3 * (4 - 1.5)'). | ✅ Yes |

### 💡 Live Terminal Examples

**Evaluate mathematical expression**:
```bash
nhash math evaluate "2^3 * (4.5 - 1.5)"
```

**Evaluate logarithmic expression**:
```bash
nhash math evaluate "log(100) + sin(0)"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash math evaluate "2^3 * (4.5 - 1.5)" --output result.txt
> ```
