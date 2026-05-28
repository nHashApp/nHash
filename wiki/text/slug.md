# ⚡ `nhash text slug` - URL Slug Creator

Sanitizes diacritics and special characters to build clean URL-friendly slugs.

---

> [!NOTE]
> **Subcommand Aliases**: `sl`

## 🛠️ Usage Syntax

```bash
nhash text slug [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | String to slugify. | ✅ Yes |

### 💡 Live Terminal Examples

**Generate clean URL slug**:
```bash
nhash text slug "Hello World! nHash is Awesome."
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text slug "Hello World! nHash is Awesome." --output result.txt
> ```
