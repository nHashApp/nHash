# ⚡ `nhash text count` - Phrase / Regex Matches Counter

Counts the occurrence of exact phrases or regular expressions in text.

---

> [!NOTE]
> **Subcommand Aliases**: `cnt`

## 🛠️ Usage Syntax

```bash
nhash text count [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The string payload. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--pattern` | `-p` | `string` | Phrase or regex to find and count. | `N/A` |

### 💡 Live Terminal Examples

**Count occurrences of the word 'hello'**:
```bash
nhash text count "hello world hello" -p "hello"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text count "hello world hello" -p "hello" --output result.txt
> ```
