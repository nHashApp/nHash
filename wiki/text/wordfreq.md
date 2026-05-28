# ⚡ `nhash text wordfreq` - Word Frequency Analyzer

Renders a table displaying the top N most frequent words in a text payload.

---

> [!NOTE]
> **Subcommand Aliases**: `wf`

## 🛠️ Usage Syntax

```bash
nhash text wordfreq [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The raw text string. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--count` | `-c` | `int` | Display limit count. | `10` |

### 💡 Live Terminal Examples

**Calculate word frequency table**:
```bash
nhash text wordfreq "hello world hello coding world hello"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text wordfreq "hello world hello coding world hello" --output result.txt
> ```
