using Generics.Entities;
using Generics.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        // Repository for products
        var productRepository = new InMemoryRepository<Product>();

        productRepository.Add(new Product { Id = 1, Name = "Laptop" });
        productRepository.Add(new Product { Id = 2, Name = "Monitor" });

        var product = productRepository.GetById(1);
        AuditLogger.Log(product!);

        // Repository for orders (same generic logic, different type)
        var orderRepository = new InMemoryRepository<Order>();

        var order = Factory.Create<Order>();
        order.Id = 10;
        order.Amount = 500;

        orderRepository.Add(order);
        AuditLogger.Log(order);

        // Validation
        Validator.Validate(order);

        var invalidProduct = new Product { Id = 3, Name = "" };

        try
        {
            Validator.Validate(invalidProduct);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
        }

        // Nullable values (struct constraint)
        int? stockFromApi = null;
        decimal? discountFromApi = 0.10m;

        var stock = NullableHelper.Unwrap(stockFromApi, fallback: 0);
        var discount = NullableHelper.Unwrap(discountFromApi, fallback: 0m);

        Console.WriteLine($"Stock: {stock}");
        Console.WriteLine($"Discount: {discount:P0}");
    }
}