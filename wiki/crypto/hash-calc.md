# ⚡ `nhash crypto hash calc` - Direct Text Hash Calculator

Calculates direct cryptographic fingerprints (MD5, SHA1, SHA256, SHA384, SHA512, CRC32, CRC8, Murmur3) of an input string.

---

> [!NOTE]
> **Subcommand Aliases**: `calc`

## 🛠️ Usage Syntax

```bash
nhash crypto hash calc [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The raw text string to hash. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--algo` | `-a` | `string` | Specific hashing algorithm (md5, sha1, sha256, sha384, sha512, crc32, crc8, murmur3) or 'all'. | `all` |

### 💡 Live Terminal Examples

**Calculate all hashes for text**:
```bash
nhash crypto hash calc "test"
```

**Calculate only SHA-256 fingerprint**:
```bash
nhash crypto hash calc "test" -a sha256
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto hash calc "test" --output result.txt
> ```
