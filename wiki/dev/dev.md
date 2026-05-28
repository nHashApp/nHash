# 💻 Developer Diagnostics

Real-time developer utilities: Cron schedule tester, color translators, semantic version checkers, and number inspectors.

---

> [!NOTE]
> **Category Aliases**: `developer`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`cron`](./cron.md) | Cron Expression Explainer | Explains standard Cron syntax in plain English and calculates the next N estimated execution schedules. |
| [`regex`](./regex.md) | Regex Match Tester | Tests regular expressions in real-time against custom strings and outputs matching indices and capturing groups. |
| [`color`](./color.md) | Color Format & Swatch Translator | Translates colors instantly between formats (HEX, RGB, HSL, CMYK) with terminal truecolor swatch previews. |
| [`semver`](./semver.md) | Semantic Version Comparator | Compares two semantic versions (SemVer 2.0) logically and determines version hierarchy. |
| [`number`](./number.md) | Number Inspector & Analyzer | Inspects integers and floats to output representations across Decimal, Hex, Octal, Binary, and Scientific layouts. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `dev` (or one of its aliases):

```bash
nhash dev [subcommand] [arguments] [options]
```
