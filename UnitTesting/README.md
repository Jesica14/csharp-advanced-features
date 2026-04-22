# EntityFramework.Tests

NUnit test project for `EntityFramework` demonstrating three approaches to testing methods with different access levels.

## Approaches covered

### 1. InternalsVisibleTo — testing internal methods

`CalculateTotalPrice` is `internal` instead of `private`. The test project gets access via an assembly attribute in `ReservationService.cs`:

```csharp
[assembly: InternalsVisibleTo("EntityFramework.Tests")]
```

This lets us call `_service.CalculateTotalPrice()` directly without reflection.

### 2. Reflection — testing private methods

`IsValidDateRange` is `private` — accessed via reflection at runtime:

```csharp
var method = typeof(ReservationService)
    .GetMethod("IsValidDateRange", BindingFlags.NonPublic | BindingFlags.Static)
    ?? throw new MissingMethodException("IsValidDateRange not found.");

var result = (bool)method.Invoke(null, [checkIn, checkOut])!;
```

Use this only when refactoring is not an option. It is fragile — if we rename the method, the test breaks at runtime, not compile time.

### 3. Public method tests

Standard unit tests using an InMemory database — no SQL Server needed, isolated per test.

```csharp
var options = new DbContextOptionsBuilder<HotelDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .Options;
```

## When to use each approach

| Approach | Use case |
|---|---|
| Public tests | **Default choice** — test observable behavior through the public API |
| `InternalsVisibleTo` | Internal logic is complex and worth testing directly; we control the codebase |
| Reflection | **Last resort** — cannot modify source code and need to test private members |
