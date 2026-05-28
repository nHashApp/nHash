# ⚡ `nhash text escape` - Data Layout Escaper / Unescaper

Escapes or unescapes strings for layouts including JSON, C#, SQL, HTML, URL, and XML.

---

> [!NOTE]
> **Subcommand Aliases**: `esc`

## 🛠️ Usage Syntax

```bash
nhash text escape [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The text to escape or unescape. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--type` | `-l` | `string` | Format style: json, csharp, sql, xml, html, url. | `N/A` |
| `--unescape` | `-u` | `bool` | Unescape the payload instead of escaping. | `false` |

### 💡 Live Terminal Examples

**Escape string for SQL query statements**:
```bash
nhash text escape "John's Store" -l sql
```

**Unescape JSON string payload**:
```bash
nhash text escape "{\"key\":\"value\"}" -l json -u
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text escape "John's Store" -l sql --output result.txt
> ```
