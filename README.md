# nHash

nHash is a lightweight and easy-to-use hashing tool for Windows and Linux that allows users to quickly and securely generate cryptographic hash values for files and text. It supports a variety of popular hash algorithms, including MD5, SHA-1, SHA-256, and more, making it an essential tool for verifying file integrity and ensuring data privacy.

## Features

- **UUID:** Generate a Universally unique identifier (UUID/GUID) version 1 to 5
- Encode
    - **JWT:** JWT token decode
    - **Base64:** Encode/Decode Base64
    - **URL:** URL Encode/Decode
    - **HTML:** HTML Encode/Decode
- Hash
    - **Calc:** Calculate hash fingerprint (MD5, SHA-1, SHA-2 (SHA-256, SHA-384, SHA512), SHA-3, Blake, ...)
    - **Checksum:** Calculate checksum fingerprint (MD5, SHA-1, CRC32, CRC8, Adler-32,...)
- Text
    - **Humanize:** Humanizer text (Pascal-case, Camel-case, Kebab, Underscore, lowercase, etc)
    - **JSON:** JSON tools
    - **YAML:** YAML tools
    - **XML:** XML tools
- **Password:** Generate a random password with custom length, prefix, suffix, character, etc options

## Usage

1. Download and install nHash from the [releases page](https://github.com/nHashApp/nHash/releases/latest) on your Windows or Linux machine.
2. You can view the options by running the nHash application with the `--help`.
3. Use nHash with your desired parameters and command.
4. For more information on sub-command parameters or options, run a command with the `--help` option. For example, `nhash password --help`.

```
❯ nhash --help
Description:
  Hash and Text utilities in command-line mode

Usage:
  nhash [command] [options]

Options:
  --output <output>  File name for writing output
  --version          Show version information
  -?, -h, --help     Show help and usage information

Commands:
  uuid      Generate a Universally unique identifier (UUID/GUID) version 1 to 5
  encode    Encode/Decode features (JWT, Base64, URL, HTML)
  hash      Calculate hash and checksum fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)
  text      Text utilities (Humanizer, JSON, YAML, XML)
  password  Generate a random password with custom length, prefix, suffix, character, etc options

```

## Contributing

If you would like to contribute to nHash, please fork the repository and submit a pull request. We welcome all contributions, including bug fixes, new features, and documentation improvements.

## License

nHash is licensed under the [MIT License](https://github.com/nhash/nhash/blob/master/LICENSE).

## References
* Humanizer: https://github.com/Humanizr/Humanizer 
* MlkPwgen: https://github.com/mkropat/MlkPwgen
* SHA3.Net: https://github.com/dariogriffo/sha3.net
