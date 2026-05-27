# ⚡ `nhash crypto hash checksum` - File Hash & Checksum Calculator

Calculates cryptographic hash fingerprint (MD5, SHA256, etc.) of a local file.

---

> [!NOTE]
> **Subcommand Aliases**: `checksum`

## 🛠️ Usage Syntax

```bash
nhash crypto hash checksum [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<file>` | `string` | Path to the local target file to scan. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--algo` | `-a` | `string` | Specific hashing algorithm (md5, sha1, sha256, sha384, sha512, crc32, crc8, murmur3) or 'all'. | `all` |

### 💡 Live Terminal Examples

**Calculate all checksums for a file**:
```bash
nhash crypto hash checksum document.pdf
```

**Calculate SHA-512 fingerprint of a zip archive**:
```bash
nhash crypto hash checksum backup.zip -a sha512
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto hash checksum document.pdf --output result.txt
> ```
