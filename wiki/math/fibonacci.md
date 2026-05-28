# ⚡ `nhash math fibonacci` - Fibonacci Generator

Generates a list containing the first N items in the Fibonacci sequence.

---

> [!NOTE]
> **Subcommand Aliases**: `fib`, `f`

## 🛠️ Usage Syntax

```bash
nhash math fibonacci [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--count` | `-c` | `int` | Count of Fibonacci numbers to generate. | `10` |

### 💡 Live Terminal Examples

**Generate the first 10 Fibonacci numbers**:
```bash
nhash math fibonacci --count 10
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash math fibonacci --count 10 --output result.txt
> ```
