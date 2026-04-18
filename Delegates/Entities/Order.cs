namespace Delegates.Entities;

public class Order
{
    public int Id { get; init; }

    public string Customer { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public bool IsPaid { get; init; }
    
    public override string ToString()
    {
        return $"Order #{Id} | {Customer} | {Amount:C} | Paid = {IsPaid}";
    }
}
