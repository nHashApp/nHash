# ⚡ `nhash id totp` - TOTP MFA Generator

Generates a 6 or 8-digit Time-based One-Time Password (RFC 6238) from a secret with a countdown.

---

> [!NOTE]
> **Subcommand Aliases**: `otp`

## 🛠️ Usage Syntax

```bash
nhash id totp [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<secret>` | `string` | Secret key (Base32 format) to calculate TOTP. | ✅ Yes |

### 💡 Live Terminal Examples

**Generate TOTP code from secret**:
```bash
nhash id totp JBSWY3DPEHPK3PXP
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash id totp JBSWY3DPEHPK3PXP --output result.txt
> ```
