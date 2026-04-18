# Generics — Constraints

Inventory system demonstrating the 5 most common generic constraints in C#.

## Structure

```
Entities → BaseEntity, IValidator, Product, Order
Services → One class per constraint
```

## Constraints

| Constraint | Class | Why |
|---|---|---|
| `where T : class` | `IRepository<T>` | Entities live on the heap and are shared by reference |
| `where T : BaseEntity` | `AuditLogger`, `InMemoryRepository<T>` | Guarantees access to `Id` and `ToLog()` across all entities |
| `where T : new()` | `Factory` | Allows calling `new T()` without knowing the concrete type |
| `where T : IValidator` | `Validator` | Guarantees `IsValid()` and `ValidationError()` on any entity |
| `where T : struct` | `NullableHelper` | `T?` is always `Nullable<T>`, so `.HasValue` is safe |

## Run

```bash
dotnet run
```
