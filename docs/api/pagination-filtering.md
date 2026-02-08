# Pagination and Filtering Standards

## List endpoints
- Use consistent query parameters:
  - `page`, `pageSize` (offset pagination)
  - `sortBy`, `sortOrder`
- Return:
  - `items`
  - `total` (if offset-based)
  - `nextCursor` (if cursor-based)

## Filtering
- Prefer explicit filter params: `status`, `from`, `to`, `customerId`, etc.
- Avoid free-form query unless justified.
