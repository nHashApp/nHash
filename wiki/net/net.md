# 🌐 Network Analysis Utilities

Network diagnosis: IP retrievers, DNS lookups, TCP port scans, WHOIS database queries, HTTP pings, SSL cert inspects, and CIDR calculators.

---

> [!NOTE]
> **Category Aliases**: `network`

## 🧩 Subcommands List

| Command | Title | Purpose |
| :--- | :--- | :--- |
| [`ip`](./ip.md) | IP Address Resolver | Queries internal network IPs or external public IP address along with geographic locator info. |
| [`dns`](./dns.md) | DNS Record Lookup | Resolves DNS registry records (A, AAAA, MX, TXT, CNAME, ANY) for a host domain. |
| [`port`](./port.md) | TCP Port Scanner | Tests TCP port connectivity and network socket response from a remote host. |
| [`whois`](./whois.md) | Domain WHOIS Lookup | Retrieves registration and ownership metadata from WHOIS registry servers. |
| [`ping`](./ping.md) | HTTP Latency Ping | Performs raw HTTP request ping to measure response latency and returns HTTP response headers. |
| [`ssl`](./ssl.md) | SSL Certificate Inspector | Inspects SSL/TLS security certificates for a domain and displays validation/expiry details. |
| [`cidr`](./cidr.md) | CIDR Subnet Calculator | Calculates IP boundaries, subnets, masks, broadcasts, and hosts counts from a CIDR notation. |
| [`mac`](./mac.md) | MAC Vendor Lookup | Resolves the hardware OUI vendor/manufacturer of a MAC address. |

---

## 🚀 How to Execute

To call any feature under this module, prepend the category name `net` (or one of its aliases):

```bash
nhash net [subcommand] [arguments] [options]
```
