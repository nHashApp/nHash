# 🆔 Unique Identifier Tools

Primary key generation tools: UUID versions, ULIDs, NanoIDs, CUID2s, Snowflakes, and TOTP verification.

---

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`uuid`](./uuid.md) | UUID / ULID / NanoID Generator | Generates random UUIDs/GUIDs (v1 to v8), ULIDs, or NanoIDs dynamically. |
| [`inspect`](./inspect.md) | UUID Metadata Inspector | Decodes an existing UUID/GUID to reveal internal version details, variant, and creation timestamp. |
| [`cuid`](./cuid.md) | Secure CUID2 Generator | Generates highly secure, next-generation CUID2 identifiers (collision-resistant). |
| [`snowflake`](./snowflake.md) | Snowflake ID Generator & Decoder | Generates or decodes Twitter-compatible 64-bit Snowflake IDs containing timestamps. |
| [`totp`](./totp.md) | TOTP MFA Generator | Generates a 6 or 8-digit Time-based One-Time Password (RFC 6238) from a secret with a countdown. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `id` (or one of its aliases):

```bash
nhash id [subcommand] [arguments] [options]
```
