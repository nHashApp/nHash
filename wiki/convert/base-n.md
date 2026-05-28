# ⚡ `nhash convert base n` - Arbitrary Base N Numerical Converter

Converts arbitrary numerical strings between bases 2 to 36 accurately.

---

## 🛠️ Usage Syntax

```bash
nhash convert base n [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<number>` | `string` | The numeric string to convert. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--from` | `-f` | `int` | Source base range (2-36). | `10` |
| `--to` | `-t` | `int` | Target base range (2-36). | `16` |

### 💡 Live Terminal Examples

**Convert a binary number to hexadecimal**:
```bash
nhash convert base-n 101010 -f 2 -t 16
```

**Convert a decimal number to base-36**:
```bash
nhash convert base-n 123456789 -f 10 -t 36
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert base-n 101010 -f 2 -t 16 --output result.txt
> ```
