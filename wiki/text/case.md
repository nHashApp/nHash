# ⚡ `nhash text case` - String Case Converter

Converts string case formats (camelCase, snake_case, PascalCase, kebab-case, UPPERCASE, lowercase, etc.).

---

## 🛠️ Usage Syntax

```bash
nhash text case [arguments] [options]
```

### 📥 Input Arguments

| Argument Name | Type | Description | Required |
| :--- | :--- | :--- | :--- |
| `<text>` | `string` | The input text payload. | ✅ Yes |

### ⚙️ Options & Parameters

| Option Flag | Short Flag | Type | Description | Default Value |
| :--- | :--- | :--- | :--- | :--- |
| `--type` | `-c` | `string` | Case style: camel, pascal, snake, kebab, upper, lower, title, train, sentence, alternating, inverse, random, slug. | `N/A` |

### 💡 Live Terminal Examples

**Convert text to CamelCase**:
```bash
nhash text case "hello world" -c camel
```

**Convert text to SnakeCase**:
```bash
nhash text case "hello world" -c snake
```


---

> [!TIP]
> All subcommands in **nHash** inherit the global `--output` (`-o`) redirection parameter to save execution results directly to a local file.
> ```bash
> nhash text case "hello world" -c camel --output result.txt
> ```
