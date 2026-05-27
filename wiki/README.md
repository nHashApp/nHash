# ⚡ nHash CLI Technical Wiki

Welcome to the official technical wiki for **nHash** - a highly optimized, fully modular, and premium command-line utility. 

---

This wiki covers detailed syntax, input arguments, option parameters, and real-world examples for all **11 category modules** and their **78 specialized subcommands**.

### 🗺️ Modules & Subcommands Index

| Category Module | Description & Link | Quick Subcommand Access |
| :--- | :--- | :--- |
| [**`art`**](./art/art.md) | 🎨 Art & Visual Utilities | [`ascii`](./art/ascii.md), [`box`](./art/box.md), [`table`](./art/table.md), [`gradient`](./art/gradient.md) |
| [**`convert`**](./convert/convert.md) | ⚡ Data Conversion & Formatting | [`base-n`](./convert/base-n.md), [`format`](./convert/format.md), [`encode-base32`](./convert/encode-base32.md), [`encode-base36`](./convert/encode-base36.md), [`encode-base45`](./convert/encode-base45.md), [`encode-base58`](./convert/encode-base58.md), [`encode-base62`](./convert/encode-base62.md), [`encode-base64`](./convert/encode-base64.md), [`encode-base85`](./convert/encode-base85.md), [`encode-base91`](./convert/encode-base91.md), [`encode-bintext`](./convert/encode-bintext.md), [`encode-hex`](./convert/encode-hex.md), [`encode-html`](./convert/encode-html.md), [`encode-jwt`](./convert/encode-jwt.md), [`encode-morse`](./convert/encode-morse.md), [`encode-punycode`](./convert/encode-punycode.md), [`encode-rot13`](./convert/encode-rot13.md), [`encode-url`](./convert/encode-url.md) |
| [**`crypto`**](./crypto/crypto.md) | 🔒 High-Security Cryptography | [`cipher`](./crypto/cipher.md), [`hash-calc`](./crypto/hash-calc.md), [`hash-checksum`](./crypto/hash-checksum.md), [`hmac`](./crypto/hmac.md), [`password`](./crypto/password.md), [`signature`](./crypto/signature.md) |
| [**`date`**](./date/date.md) | 📅 Calendar & Datetime Calculations | [`epoch`](./date/epoch.md), [`convert`](./date/convert.md), [`diff`](./date/diff.md), [`timezone`](./date/timezone.md), [`iso`](./date/iso.md), [`add`](./date/add.md), [`workdays`](./date/workdays.md) |
| [**`dev`**](./dev/dev.md) | 💻 Developer Diagnostics | [`cron`](./dev/cron.md), [`regex`](./dev/regex.md), [`color`](./dev/color.md), [`semver`](./dev/semver.md), [`number`](./dev/number.md) |
| [**`file`**](./file/file.md) | 📁 Filesystem Operations | [`type`](./file/type.md), [`tree`](./file/tree.md), [`dedup`](./file/dedup.md), [`search`](./file/search.md), [`rename`](./file/rename.md), [`integrity`](./file/integrity.md) |
| [**`id`**](./id/id.md) | 🆔 Unique Identifier Tools | [`uuid`](./id/uuid.md), [`inspect`](./id/inspect.md), [`cuid`](./id/cuid.md), [`snowflake`](./id/snowflake.md), [`totp`](./id/totp.md) |
| [**`math`**](./math/math.md) | 🧮 Calculator & Solvers | [`evaluate`](./math/evaluate.md), [`prime`](./math/prime.md), [`fibonacci`](./math/fibonacci.md), [`factor`](./math/factor.md) |
| [**`net`**](./net/net.md) | 🌐 Network Analysis Utilities | [`ip`](./net/ip.md), [`dns`](./net/dns.md), [`port`](./net/port.md), [`whois`](./net/whois.md), [`ping`](./net/ping.md), [`ssl`](./net/ssl.md), [`cidr`](./net/cidr.md), [`mac`](./net/mac.md) |
| [**`sys`**](./sys/sys.md) | 🖥️ System Diagnostics | [`info`](./sys/info.md), [`env`](./sys/env.md), [`process`](./sys/process.md) |
| [**`text`**](./text/text.md) | 📝 Text & String Processors | [`case`](./text/case.md), [`stats`](./text/stats.md), [`wordfreq`](./text/wordfreq.md), [`count`](./text/count.md), [`escape`](./text/escape.md), [`diff`](./text/diff.md), [`palindrome`](./text/palindrome.md), [`slug`](./text/slug.md), [`humanize`](./text/humanize.md), [`lorem`](./text/lorem.md), [`json`](./text/json.md), [`yaml`](./text/yaml.md), [`xml`](./text/xml.md) |

## 🛠️ Global Execution Tips

### 💾 Saving Output to a File
All commands natively support the `--output` or `-o` option to dump terminal outputs directly into a text file:
```bash
nhash dev color "#FF5733" --output color_swatch.txt
```

### 🧩 Command Aliases
For fast workflows, use short module and command aliases (e.g. `e` for `encode`, `b64` for `base64`, `t` for `text`):
```bash
nhash conv e b64 "hello"
```
