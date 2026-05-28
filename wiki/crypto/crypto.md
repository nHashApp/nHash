# 🔒 High-Security Cryptography

Advanced security utilities: hash calculation, HMAC generators, symmetric key encryption, key-pair creation, and password managers.

---

> [!NOTE]
> **Category Aliases**: `cr`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`cipher`](./cipher.md) | Symmetric Byte Cipher Encrypter / Decrypter | Symmetric encryption/decryption supporting secure AES-GCM and ChaCha20-Poly1305. |
| [`hash-calc`](./hash-calc.md) | Direct Text Hash Calculator | Calculates direct cryptographic fingerprints (MD5, SHA1, SHA256, SHA384, SHA512, CRC32, CRC8, Murmur3) of an input string. |
| [`hash-checksum`](./hash-checksum.md) | File Hash & Checksum Calculator | Calculates cryptographic hash fingerprint (MD5, SHA256, etc.) of a local file. |
| [`hmac`](./hmac.md) | Keyed-Hash Message Authentication Code (HMAC) | Generates HMAC hashes using a secret security key across MD5, SHA-1, SHA-256, and SHA-512 engines. |
| [`password`](./password.md) | Secure Password Generator / Strength Checker | Generates strong, cryptographically secure passwords or checks password security levels. |
| [`signature`](./signature.md) | Asymmetric RSA Signature Suite | Handles secure PKCS#8 private/public RSA key pair generation, data signing, and signature verification. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `crypto` (or one of its aliases):

```bash
nhash crypto [subcommand] [arguments] [options]
```
