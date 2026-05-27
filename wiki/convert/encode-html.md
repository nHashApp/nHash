# ⚡ `nhash convert encode html` - HTML Entity Encoder / Decoder

Performs standard HTML escaping (entities) or unescapes it.

---

> [!NOTE]
> **Subcommand Aliases**: `h`

## 🛠️ Usage Syntax

```bash
nhash convert encode html [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | Text containing special chars to encode or entities to decode. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--decode` | `-d` | `bool` | Decode HTML-encoded entities instead of encoding. | `false` |

### 💡 Live Terminal Examples

**HTML encode sensitive markup characters**:
```bash
nhash convert encode html "<div>hello</div>"
```

**HTML decode entities**:
```bash
nhash convert encode html "&lt;div&gt;hello&lt;/div&gt;" -d
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash convert encode html "<div>hello</div>" --output result.txt
> ```
