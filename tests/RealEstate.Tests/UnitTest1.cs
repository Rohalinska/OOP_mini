using Xunit;
using Moq;
using RealEstate.Domain;
using RealEstate.Application;

namespace RealEstate.Tests;

public class PropertyTests
{
    // --- UNIT TESTS (Доменна логіка) ---

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Constructor_ShouldThrow_IfPriceIsInvalid(decimal price)
    {
        Assert.Throws<InvalidPriceException>(() => new Apartment("Київ", price, 1));
    }

    [Fact]
    public void Constructor_ShouldThrow_IfAddressIsEmpty()
    {
        Assert.Throws<InvalidAddressException>(() => new Apartment("", 1000, 1));
    }

    [Fact]
    public void MarkAsSold_ShouldChangeStatus()
    {
        var apt = new Apartment("Київ", 1000, 1);
        apt.MarkAsSold();
        Assert.Equal(PropertyStatus.Sold, apt.Status);
    }

    [Fact]
    public void MarkAsSold_ShouldThrow_IfAlreadySold()
    {
        var apt = new Apartment("Київ", 1000, 1);
        apt.MarkAsSold();
        Assert.Throws<AlreadySoldException>(() => apt.MarkAsSold());
    }

    // --- INTEGRATION TESTS (Робота з файлами) ---

    [Fact]
    public async Task Service_ShouldCorrectlyAddAndRetrieveData()
    {
        // Arrange
        var mockRepo = new Mock<IPropertyRepository>();
        var data = new List<Property>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Property>())).Callback<Property>(p => data.Add(p));
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

        var service = new PropertyService(mockRepo.Object);
        var apt = new Apartment("Тестова 1", 5000, 2);

        // Act
        await service.AddPropertyAsync(apt);
        var result = await service.GetAllPropertiesAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Тестова 1", result[0].Address);
    }

    [Fact]
    public async Task Analytics_TotalValue_ShouldBeCorrect()
    {
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Property> {
            new Apartment("A", 1000, 1),
            new Apartment("B", 2000, 1)
        });

        var service = new PropertyService(mockRepo.Object);
        var total = await service.GetTotalPortfolioValueAsync();

        Assert.Equal(3000, total);
    }
}