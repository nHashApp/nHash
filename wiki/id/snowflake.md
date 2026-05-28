# ⚡ `nhash id snowflake` - Snowflake ID Generator & Decoder

Generates or decodes Twitter-compatible 64-bit Snowflake IDs containing timestamps.

---

## 🛠️ Usage Syntax

```bash
nhash id snowflake [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<id>` | `string` | Optional Snowflake ID string to decode and extract metadata from. | ❌ No |

### 💡 Live Terminal Examples

**Generate a new Snowflake ID**:
```bash
nhash id snowflake
```

**Decode Snowflake ID metadata**:
```bash
nhash id snowflake 1541234567890
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash id snowflake --output result.txt
> ```
