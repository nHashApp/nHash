# ⚡ Data Conversion & Formatting

Utilities to format structured configurations and perform base/text representations encoders.

---

> [!NOTE]
> **Category Aliases**: `conv`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`base-n`](./base-n.md) | Arbitrary Base N Numerical Converter | Converts arbitrary numerical strings between bases 2 to 36 accurately. |
| [`format`](./format.md) | Structured Data Converter | Formats and converts direct structured config files/texts (JSON ↔ YAML ↔ XML). |
| [`encode-base32`](./encode-base32.md) | Base32 Text Encoder / Decoder | Encodes or decodes text according to Base32 representation standards. |
| [`encode-base36`](./encode-base36.md) | Base36 Text Encoder / Decoder | Encodes or decodes text according to Base36 representation standards. |
| [`encode-base45`](./encode-base45.md) | Base45 Text Encoder / Decoder | Encodes or decodes text using RFC 9285 Base45 standard (commonly used in QR Codes and health certificates). |
| [`encode-base58`](./encode-base58.md) | Base58 Text Encoder / Decoder | Encodes or decodes text according to Base58 representation standards (frequently used in Bitcoin address formats). |
| [`encode-base62`](./encode-base62.md) | Base62 Text Encoder / Decoder | Encodes or decodes text according to Base62 representation standards (commonly used for clean URL shorteners). |
| [`encode-base64`](./encode-base64.md) | Base64 Text Encoder / Decoder | Encodes or decodes text according to standard RFC 4648 Base64 representation. |
| [`encode-base85`](./encode-base85.md) | Base85 Text Encoder / Decoder | Encodes or decodes text according to Base85 (Ascii85) representation standards. |
| [`encode-base91`](./encode-base91.md) | Base91 Text Encoder / Decoder | Encodes or decodes text using standard basE91 algorithm. |
| [`encode-bintext`](./encode-bintext.md) | Binary, Octal, or Decimal Converter | Converts plain text to/from Binary, Octal, or Decimal values. |
| [`encode-hex`](./encode-hex.md) | Hexadecimal (Base16) Encoder / Decoder | Encodes or decodes text to/from hexadecimal bytes format. |
| [`encode-html`](./encode-html.md) | HTML Entity Encoder / Decoder | Performs standard HTML escaping (entities) or unescapes it. |
| [`encode-jwt`](./encode-jwt.md) | JSON Web Token (JWT) Decode & Build Tools | Decodes JWT token structures (Header, Payload) or builds unsigned JWTs. |
| [`encode-morse`](./encode-morse.md) | Morse Code Encoder / Decoder | Encodes standard text characters to Morse code dots and dashes, or decodes Morse code back. |
| [`encode-punycode`](./encode-punycode.md) | Punycode (IDN Domain) Encoder / Decoder | Translates internationalized domain names (containing non-ASCII chars) to punycode format (RFC 3492). |
| [`encode-rot13`](./encode-rot13.md) | ROT13 (Caesar Cipher) Rotator | Rotates characters by N positions (default 13, Caesar shift) across the alphabet. |
| [`encode-url`](./encode-url.md) | URL Percent Encoder / Decoder | Encodes text characters into standard Percent-encoded representations, or decodes them. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `convert` (or one of its aliases):

```bash
nhash convert [subcommand] [arguments] [options]
```
