namespace Delegates.Services;

// Func<decimal, decimal> — takes a price, returns a modified price
public static class PricingService
{
    public static decimal Apply(decimal price, Func<decimal, decimal> rule) => rule(price);

    // Ready-made rules the caller can compose
    public static readonly Func<decimal, decimal> WithTax = p => p * 1.16m;

    public static readonly Func<decimal, decimal> WithDiscount = p => p * 0.90m;
    
    public static readonly Func<decimal, decimal> WithBoth = p => p * 1.16m * 0.90m;
}
