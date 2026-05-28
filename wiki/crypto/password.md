# ⚡ `nhash crypto password` - Secure Password Generator / Strength Checker

Generates strong, cryptographically secure passwords or checks password security levels.

---

## 🛠️ Usage Syntax

```bash
nhash crypto password [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--length` | `-l` | `int` | Password length to generate. | `16` |
| `--no-upper` | `` | `bool` | Exclude uppercase alphabets (A-Z). | `false` |
| `--no-lower` | `` | `bool` | Exclude lowercase alphabets (a-z). | `false` |
| `--no-digit` | `` | `bool` | Exclude numeric digits (0-9). | `false` |
| `--no-special` | `` | `bool` | Exclude punctuation/special symbols. | `false` |
| `--check` | `-c` | `string` | Pass an existing password string to evaluate its strength and entropy levels. | `N/A` |

### 💡 Live Terminal Examples

**Generate strong secure password (default 16 chars)**:
```bash
nhash crypto password
```

**Generate a 32-character numeric/lowercase password**:
```bash
nhash crypto password -l 32 --no-upper --no-special
```

**Check strength of a password**:
```bash
nhash crypto password -c "Admin@123456"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto password --output result.txt
> ```
