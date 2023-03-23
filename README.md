# ![nHash](images/icon_32.png) nHash
Hash and Text utilities in command-line mode

nHash is a console application that provides various hash and text utilities in command-line mode. It supports generating Universally Unique Identifiers (UUID/GUID), encoding/decoding features such as JWT, Base64, URL, and HTML, calculating hash fingerprints, and performing text utilities such as humanization.

The application is compatible with both Windows and Linux operating systems and supports piping in Linux. It provides a simple and efficient way to perform various text and hash-related tasks on the command-line, without the need for complex scripts or tools.

To get started, simply download the latest release from the [releases page](https://github.com/example/nhash/releases/latest) and run the executable file in a terminal or command prompt. For more information on the available commands and options, refer to the documentation provided in the application or use the --help option.

nHash is an open-source project released under the MIT License. Contributions and feedback are welcome, and can be submitted through the GitHub repository.

```
> nhash --help
Description:
  Hash and Text utilities in command-line mode

Usage:
  nhash [command] [options]

Options:
  --output <output>  File name for writing output
  --version          Show version information
  -?, -h, --help     Show help and usage information

```
## Features

| Commands               |                                                                                       |
|------------------------|---------------------------------------------------------------------------------------|
| [uuid](#uuid)          | Generate a Universally unique identifier (UUID/GUID) version 1 to 5                   |
| [encode](#encode)      | Encode/Decode features (JWT, Base64, URL, HTML)                                       |
| [hash](#hash) <text>   | Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8)       |
| [text](#text)          | Text utilities (Humanizer, JSON, YAML, XML)                                           |
| password               | Generate a random password with custom length, prefix, suffix, character, etc options |
 
## Usage

## UUID
```
❯ nhash uuid --help
Description:
  Generate a Universally unique identifier (UUID/GUID) version 1 to 5

Usage:
  nhash uuid [options]

Options:
  --bracket                       Generate with brackets
  --no-hyphen                     Generate without hyphens
  --version <All|V1|V2|V3|V4|V5>  Select UUID version [default: All]
  --output <output>               File name for writing output
  -?, -h, --help                  Show help and usage information
```  
#### Sample
```
❯ nhash uuid
UUID v1:
247e9324-e636-0003-0000-000001800000
UUID v2:
307e9324-0003-8002-0000-000000000000
UUID v3:
4f366064-c703-af30-9839-02e8cf8006c5
UUID v4:
d6032068-dab5-a04c-a47f-9dde41691d00
UUID v5:
df633114-5cc5-6953-8ef6-4eed9a804629
 
❯ nhash uuid --version v4 --bracket
{898486f4-0447-46ba-9bd1-54e0542069dd}
  
❯ nhash uuid --version v4 --no-hyphen
dcb5d66f6b604e27bd8d7da77156d07d
  
❯ nhash uuid --version v4 --no-hyphen --bracket
{ddee757350644a38bb6486cf6846d66e}  

❯ nhash uuid --version v4 --no-hyphen --bracket --output uuid.txt
```    
---
## Encode
```
❯ nhash encode --help
Description:
  Encode/Decode features (JWT, Base64, URL, HTML)

Usage:
  nhash encode [command] [options]

Options:
  --output <output>  File name for writing output
  -?, -h, --help     Show help and usage information

Commands:
  jwt <token>    JWT token decode (Comply with GDPR rules)
  base64 <text>  Encode/Decode Base64
  url <text>     URL Encode/Decode
  html <text>    HTML Encode/Decode
```

### URL
```
❯ nhash url --help
Description:
  URL Encode/Decode

Usage:
  nhash url <text> [options]

Arguments:
  <text>  text for url encode/decode

Options:
  --decode           Decode url-encoded text
  --output <output>  File name for writing output
  -?, -h, --help     Show help and usage information
```  
#### Sample
```
❯ nhash url https://google.com
https%3a%2f%2fgoogle.com

❯ nhash url https://google.com --output sample.txt
 
❯ nhash url https%3a%2f%2fgoogle.com --decode
https://google.com 
```    
---

### HTML
```
❯ nhash html --help
Description:
  HTML Encode/Decode

Usage:
  nhash html <text> [options]

Arguments:
  <text>  text for html encode/decode

Options:
  --decode           Decode html-encoded text
  --output <output>  File name for writing output
  -?, -h, --help     Show help and usage information
```  
#### Sample
```
❯ nhash html '<html><body><h1>hello</h1></body></html>'
&lt;html&gt;&lt;body&gt;&lt;h1&gt;hello&lt;/h1&gt;&lt;/body&gt;&lt;/html&gt;
 
❯ nhash html '&lt;html&gt;&lt;body&gt;&lt;h1&gt;hello&lt;/h1&gt;&lt;/body&gt;&lt;/html&gt;' --decode
<html><body><h1>hello</h1></body></html>

❯ nhash html '&lt;html&gt;&lt;body&gt;&lt;h1&gt;hello&lt;/h1&gt;&lt;/body&gt;&lt;/html&gt;' --decode --output sample.html
```    
---

### Hash
```
❯ nhash hash --help
Description:
  Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)

Usage:
  nhash hash [<text>] [options]

Arguments:
  <text>  Text for calculate fingerprint []

Options:
  --file <file>                                          File name for calculate hash
  --lower                                                Generate lower case
  --type <All|CRC32|CRC8|MD5|SHA1|SHA256|SHA384|SHA512>  Hash type (MD5, SHA-1, SHA-256,...) [default: All]
  --output <output>                                      File name for writing output
  -?, -h, --help                                         Show help and usage information


```  
#### Sample
```
❯ nhash hash hello
MD5:
5D41402ABC4B2A76B9719D911017C592
SHA-1:
AAF4C61DDCC5E8A2DABEDE0F3B482CD9AEA9434D
SHA-256:
2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824
SHA-384:
59E1748777448C69DE6B800D7A33BBFB9FF1B463E44354C3553BCDB9C666FA90125A3C79F90397BDF5F6A13DE828684F
SHA-512:
9B71D224BD62F3785D96D46AD3EA3D73319BFBC2890CAADAE2DFF72519673CA72323C3D99BA5C11D7C7ACC6E14B8C5DA0C4663475C2E5C3ADEF46F73BCDEC043
CRC32:
3610A686
  
```    
```
❯ nhash hash --file sample.txt
MD5:
B1946AC92492D2347C6235B4D2611184
SHA-1:
F572D396FAE9206628714FB2CE00F72E94F2258F
SHA-256:
5891B5B522D5DF086D0FF0B110FBD9D21BB4FC7163AF34D08286A2E846F6BE03
SHA-384:
1D0F284EFE3EDEA4B9CA3BD514FA134B17EAE361CCC7A1EEFEFF801B9BD6604E01F21F6BF249EF030599F0C218F2BA8C
SHA-512:
E7C22B994C59D9CF2B48E549B1E24666636045930D3DA7C1ACB299D1C3B7F931F94AAE41EDDA2C2B207A36E10F8BCB8D45223E54878F5B316E7CE3B6BC019629
CRC32:
363A3020
    
```    
```
❯ cat sample.txt | nhash hash
MD5:
B1946AC92492D2347C6235B4D2611184
SHA-1:
F572D396FAE9206628714FB2CE00F72E94F2258F
SHA-256:
5891B5B522D5DF086D0FF0B110FBD9D21BB4FC7163AF34D08286A2E846F6BE03
SHA-384:
1D0F284EFE3EDEA4B9CA3BD514FA134B17EAE361CCC7A1EEFEFF801B9BD6604E01F21F6BF249EF030599F0C218F2BA8C
SHA-512:
E7C22B994C59D9CF2B48E549B1E24666636045930D3DA7C1ACB299D1C3B7F931F94AAE41EDDA2C2B207A36E10F8BCB8D45223E54878F5B316E7CE3B6BC019629
CRC32:
363A3020
```
```
❯ nhash hash --file sample.txt --type md5
B1946AC92492D2347C6235B4D2611184

❯ nhash hash --file sample.txt --type sha1 --output crc.txt

```
---

### Base64
```
❯ nhash base64 --help
Description:
  Encode/Decode Base64

Usage:
  nhash base64 <text> [options]

Arguments:
  <text>  text for encode/decode Base64

Options:
  --decode        Decode Base64 text
  -?, -h, --help  Show help and usage information
```  
#### Sample
```
❯ nhash base64 hello
aGVsbG8=
 
❯ nhash base64 aGVsbG8= --decode
hello

❯ nhash base64 hello | nhash base64 --decode
hello
```    
---

## Text
```
❯ nhash text --help
Description:
  Text utilities (Humanizer)

Usage:
  nhash text [command] [options]

Options:
  -?, -h, --help  Show help and usage information

Commands:
  humanize                                              Humanizer text (Pascal-case, Camel-case, Kebab, 
  <Camel|Dehumanize|Humanize|Hyphenate|Kebab|Lowercase  Underscore, lowercase, etc)
  |Pascal|Underscore|Uppercase> <text>
```

### Humanize
```
❯ nhash text humanize --help
Description:
  Humanizer text (Pascal-case, Camel-case, Kebab, Underscore, lowercase, etc)

Usage:
  nhash text humanize <type> <text> [options]

Arguments:
  <Camel|Dehumanize|Humanize|Hyphenate|Kebab|Lowercase|Pascal|Underscore|Uppercase>  Humanize type
  <text>                                                                             Text for humanize

Options:
  -?, -h, --help  Show help and usage information
```
#### Sample
```
❯ nhash text humanize camel "hello world"
helloWorld

❯ nhash text humanize kebab "hello world"
hello-world

❯ nhash text humanize pascal "hello world"
HelloWorld

❯ nhash text humanize uppercase "hello world"
HELLO WORLD

```
---
  
## References
* Humanizer: https://github.com/Humanizr/Humanizer 
* MlkPwgen: https://github.com/mkropat/MlkPwgen
