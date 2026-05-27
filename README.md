# ⚡ nHash - Ultra-Fast, Modular CLI Developer & Cryptographic Utility

**nHash** is a highly optimized, fully modular, and premium command-line utility designed for software engineers, security professionals, and power users. Built from the ground up to be **100% Native AOT (Ahead-of-Time) compilation compatible** under **.NET 10.0**, nHash delivers instantaneous execution, zero startup latency, and minimal memory usage without JIT compilation overhead.

It uses a sleek, harmoniously designed terminal user experience powered by `Spectre.Console` and `System.CommandLine`, offering colorful reports, truecolor visual previews, and smooth interactive outputs.

---

## 🚀 Key Features

*   **⚡ Blazing Fast & Native AOT Ready**: Instant CLI invocation, lightweight footprints, zero reflection runtime execution.
*   **🧩 10 Clean-Architecture Modules**: Extensively structured into logically independent subdomains.
*   **🎨 Premium Visual Design**: Implements truecolor gradient formatting, beautiful panel boxes, and tabular structures.
*   **💾 Global Output Redirection**: All commands natively support saving output to a file using the recursive `--output` / `-o` option.

---

## 🗺️ Command Hierarchy & Modules

The application is structured into **10 root-level commands** with specialized subcommands and ultra-short aliases:

```
nhash
 ├── convert (conv)       - Base & structured text encoders/formatters
 ├── crypto (cr)          - Security hashes, asymmetric & symmetric ciphers
 ├── id                   - Database identifiers (UUID, Snowflake, CUID2) & TOTP
 ├── text (t)             - Case, statistics, diffing, escaping, & analysis
 ├── date (time)          - Epochs, calendars, zones, workdays, & durations
 ├── file                 - Duplicates, directory trees, regex searching, & type detection
 ├── dev (developer)      - Developer diagnostics (cron, regex testing, colors, semver)
 ├── sys (system)         - Real-time CPU, RAM, env-vars, and process managers
 ├── math (calc)          - Expression evaluator, factors, primes, & Fibonacci
 └── art                  - Borders, boxes, CSV-to-Table, & gradient displays
```

---

## 📖 Complete Command Reference

### 1. `convert` (alias: `conv`)
Data and base conversion utilities.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `encode jwt decode` | `e j d` | Decodes JWT token details (Header, Payload) and extracts human-readable metadata. | `nhash convert encode jwt decode <token>` |
| `encode jwt build` | `e j b` | Generates an unsigned JWT token using specified headers and payloads. | `nhash conv e j build -p "{\"sub\":\"123\"}"` |
| `encode base64` | `e b64` | Encodes/decodes base64 text. | `nhash conv e base64 "hello" -d` |
| `encode base58` | `e b58` | Encodes/decodes base58 representations. | `nhash conv e base58 "hello"` |
| `encode base32` | `e b32` | Encodes/decodes base32 representations. | `nhash conv e base32 "hello"` |
| `encode hex` | `e hex` | Encodes/decodes hex byte arrays. | `nhash conv e hex "hello"` |
| `encode rot13` | `e rot` | Rotates characters by N indices (Caesar shift, default 13). | `nhash conv e rot "hello" -s 13` |
| `encode morse` | `e ms` | Encodes text to Morse code dots and dashes, or decodes it. | `nhash conv e morse "hello"` |
| `encode bintext` | `e bt` | Converts text to/from Binary, Octal, or Decimal values. | `nhash conv e bt "hello" -b 2` |
| `encode base45`/`base91`| `e b45`/`e b91` | Encoders for RFC 9285 Base45 and Bitqueue-based Base91. | `nhash conv e b45 "hello"` |
| `encode punycode` | `e pny` | Encodes/decodes domain names to ASCII Punycode format. | `nhash conv e punycode "مأرب.com"` |
| `encode url`/`html` | - | Standard URL Percent encoding and HTML entity formatting. | `nhash conv e url "hello world"` |
| `format` | - | Formats and converts direct structured configurations (JSON ↔ YAML ↔ XML). | `nhash conv format "{\"a\":1}" -f json -t yaml` |
| `base-n` | - | Converts arbitrary numerical strings between bases 2 to 36. | `nhash conv base-n "1010" -f 2 -t 10` |

---

