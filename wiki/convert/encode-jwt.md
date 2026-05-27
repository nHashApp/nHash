# ⚡ `nhash convert encode jwt` - JSON Web Token (JWT) Decode & Build Tools

Decodes JWT token structures (Header, Payload) or builds unsigned JWTs.

---

> [!NOTE]
> **Subcommand Aliases**: `j`

## 🛠️ Usage Syntax

```bash
nhash convert encode jwt [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<token>` | `string` | JWT string to decode (only applicable to the decode subcommand). | ❌ No |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--header` | `-H` | `string` | JSON Header string (used in build). | `N/A` |
| `--payload` | `-p` | `string` | JSON Payload string (used in build). | `N/A` |
| `--no-summary` | `` | `bool` | Don't print human-readable summary metadata (only in decode). | `false` |

### 💡 Live Terminal Examples

**Decode a JWT token**:
```bash
nhash convert encode jwt decode eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**Build an unsigned JWT token from payload JSON**:
```bash
nhash conv e j build -p "{\"sub\":\"123\"}"
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode jwt decode eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c --output result.txt
> ```
