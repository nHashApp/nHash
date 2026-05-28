# ⚡ `nhash file integrity` - SHA-256 Sidecar Integrity Suite

Computes SHA-256 file checksums, verifies hashes, or writes/updates sidecar '.sha256' files.

---

## 🛠️ Usage Syntax

```bash
nhash file integrity [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<file>` | `string` | Path to the target file. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--hash` | `-h` | `string` | Expected SHA-256 hash to verify against. | `N/A` |

### 💡 Live Terminal Examples

**Compute hash and write sidecar .sha256 file**:
```bash
nhash file integrity document.pdf
```

**Verify document hash directly**:
```bash
nhash file integrity document.pdf -h "e3b0c442..."
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash file integrity document.pdf --output result.txt
> ```