### 2. `crypto` (alias: `cr`)
High-security cryptography, hashing, and signature tools.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `hash` | - | Multi-hash calculator (MD5, SHA1, SHA256, SHA512, CRC32, Murmur3). | `nhash crypto hash "test"` |
| `hmac` | - | Generates Keyed-Hash Message Authentication Codes. | `nhash crypto hmac "test" -k "secret"` |
| `cipher` | - | Symmetric byte encryption/decryption (AES, DES, TripleDES, RC2). | `nhash crypto cipher "text" -e -k "key" -a aes` |
| `signature keygen` | `sig keygen` | Generates safe PKCS#8 private/public RSA keypairs in `.pem` format. | `nhash crypto sig keygen -s 2048` |
| `signature sign` | `sig sign` | Signs plain text with a private RSA `.pem` key. | `nhash crypto sig sign "data" -k private.pem` |
| `signature verify` | `sig verify` | Verifies RSA signatures using a public RSA `.pem` key. | `nhash crypto sig verify "data" -k public.pem -g <sig>` |
| `password` | - | Generates strong, cryptographically secure passwords or checks strength. | `nhash crypto password -l 16` |

---

### 3. `id`
Identity verification and database primary key identifier tools.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `uuid` | `u` | Generates random UUIDs/GUIDs (v1 to v8), ULIDs, or NanoIDs. | `nhash id uuid -v v7` |
| `inspect` | `ins` | Decodes a UUID to reveal metadata like version, variant, and creation timestamp. | `nhash id inspect <uuid>` |
| `cuid` | - | Generates cryptographically secure CUID2 identifiers. | `nhash id cuid` |
| `snowflake` | - | Generates or decodes Twitter-compatible 64-bit Snowflake IDs. | `nhash id snowflake 12345678` |
| `totp` | `otp` | Generates a 6/8-digit Time-based One-Time Password (RFC 6238) with countdown. | `nhash id totp JBSWY3DPEHPK3PXP` |

---

### 4. `text` (alias: `t`)
Text processing, analysis, and cleaning utilities.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `case` | - | Converts strings between camelCase, snake_case, PascalCase, kebab-case, etc. | `nhash text case "hello world" -c camel` |
| `stats` | - | Counts characters, words, sentences, lines, spaces, and byte sizes. | `nhash text stats "sample content"` |
| `wordfreq` | `wf` | Renders a table of the top N most frequent words in a text. | `nhash text wordfreq "hello world hello"` |
| `count` | `cnt` | Counts the occurrence of exact phrases or regular expressions in text. | `nhash text count "hello world" -p "hello"` |
| `escape` | `esc` | Escapes or unescapes string formats for JSON, C#, SQL, and XML structures. | `nhash text escape "John's Store" -l sql` |
| `diff` | - | Highlights line-by-line differences between two files or strings. | `nhash text diff file1.txt file2.txt` |
| `palindrome` | `pal` | Verifies whether the provided string reads the same forwards and backwards. | `nhash text pal "Racecar"` |
| `slug` | `sl` | Sanitizes diacritics and special characters to build URL-friendly slugs. | `nhash text slug "Hello World!"` |
| `humanize` | - | Translates camel/snake/kebab identifiers to space-separated readable titles. | `nhash text humanize "my_variable"` |
| `lorem` | - | Generates custom paragraphs of Lorem Ipsum placeholder text. | `nhash text lorem -p 3` |
| `json`/`yaml`/`xml` | - | Schema checking, prettifying, and minifying tools. | `nhash text json "{\"a\": 1}" -m` |

---

### 5. `date` (alias: `time`)
Calendar, timezone, and business workday calculations.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `epoch` | - | Translates Unix epoch values to human datetime, or converts datetime to epoch. | `nhash date epoch 1716800000` |
| `timezone` | - | Translates time zones (e.g. converting UTC to Eastern European Time). | `nhash date timezone "12:00" -f UTC -t "Eastern European Time"` |
| `convert` | - | Conversions between Gregorian, Jalali (Shamsi), and Hijri calendars. | `nhash date convert "2026-05-27" -f gregorian -t jalali` |
| `diff` | - | Evaluates time durations and exact intervals between two dates. | `nhash date diff "2026-05-01" "2026-05-27"` |
| `iso` | - | Detailed parsing of ISO 8601 strings into discrete offsets, days, and weeks. | `nhash date iso "2026-05-27T12:00:00Z"` |
| `add` | - | Performs complex duration algebra on datetimes (e.g., `+1y-2m+30d`). | `nhash date add "2026-05-27" -d "+30d"` |
| `workdays` | - | Calculates business working days between two dates, excluding weekends. | `nhash date workdays "2026-05-01" "2026-05-27"` |

---

