namespace Generics.Entities;

public class Product : BaseEntity, IValidator
{
    public string Name { get; set; } = string.Empty;

    public bool IsValid() => !string.IsNullOrWhiteSpace(Name);

    public string ValidationError() => "Product name cannot be empty.";
    
    public override string ToLog() => $"[Product] Id={Id} | Name={Name}";
}
