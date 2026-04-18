namespace Generics.Entities;

public class Order : BaseEntity, IValidator
{
    public decimal Amount { get; set; }

    public bool IsValid() => Amount > 0;
    public string ValidationError() => "Order amount must be greater than zero.";
    public override string ToLog() => $"[Order] Id={Id} | Amount={Amount:C}";
}
