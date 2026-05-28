# 📅 Calendar & Datetime Calculations

Timezone conversions, Epoch conversion, Hijri/Gregorian/Jalali calendar translators, durations add/subtract, and workday indicators.

---

> [!NOTE]
> **Category Aliases**: `time`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`epoch`](./epoch.md) | Unix Epoch Datetime Converter | Converts Unix epoch timestamps to human-readable dates, or human datetimes to Unix epoch. |
| [`convert`](./convert.md) | Multilingual Calendar Translator | Converts calendar dates seamlessly between Gregorian, Jalali (Shamsi), and Hijri systems. |
| [`diff`](./diff.md) | Date Interval & Duration Calculator | Evaluates precise elapsed time intervals, durations, and days count between two input dates. |
| [`timezone`](./timezone.md) | Global Time Zone Converter | Converts date-time inputs dynamically from a source timezone ID to a target timezone ID. |
| [`iso`](./iso.md) | ISO 8601 String Parser | Parses standard ISO 8601 datetime strings to return discrete components, offsets, and indicators. |
| [`add`](./add.md) | Complex Date Algebra Calculator | Performs complex duration additions/subtractions dynamically on top of standard datetimes. |
| [`workdays`](./workdays.md) | Business Workdays Calculator | Calculates the total business working days count (excluding standard weekends) between two dates. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `date` (or one of its aliases):

```bash
nhash date [subcommand] [arguments] [options]
```
