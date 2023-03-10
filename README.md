# nHash
Hash utilities with command-line. like: MD5, SHA1, Base64 etc


```
> nhash --help
Description:
  Hash and Text utilities in command-line mode

Usage:
  nhash [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

```

|Commands|    |
|----|----|
| uuid | Generate Universally unique identifier (UUID/GUID) |
| url <text> | URL Encode/Decode |
| html <text> | HTML Encode/Decode |
| hash <type> <text> | Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512) |
| base64 <text> | Encode/Decode Base64 |
| humanize <type> <text> | Humanizer text (Humanize, Dehumanize, Camel, Hyphenate, Kebab, Pascal, Under-score, Uppercase, Lowercase) | 
 
## Commands
  
### UUID

 
```
❯ nhash uuid --help
Description:
  Generate Universally unique identifier (UUID/GUID)

Usage:
  nhash uuid [options]

Options:
  --bracket       Generate with brackets
  --no-hyphen     Generate without hyphens
  -?, -h, --help  Show help and usage information
  
```  
#### Sample  
```
❯ nhash uuid
29bee9e6-8eab-477b-b643-e2699f0029db
  
❯ nhash uuid --bracket
{898486f4-0447-46ba-9bd1-54e0542069dd}
  
❯ nhash uuid --no-hyphen
dcb5d66f6b604e27bd8d7da77156d07d
  
❯ nhash uuid --no-hyphen --bracket
{ddee757350644a38bb6486cf6846d66e}  
```  
  
Download: https://github.com/nRafinia/nHash/releases
