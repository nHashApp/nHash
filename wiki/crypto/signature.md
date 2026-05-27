# ⚡ `nhash crypto signature` - Asymmetric RSA Signature Suite

Handles secure PKCS#8 private/public RSA key pair generation, data signing, and signature verification.

---

> [!NOTE]
> **Subcommand Aliases**: `sig`

## 🛠️ Usage Syntax

```bash
nhash crypto signature [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Data string to sign or verify (only applicable to sign/verify subcommands). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--key-size` | `-s` | `int` | RSA key modulus size (1024, 2048, 4048) (used in keygen). | `2048` |
| `--key` | `-k` | `string` | Path to the RSA private key (for sign) or public key (for verify) PEM file. | `N/A` |
| `--signature` | `-g` | `string` | The signature hex string to verify against (used in verify). | `N/A` |

### 💡 Live Terminal Examples

**Generate a secure 2048-bit RSA keypair**:
```bash
nhash crypto signature keygen -s 2048
```

**Sign text using RSA private key**:
```bash
nhash crypto signature sign "sensitive text" -k private.pem
```

**Verify RSA signature with public key**:
```bash
nhash crypto signature verify "sensitive text" -k public.pem -g <HEX_SIGNATURE>
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash crypto signature keygen -s 2048 --output result.txt
> ```
