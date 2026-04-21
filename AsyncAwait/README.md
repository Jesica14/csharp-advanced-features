# Async/Await Nuances

Order processing system demonstrating async/await patterns, common pitfalls, and the difference between concurrency and parallelism.

## Core concepts

### Threads and Tasks

**Thread** — the smallest unit of execution within a process. Creating threads is expensive; the runtime manages a pool of reusable threads via the ThreadPool.

**Task** — an abstraction over asynchronous work. It allows the runtime to schedule operations without directly managing threads.

**async/await** — does not create new threads. It releases the current thread while waiting for I/O (DB, HTTP, file), allowing it to handle other work.

## Concurrency vs Parallelism

**Concurrency** — multiple tasks in progress at the same time, not necessarily running simultaneously. One thread can manage many I/O operations concurrently.

```csharp
var t1 = GetOrderAsync(1);
var t2 = GetOrderAsync(2);
var t3 = GetOrderAsync(3);
await Task.WhenAll(t1, t2, t3);
```

**Parallelism** — multiple tasks running simultaneously on multiple CPU cores. Requires multiple threads.

```csharp
Parallel.ForEach(orders, order => ProcessOrder(order));
```

| | async/await | Parallel |
|---|---|---|
| Best for | I/O — DB, HTTP, files | CPU-heavy — calculations |
| Creates threads | No — releases and reuses | Yes — ThreadPool |
| Blocks thread | No | Yes |

## Nuances covered

**Exception handling** — exceptions in awaited tasks propagate normally through `try/catch`. Without `await`, exceptions may go unobserved

**Unawaited tasks** — not awaiting a Task means execution continues immediately, which can lead to race conditions

**ConfigureAwait(false)** — prevents capturing the current `SynchronizationContext` (useful in library code to avoid deadlocks)

**async void** — cannot be awaited and exceptions are not observable. Only valid for event handlers (prefer `Task`)

**Task.Run** — used for CPU-bound work to offload execution to the ThreadPool
