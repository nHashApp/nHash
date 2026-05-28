# ⚡ `nhash text stats` - String Metrics Analyzer

Counts characters, words, sentences, spaces, lines, and raw byte sizes.

---

## 🛠️ Usage Syntax

```bash
nhash text stats [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text payload to evaluate. | ✅ Yes |

### 💡 Live Terminal Examples

**Inspect text stats**:
```bash
nhash text stats "This is a sample sentence. nHash is awesome!"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text stats "This is a sample sentence. nHash is awesome!" --output result.txt
> ```
