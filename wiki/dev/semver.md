# ⚡ `nhash dev semver` - Semantic Version Comparator

Compares two semantic versions (SemVer 2.0) logically and determines version hierarchy.

---

> [!NOTE]
> **Subcommand Aliases**: `sv`

## 🛠️ Usage Syntax

```bash
nhash dev semver [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<version1>` | `string` | First semantic version string (e.g. 1.0.0-beta). | ✅ Yes |
| `<version2>` | `string` | Second semantic version string (e.g. 1.0.0). | ✅ Yes |

### 💡 Live Terminal Examples

**Compare beta with stable release**:
```bash
nhash dev semver "1.0.0-beta" "1.0.0"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash dev semver "1.0.0-beta" "1.0.0" --output result.txt
> ```
