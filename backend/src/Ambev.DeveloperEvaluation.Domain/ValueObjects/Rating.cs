namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
///  Represents a rating for a product, including the average rate and the number of ratings.
/// </summary>
public class Rating
{
    // Rate of the product (e.g., average rating)
    public decimal Rate { get; private set; }

    // Count of ratings (e.g., number of reviews)
    public int Count { get; private set; }

    public Rating()
    {

    }

    /// <summary>
    /// Constructor for the Rating class.
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="count"></param>
    private Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    /// <summary>
    ///  Creates a new instance of the Rating class.
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Rating Create(decimal rate, int count)
    {
        return new Rating(rate, count);
    }
}