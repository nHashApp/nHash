# ⚡ `nhash convert encode rot13` - ROT13 (Caesar Cipher) Rotator

Rotates characters by N positions (default 13, Caesar shift) across the alphabet.

---

> [!NOTE]
> **Subcommand Aliases**: `rot`

## 🛠️ Usage Syntax

```bash
nhash convert encode rot13 [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The text string to shift. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--shift` | `-s` | `int` | Shift indices offset count (can be positive/negative). | `13` |

### 💡 Live Terminal Examples

**Rotate text by default 13 spaces**:
```bash
nhash convert encode rot13 "hello"
```

**Perform Caesar shift with custom offset 5**:
```bash
nhash convert encode rot13 "hello" -s 5
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode rot13 "hello" --output result.txt
> ```
