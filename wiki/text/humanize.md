# ⚡ `nhash text humanize` - Developer String Humanizer

Translates camel/snake/kebab developer identifiers into space-separated readable titles.

---

## 🛠️ Usage Syntax

```bash
nhash text humanize [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The variable/field name to humanize. | ✅ Yes |

### 💡 Live Terminal Examples

**Humanize developer snake-case string**:
```bash
nhash text humanize "my_database_field_name"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text humanize "my_database_field_name" --output result.txt
> ```
