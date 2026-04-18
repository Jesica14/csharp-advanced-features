namespace Generics.Entities;

public interface IValidator
{
    bool IsValid();
    
    string ValidationError();
}
