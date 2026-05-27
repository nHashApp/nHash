# 🖥️ System Diagnostics

Operating system and process telemetry: hardware details, environment variables, and background process lists.

---

> [!NOTE]
> **Category Aliases**: `system`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`info`](./info.md) | Telemetry Summary | Outputs a beautiful Spectre panel showing CPU Architecture, cores count, RAM, OS, and Runtime version. |
| [`env`](./env.md) | Environment Variables Viewer | Lists and filters active system Environment Variables. |
| [`process`](./process.md) | Process telemetry manager | Lists active background processes sorted by RAM memory consumption. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `sys` (or one of its aliases):

```bash
nhash sys [subcommand] [arguments] [options]
```
