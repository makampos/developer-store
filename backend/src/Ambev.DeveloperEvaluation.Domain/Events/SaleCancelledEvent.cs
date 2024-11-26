namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleCancelledEvent(int Id)
{
    public static SaleCancelledEvent Build(int id)
    {
        return new SaleCancelledEvent(id);
    }
    public override string ToString()
    {
        return $"Sale Id: {Id}";
    }
}