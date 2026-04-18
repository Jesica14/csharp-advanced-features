# Delegates & Lambdas

Order system demonstrating delegates, Func, Action, Predicate, multicast delegates, and events in C#.

## Structure

```
Entities → Order
Services → One class per delegate type
```

## Concepts

| Concept         | Class               | Purpose                                      |
|-----------------|---------------------|----------------------------------------------|
| Custom delegate | `LogFormatter`      | Reusable method signature for runtime swaps  |
| `Func<T, TResult>` | `PricingService` | Inject pricing rules dynamically             |
| `Action<T>`     | `NotificationService` | Chain multiple handlers (log, email, audit) |
| `Predicate<T>`  | `OrderFilter`       | Simplify collection filtering                |
| `event`         | `OrderService`      | Notify subscribers safely                    |

## Key Differences

- **`Func`**: Returns a value (transformations).
- **`Action`**: No return value (logging, saving).
- **`Predicate`**: Returns `bool` for filtering ( equivalent to `Func<T, bool>`).
- **`event`**: Multicast delegate with restricted access (subscribe/unsubscribe only).
