# ⚡ `nhash crypto hmac` - Keyed-Hash Message Authentication Code (HMAC)

Generates HMAC hashes using a secret security key across MD5, SHA-1, SHA-256, and SHA-512 engines.

---

## 🛠️ Usage Syntax

```bash
nhash crypto hmac [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Plain input text to hash. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--key` | `-k` | `string` | Secret key used to calculate HMAC. | `N/A` |
| `--algo` | `-a` | `string` | HMAC engine (md5, sha1, sha256, sha512) or 'all'. | `all` |

### 💡 Live Terminal Examples

**Calculate HMAC for text using a secret key**:
```bash
nhash crypto hmac "message data" -k "secret_key"
```

**Calculate HMAC-SHA256 only**:
```bash
nhash crypto hmac "message data" -k "secret_key" -a sha256
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto hmac "message data" -k "secret_key" --output result.txt
> ```
