using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
///  Represents a product in the system with information and ratings.
/// </summary>
public class Product : BaseEntity
{
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public Rating Rating { get; private set; }

    /// <summary>
    /// Only For EF
    /// </summary>
    public Product() { }

    /// <summary>
    ///  Constructor for the Product class.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="price"></param>
    /// <param name="description"></param>
    /// <param name="category"></param>
    /// <param name="image"></param>
    /// <param name="rating"></param>
    private Product(string title, decimal price, string description, string category, string image, Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    /// <summary>
    ///  Creates a new instance of the Product class.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="price"></param>
    /// <param name="description"></param>
    /// <param name="category"></param>
    /// <param name="image"></param>
    /// <param name="rating"></param>
    /// <returns></returns>
    public static Product Create(string title, decimal price, string description, string category, string image, Rating rating)
    {
        return new Product(title, price, description, category, image, rating);
    }

    /// <summary>
    ///  Updates the product's information.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="price"></param>
    /// <param name="description"></param>
    /// <param name="category"></param>
    /// <param name="image"></param>
    /// <param name="rating"></param>
    public void Update(string title, decimal price, string description, string category, string image, Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    /// <summary>
    ///  Updates the product's rating.
    /// </summary>
    /// <param name="rating"></param>
    public void UpdateRating(Rating rating)
    {
        Rating = rating;
    }
}