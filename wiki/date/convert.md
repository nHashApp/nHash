# ⚡ `nhash date convert` - Multilingual Calendar Translator

Converts calendar dates seamlessly between Gregorian, Jalali (Shamsi), and Hijri systems.

---

## 🛠️ Usage Syntax

```bash
nhash date convert [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<date>` | `string` | Date string to convert (e.g. 2026-05-27 12:00:00 or 1405/03/06). | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--from` | `-f` | `string` | Source calendar type (gregorian, jalali/persian, hijri). | `gregorian` |
| `--to` | `-t` | `string` | Target calendar type (gregorian, jalali/persian, hijri). | `jalali` |

### 💡 Live Terminal Examples

**Convert Gregorian date to Jalali (Persian Shamsi)**:
```bash
nhash date convert "2026-05-27"
```

**Convert Jalali date to Gregorian**:
```bash
nhash date convert "1405/03/06" -f jalali -t gregorian
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash date convert "2026-05-27" --output result.txt
> ```
