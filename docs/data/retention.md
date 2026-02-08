# Data Retention and Archival

## Requirements
- Audit trails must be retained for **7 years**.
- Event stream retention depends on broker configuration.
- Reporting projections can be rebuilt; their retention can differ.

## Templates
- Orders: retain order history for **3 years**
- Billing ledger: retain for **7 years**
- Approvals: retain for **3 years**

## Deletion and privacy
PII: assume minimal; handle deletion requests by anonymizing PII in read models while retaining audit records in source-of-truth where required by policy.

_Last updated: 2026-02-08_
