# 📝 Text & String Processors

Case converters, count patterns, escaping, palindromes, humanizers, Lorem Ipsum placeholders, and JSON/YAML/XML minifiers.

---

> [!NOTE]
> **Category Aliases**: `t`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`case`](./case.md) | String Case Converter | Converts string case formats (camelCase, snake_case, PascalCase, kebab-case, UPPERCASE, lowercase, etc.). |
| [`stats`](./stats.md) | String Metrics Analyzer | Counts characters, words, sentences, spaces, lines, and raw byte sizes. |
| [`wordfreq`](./wordfreq.md) | Word Frequency Analyzer | Renders a table displaying the top N most frequent words in a text payload. |
| [`count`](./count.md) | Phrase / Regex Matches Counter | Counts the occurrence of exact phrases or regular expressions in text. |
| [`escape`](./escape.md) | Data Layout Escaper / Unescaper | Escapes or unescapes strings for layouts including JSON, C#, SQL, HTML, URL, and XML. |
| [`diff`](./diff.md) | Text Diffing Tool | Highlights exact differences (line-by-line) between two files or strings. |
| [`palindrome`](./palindrome.md) | Palindrome Checker | Verifies whether a string reads the same forwards and backwards. |
| [`slug`](./slug.md) | URL Slug Creator | Sanitizes diacritics and special characters to build clean URL-friendly slugs. |
| [`humanize`](./humanize.md) | Developer String Humanizer | Translates camel/snake/kebab developer identifiers into space-separated readable titles. |
| [`lorem`](./lorem.md) | Lorem Ipsum Paragraphs Generator | Generates paragraphs of Lorem Ipsum placeholder text. |
| [`json`](./json.md) | JSON Schema Check & Format Suite | Parses, prettifies, minifies, or validates JSON string documents. |
| [`yaml`](./yaml.md) | YAML Formatting & Minification Suite | Parses, prettifies, minifies, or validates YAML string documents. |
| [`xml`](./xml.md) | XML Document Prettifier & Minifier | Parses, prettifies, minifies, or validates XML string documents. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `text` (or one of its aliases):

```bash
nhash text [subcommand] [arguments] [options]
```