### 6. `file`
Local filesystem operations and forensic file inspection.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `type` | - | Scans binary file headers (magic bytes) to accurately verify MIME/types. | `nhash file type image.png` |
| `tree` | - | Builds an elegant terminal-based directory tree with sizes and depth controls. | `nhash file tree . -d 2 -s` |
| `dedup` | - | Scans directories for identical files using fast size-grouping and SHA-256 hash checks. | `nhash file dedup .` |
| `search` | - | Runs a blistering fast local regex grep across file contents in a directory. | `nhash file search . -r "TODO.*"` |
| `rename` | - | Performs batch file renaming using regex replacements with visual previews. | `nhash file rename . -p "v1_(.*)" -r "v2_$1" -v` |
| `integrity` | - | Computes SHA-256 checksums and automatically writes sidecar `.sha256` files. | `nhash file integrity document.pdf` |

---

### 7. `dev` (alias: `developer`)
Developer productivity utilities.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `cron` | `cr` | Explains Cron syntax in clear text and calculates the next N executions. | `nhash dev cron "*/5 * * * *"` |
| `regex` | `rg` | Tests regular expressions in real-time against inputs and outputs capture matches. | `nhash dev regex "input text" -p "regex"` |
| `color` | `c` | Multi-format color translator (HEX ↔ RGB ↔ HSL ↔ CMYK) with terminal truecolor swatch previews. | `nhash dev color "#FF5733"` |
| `semver` | `sv` | Compares two semantic versioning identifiers numerically (v1 > v2, v1 < v2, v1 == v2). | `nhash dev semver "1.0.0-beta" "1.0.0"` |
| `number` | `num` / `n` | Multi-base integer/float analyzer (Decimal, Hex, Octal, Binary, Scientific). | `nhash dev number "12345"` |

---

### 8. `sys` (alias: `system`)
Real-time operating system diagnostics.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `info` | `i` | Renders OS, CPU Architecture, Core Count, Total/Free RAM, and Runtime. | `nhash sys info` |
| `env` | `e` | Lists and filters active Environment Variables. | `nhash sys env -f "PATH"` |
| `process` | `ps` / `p` | Lists running background processes sorted by memory consumption. | `nhash sys process -n 10` |

---

### 9. `math` (alias: `calc`)
Mathematical calculators and solvers.

| Command | Aliases | Description | Example |
| :--- | :--- | :--- | :--- |
| `evaluate` | `eval` / `calc` / `c` | Evaluates complex arithmetic expressions natively (+, -, *, /, %, ^, sin, cos, etc.). | `nhash math evaluate "2^3 * (4.5 - 1.5)"` |
| `prime` | `pr` / `p` | Evaluates prime status, perfect number flags, and Fibonacci membership. | `nhash math prime 97` |
| `fibonacci` | `fib` / `f` | Generates lists of the first N Fibonacci sequence numbers. | `nhash math fibonacci -c 10` |
| `factor` | `fac` / `fact` | Factoring integers into prime components. | `nhash math factor 120` |

---

### 10. `art`
Console visual rendering engines.

| Command | Description | Example |
| :--- | :--- | :--- |
| `ascii` | Renders large stylized ascii text banners (uses loaded fonts). | `nhash art ascii "nHash"` |
| `box` | Draws beautiful Spectre Panel boxes around input strings with customizable border styles. | `nhash art box "Welcome to nHash" --title "Greeting"` |
| `table` | Translates comma-separated rows (CSV) into truecolor styled terminal grids. | `nhash art table "Header1,Header2\nRow1,Row2"` |
| `gradient` | Displays texts shaded with custom horizontal hex-color transitions. | `nhash art gradient "Rainbow" -f "#FF0000" -t "#00FF00"` |

---

## 🛠️ Build & Compilation

To build nHash on your machine, clone the repository and use the standard C# dotnet SDK:

### Prerequisites
*   [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Standard Build
```powershell
# Navigate to the workspace src directory
cd nHash\src

# Build the solution
dotnet build -c Release
```

### ⚡ 100% Native AOT (Ahead-of-Time) Compilation
Compiling directly to a Native AOT binary creates a single, self-contained executable that starts instantly and does not require the .NET Runtime on the target machine.

```powershell
# Publish the Console executable as self-contained Native AOT
dotnet publish nHash.Console\nHash.Console.csproj -c Release -r win-x64 --self-contained -p:PublishAot=true
```
*(Replace `win-x64` with your target runtime identifier, e.g. `linux-x64` or `osx-arm64`.)*

---

## 📂 Project Architecture

The solution adheres strictly to **Clean Architecture** principles:
*   `nHash.Console`: Interface layer using `System.CommandLine` for CLI routing and `Spectre.Console` for rendering.
*   `nHash.Application`: Application core defining domain interfaces and containing all business/algorithmic logic.
*   `nHash.Infrastructure`: External client services and third-party integrations (e.g. system API calls, HTTP connections).
*   `nHash.Domain`: Shared entities, common value models, and core configuration records.

---

## 📄 License
This project is licensed under the MIT License.
