using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.ValueObjects.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.ValueObjects;

public class CartItemDetailsTests
{
    [Fact(DisplayName = "ToCartItemDetails should calculate total without discount when quantity is less than four")]
    public void ToCartItemDetails_ShouldCalculateTotalWithoutDiscount_WhenQuantityIsLessThanFour()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 3);
        var cartItemDetails = new CartItemDetails(cartItem);

        // When
        cartItemDetails.ToCartItemDetails(product);

        // Then
        cartItemDetails.TotalAmountWithDiscount.Should().Be(300m);
        cartItemDetails.TotalDiscounts.Should().Be(0m);
    }

    [Fact(DisplayName = "ToCartItemDetails should calculate total with 10% discount when quantity is between 4 and 9")]
    public void ToCartItemDetails_ShouldCalculateTotalWith10PercentDiscount_WhenQuantityIsBetween4And9()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 5);
        var cartItemDetails = new CartItemDetails(cartItem);

        // When
        cartItemDetails.ToCartItemDetails(product);

        // Then
        cartItemDetails.TotalAmountWithDiscount.Should().Be(450m); // 5 * 100 * 0.9
        cartItemDetails.TotalDiscounts.Should().Be(-50m); // 5 * 100 - 450
    }

    [Fact(DisplayName = "ToCartItemDetails should calculate total with 20% discount when quantity is between 10 and 20")]
    public void ToCartItemDetails_ShouldCalculateTotalWith20PercentDiscount_WhenQuantityIsBetween10And20()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 15);
        var cartItemDetails = new CartItemDetails(cartItem);

        // When
        cartItemDetails.ToCartItemDetails(product);

        // Then
        cartItemDetails.TotalAmountWithDiscount.Should().Be(1200m); // 15 * 100 * 0.8
        cartItemDetails.TotalDiscounts.Should().Be(-300m); // 15 * 100 - 1200
    }

    [Fact(DisplayName = "ToCartItemDetails should throw exception when quantity is greater than 20")]
    public void ToCartItemDetails_ShouldThrowException_WhenQuantityIsGreaterThan20()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 21);

        // When
        Action act = () => new CartItemDetails(cartItem).ToCartItemDetails(product);

        // Then
        act.Should().Throw<DomainException>()
           .WithMessage("Maximum limit of 20 items per product");
    }

    [Fact(DisplayName = "ToCartItemDetails should calculate total without discount when quantity is exactly four")]
    public void ToCartItemDetails_ShouldCalculateTotalWithoutDiscount_WhenQuantityIsExactlyFour()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 4);
        var cartItemDetails = new CartItemDetails(cartItem);

        // When
        cartItemDetails.ToCartItemDetails(product);

        // Then
        cartItemDetails.TotalAmountWithDiscount.Should().Be(360m); // 4 * 100 * 0.9
        cartItemDetails.TotalDiscounts.Should().Be(-40m); // 4 * 100 - 360
    }

    [Fact(DisplayName = "ToCartItemDetails should calculate total without discount when quantity is exactly ten")]
    public void ToCartItemDetails_ShouldCalculateTotalWithoutDiscount_WhenQuantityIsExactlyTen()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct(100m);
        var cartItem = CarItemTestData.GenerateValidCartItem(product.Id, 10);
        var cartItemDetails = new CartItemDetails(cartItem);

        // When
        cartItemDetails.ToCartItemDetails(product);

        // Then
        cartItemDetails.TotalAmountWithDiscount.Should().Be(800m); // 10 * 100 * 0.8
        cartItemDetails.TotalDiscounts.Should().Be(-200m); // 10 * 100 - 800
    }
}