# ⚡ `nhash id inspect` - UUID Metadata Inspector

Decodes an existing UUID/GUID to reveal internal version details, variant, and creation timestamp.

---

> [!NOTE]
> **Subcommand Aliases**: `ins`

## 🛠️ Usage Syntax

```bash
nhash id inspect [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<uuid>` | `string` | The target UUID string to inspect. | ✅ Yes |

### 💡 Live Terminal Examples

**Inspect metadata inside a UUID v7**:
```bash
nhash id inspect 018fb5d8-c100-7000-8000-010203040506
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash id inspect 018fb5d8-c100-7000-8000-010203040506 --output result.txt
> ```
