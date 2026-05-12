using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RealEstate.Domain;

public enum PropertyStatus { Available, Sold }

// Патерн Strategy
public interface ICommissionStrategy 
{ 
    string Name { get; }
    decimal Calculate(decimal price); 
}

public class StandardCommission : ICommissionStrategy 
{ 
    public string Name => "Стандарт (5%)";
    public decimal Calculate(decimal price) => price * 0.05m; 
}

public abstract class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Address { get; set; }
    public decimal Price { get; set; }
    public PropertyStatus Status { get; set; } = PropertyStatus.Available;

    protected Property() { } // Для JSON

    protected Property(string address, decimal price)
    {
        if (price <= 0) throw new ArgumentException("Ціна має бути > 0");
        Address = address;
        Price = price;
    }

    public void MarkAsSold() 
    {
        if (Status == PropertyStatus.Sold) throw new InvalidOperationException("Вже продано!");
        Status = PropertyStatus.Sold;
    }
}

public class Apartment : Property
{
    public int Floor { get; set; }
    public Apartment() { }
    public Apartment(string a, decimal p, int f) : base(a, p) => Floor = f;
}

public class House : Property
{
    public double YardSize { get; set; }
    public House() { }
    public House(string a, decimal p, double y) : base(a, p) => YardSize = y;
}

public interface IPropertyRepository
{
    Task<List<Property>> GetAllAsync();
    Task AddAsync(Property property);
    Task SaveAsync();
}