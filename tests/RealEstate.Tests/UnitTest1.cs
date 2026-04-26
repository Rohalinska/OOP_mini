using Xunit;
using RealEstate.Domain;

namespace RealEstate.Tests;

public class PropertyTests
{
    [Fact]
    public void Apartment_Commission_IsCorrect()
    {
        // Arrange
        var apt = new Apartment("Test", 100000, 1);
        
        // Act
        var commission = apt.CalculateCommission();
        
        // Assert
        Assert.Equal(5000, commission); // 5% від 100 000
    }

    [Fact]
    public void House_WithNegativePrice_ThrowsException()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => new House("Test", -10, 5));
    }
}