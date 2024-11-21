namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
///  Represents a rating for a product, including the average rate and the number of ratings.
/// </summary>
public record Rating(decimal Rate, int Count)
{
    public static Rating Create(decimal rate, int count) => new Rating(rate, count);
}
