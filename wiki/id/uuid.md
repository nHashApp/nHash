# ⚡ `nhash id uuid` - UUID / ULID / NanoID Generator

Generates random UUIDs/GUIDs (v1 to v8), ULIDs, or NanoIDs dynamically.

---

> [!NOTE]
> **Subcommand Aliases**: `u`

## 🛠️ Usage Syntax

```bash
nhash id uuid [arguments] [options]
```

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--version` | `-v` | `string` | Select UUID variant (v1, v2, v3, v4, v5, v7, v8, ulid, nanoid) or 'all'. | `all` |
| `--bracket` | `` | `bool` | Wrap generated UUID in curly brackets {}. | `false` |
| `--no-hyphen` | `` | `bool` | Remove hyphens from the generated UUID. | `false` |

### 💡 Live Terminal Examples

**Generate UUID v7 (Time-Ordered)**:
```bash
nhash id uuid -v v7
```

**Generate ULID**:
```bash
nhash id uuid -v ulid
```

**Generate all standard UUID variants**:
```bash
nhash id uuid
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash id uuid -v v7 --output result.txt
> ```
