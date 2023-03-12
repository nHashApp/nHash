# nHash (inprogress)
Hash utilities with command-line. like: MD5, SHA1, Base64 etc

Download: https://github.com/nRafinia/nHash/releases

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

|Commands|                                                                                                           |
|----|-----------------------------------------------------------------------------------------------------------|
| uuid | Generate Universally unique identifier (UUID/GUID)                                                        |
| url <text> | URL Encode/Decode                                                                                         |
| html <text> | HTML Encode/Decode                                                                                        |
| hash <type> <text> | Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32)                                 |
| base64 <text> | Encode/Decode Base64                                                                                      |
| humanize <type> <text> | Humanizer text (Humanize, Dehumanize, Camel, Hyphenate, Kebab, Pascal, Under-score, Uppercase, Lowercase) | 
 
## Commands
  
### Hash
```
❯ nhash hash --help
Description:
  Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32)

Usage:
  nhash hash [<text>] [options]

Arguments:
  <text>  Text for calculate fingerprint []

Options:
  --file <file>   File name for calculate hash
  --lower         Generate lower case
  -?, -h, --help  Show help and usage information

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
  
## References
* Humanizer: https://github.com/Humanizr/Humanizer 