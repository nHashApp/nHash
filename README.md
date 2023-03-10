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
  
### Hash
```
❯ nhash hash --help
Description:
  Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512)

Usage:
  nhash hash <type> <text> [options]

Arguments:
  <MD5|SHA1|SHA256|SHA384|SHA512>  Algorithm type
  <text>                           Text for calculate fingerprint

Options:
  --file <file>   File name for calculate hash
  --lower         Generate lower case
  -?, -h, --help  Show help and usage information
```  
#### Sample
```
❯ nhash hash md5 hello
5D41402ABC4B2A76B9719D911017C592
    
❯ nhash hash sha1 hello
AAF4C61DDCC5E8A2DABEDE0F3B482CD9AEA9434D
    
❯ nhash hash sha256 hello
2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824
    
❯ nhash hash sha384 hello
59E1748777448C69DE6B800D7A33BBFB9FF1B463E44354C3553BCDB9C666FA90125A3C79F90397BDF5F6A13DE828684F
    
❯ nhash hash sha512 hello
9B71D224BD62F3785D96D46AD3EA3D73319BFBC2890CAADAE2DFF72519673CA72323C3D99BA5C11D7C7ACC6E14B8C5DA0C4663475C2E5C3ADEF46F73BCDEC043
    
```    

```
    
```    
    
    
---
    
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
