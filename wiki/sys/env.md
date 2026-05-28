# ⚡ `nhash sys env` - Environment Variables Viewer

Lists and filters active system Environment Variables.

---

> [!NOTE]
> **Subcommand Aliases**: `e`

## 🛠️ Usage Syntax

```bash
nhash sys env [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--filter` | `-f` | `string` | Filter environment variables by key/value matches. | `N/A` |

### 💡 Live Terminal Examples

**List all active env-vars**:
```bash
nhash sys env
```

**Filter variables containing PATH**:
```bash
nhash sys env -f PATH
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash sys env --output result.txt
> ```
