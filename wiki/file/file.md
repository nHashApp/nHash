# 📁 Filesystem Operations

Forensic filesystem inspection, directory trees, duplicate file scanning, integrity checks, and regex grep.

---

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`type`](./type.md) | MIME Type & Header Inspector | Scans binary file headers (magic bytes) to verify actual MIME/file types regardless of extension. |
| [`tree`](./tree.md) | Directory Structure Tree Builder | Renders an elegant, terminal-based visual directory tree with depth limits and file sizes. |
| [`dedup`](./dedup.md) | Duplicate File Scanner | Scans directories for identical files using fast size-grouping followed by SHA-256 checks. |
| [`search`](./search.md) | Regex Grep File Scanner | Executes a fast regex pattern grep across all file contents in a target directory. |
| [`rename`](./rename.md) | Batch Regex File Renamer | Performs fast batch file renames in a directory using regex replacements with visual safe previews. |
| [`integrity`](./integrity.md) | SHA-256 Sidecar Integrity Suite | Computes SHA-256 file checksums, verifies hashes, or writes/updates sidecar '.sha256' files. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `file` (or one of its aliases):

```bash
nhash file [subcommand] [arguments] [options]
```
