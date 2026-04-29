# SQL Mastery

### Window Functions

Window functions perform calculations across a set of rows related to the current row, without collapsing them into a single result (unlike GROUP BY).

They are defined using the `OVER()` clause.

**Example:**
```sql
ROW_NUMBER() OVER (PARTITION BY GuestId ORDER BY CheckIn)
```

**Key concepts:**
- `PARTITION BY` → divides data into groups
- `ORDER BY` → defines calculation order

**Common functions:**
- `ROW_NUMBER()`
- `RANK()`
- `DENSE_RANK()`
- `SUM() OVER()`
- `AVG() OVER()`
- `LAG() / LEAD()`
- `NTILE(N)`
- `FIRST_VALUE() / LAST_VALUE()`

### Pagination

Pagination is the process of splitting a large result set into smaller chunks (pages). Instead of returning all rows, only a subset is retrieved.

**Example:**
```sql
SELECT * FROM Reservations 
ORDER BY CheckIn 
OFFSET (@Page - 1) * @PageSize ROWS 
FETCH NEXT @PageSize ROWS ONLY;
```

**Key points:**
- `ORDER BY` is required
- `OFFSET` skips rows
- `FETCH` returns rows

**Best practice:**
```sql
SELECT COUNT(*) AS TotalRecords, 
    CEILING(COUNT(*) * 1.0 / @PageSize) AS TotalPages 
FROM Reservations;
```

### Transactions

A transaction groups statements into an atomic unit — all succeed or none do.

**Example:**
```sql
BEGIN TRY
  BEGIN TRANSACTION;
  -- statements
  COMMIT;
END TRY
BEGIN CATCH
  ROLLBACK;
END CATCH;
```

### Views vs Functions vs Stored Procedures

| Type | Description |
|------|-------------|
| View | Virtual table |
| Scalar Function | Returns single value |
| Table Function | Returns table |
| Stored Procedure | Executes logic and can modify data |