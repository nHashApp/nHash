# ⚡ `nhash convert encode bintext` - Binary, Octal, or Decimal Converter

Converts plain text to/from Binary, Octal, or Decimal values.

---

> [!NOTE]
> **Subcommand Aliases**: `bt`

## 🛠️ Usage Syntax

```bash
nhash convert encode bintext [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text to encode, or space-separated numbers to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--base` | `-b` | `int` | Numeric base: 2 (binary), 8 (octal), 10 (decimal). | `N/A` |
| `--decode` | `-d` | `bool` | Decode representation to plain text. | `false` |

### 💡 Live Terminal Examples

**Encode text 'hello' to Binary (Base 2)**:
```bash
nhash convert encode bintext "hello" -b 2
```

**Decode space-separated Decimal values to text**:
```bash
nhash convert encode bintext "104 101 108 108 111" -b 10 -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode bintext "hello" -b 2 --output result.txt
> ```
