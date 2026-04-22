# Hotel EF Core

Hotel reservation system demonstrating Entity Framework Core mapping, migrations, AsNoTracking, tracking behavior, and LINQ queries.

## Concepts Covered

### Data Annotations vs Fluent API

Data Annotations: defined directly on the entity and are useful for simple constraints like required fields or string length.

Fluent API: configured in `OnModelCreating` and is better suited for relationships, keys, and more complex rules that shouldn't live inside the entity.

### AsNoTracking vs Tracking

EF Core tracks entities by default so it can detect changes and persist them on `SaveChanges()`.

`AsNoTracking()`: used for read-only queries to avoid unnecessary overhead:

```csharp
var rooms = context.Rooms
  .AsNoTracking()
  .Where(...)
  .ToList();
```

Tracked queries (default): EF Core tracks entities by default, so when we query and modify them, changes are detected and persisted on `SaveChanges()`:

```csharp
var room = context.Rooms.First(r => r.Id == id);
room.IsAvailable = false;
context.SaveChanges();
```

### LINQ Queries

| Method | Purpose |
|--------|---------|
| `.Where` | Filter rows |
| `.Select` | Projection |
| `.Include` | Eager loading |
| `.GroupBy` | Aggregation |
| `.OrderBy` | Sorting |
| `.Any` | Existence check |

### Logging

Uses ILogger for structured logs in read operations, write operations, and error handling.

### Migrations

```bash
dotnet ef migrations add MigrationName
dotnet ef database update

dotnet ef migrations list
```


## Run

```bash
dotnet ef migrations add InitialCreate
dotnet run
```
