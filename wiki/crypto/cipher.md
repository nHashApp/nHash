# ⚡ `nhash crypto cipher` - Symmetric Byte Cipher Encrypter / Decrypter

Symmetric encryption/decryption supporting secure AES-GCM and ChaCha20-Poly1305.

---

## 🛠️ Usage Syntax

```bash
nhash crypto cipher [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text payload to encrypt or ciphertext hex to decrypt. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--pass` | `-p` | `string` | Password/Passphrase for PBKDF2 key derivation. | `N/A` |
| `--type` | `-t` | `string` | Cipher type: aes (AES-GCM), chacha (ChaCha20-Poly1305). | `aes` |
| `--decrypt` | `-d` | `bool` | Decrypt the payload instead of encrypting. | `false` |

### 💡 Live Terminal Examples

**Encrypt text using AES-GCM**:
```bash
nhash crypto cipher "sensitive data" -p "strongPassword"
```

**Decrypt ciphertext hex string using AES-GCM**:
```bash
nhash crypto cipher <HEX_STRING> -p "strongPassword" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto cipher "sensitive data" -p "strongPassword" --output result.txt
> ```
